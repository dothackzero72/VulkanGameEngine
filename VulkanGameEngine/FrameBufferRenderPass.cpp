#include "FrameBufferRenderPass.h"
#include <CVulkanRenderer.h>
#include <stdexcept>


FrameBufferRenderPass::FrameBufferRenderPass()
{
}

FrameBufferRenderPass::FrameBufferRenderPass(const String& jsonPath, SharedPtr<Texture> inputTexture, ivec2 renderPassResolution)
{
    RenderPassResolution = renderPassResolution;
    SampleCount = VK_SAMPLE_COUNT_1_BIT;
    FrameBufferList.resize(cRenderer.SwapChain.SwapChainImageCount);

    VULKAN_RESULT(renderer.CreateCommandBuffer(CommandBuffer));

    nlohmann::json json = Json::ReadJson(jsonPath);
    RenderPassBuildInfoModel renderPassBuildInfo = RenderPassBuildInfoModel::from_json(json, renderPassResolution);
    BuildRenderPass(renderPassBuildInfo);
    BuildFrameBuffer(renderPassBuildInfo);

    GPUImport import = GPUImport{ .TextureList = Vector<SharedPtr<Texture>> { inputTexture } };
    Vector<VkVertexInputBindingDescription> vertexBinding = NullVertex::GetBindingDescriptions();
    Vector<VkVertexInputAttributeDescription> vertexAttribute = NullVertex::GetAttributeDescriptions();
    JsonPipelineList.emplace_back(std::make_shared<JsonPipeline>(JsonPipeline("../Pipelines/FrameBufferPipeline.json", RenderPass, import, vertexBinding, vertexAttribute, sizeof(SceneDataBuffer), renderPassResolution)));
}

FrameBufferRenderPass::~FrameBufferRenderPass()
{
}

VkCommandBuffer FrameBufferRenderPass::Draw()
{
    std::vector<VkClearValue> clearValues
    {
        VkClearValue{.color = { {1.0f, 1.0f, 1.0f, 1.0f} } }
    };

    VkRenderPassBeginInfo renderPassInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = RenderPass,
        .framebuffer = FrameBufferList[cRenderer.ImageIndex],
        .renderArea
        {
            .offset = {0, 0},
            .extent =
            {
                .width = static_cast<uint32>(RenderPassResolution.x),
                .height = static_cast<uint32>(RenderPassResolution.y)
            }
        },
        .clearValueCount = static_cast<uint32>(clearValues.size()),
        .pClearValues = clearValues.data()
    };

    VkCommandBufferBeginInfo CommandBufferBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
        .flags = VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
    };

    VULKAN_RESULT(vkResetCommandBuffer(CommandBuffer, 0));
    VULKAN_RESULT(vkBeginCommandBuffer(CommandBuffer, &CommandBufferBeginInfo));
    vkCmdBeginRenderPass(CommandBuffer, &renderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
    vkCmdBindPipeline(CommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, JsonPipelineList[0]->Pipeline);
    vkCmdBindDescriptorSets(CommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, JsonPipelineList[0]->PipelineLayout, 0, JsonPipelineList[0]->DescriptorSetList.size(), JsonPipelineList[0]->DescriptorSetList.data(), 0, nullptr);
    vkCmdDraw(CommandBuffer, 6, 1, 0, 0);
    vkCmdEndRenderPass(CommandBuffer);
    vkEndCommandBuffer(CommandBuffer);
    return CommandBuffer;
}
