#include "FrameBufferRenderPass.h"
#include <CVulkanRenderer.h>
#include <stdexcept>


FrameBufferRenderPass::FrameBufferRenderPass()
{
}

FrameBufferRenderPass::FrameBufferRenderPass(const String& jsonPath, Texture& inputTexture, ivec2 renderPassResolution)
{
    RenderPassResolution = renderPassResolution;
    SampleCount = VK_SAMPLE_COUNT_1_BIT;
    FrameBufferList.resize(cRenderer.SwapChain.SwapChainImageCount);

    VULKAN_RESULT(renderer.CreateCommandBuffer(CommandBuffer));

    nlohmann::json json = Json::ReadJson(jsonPath);
    RenderPassBuildInfoModel renderPassBuildInfo = RenderPassBuildInfoModel::from_json(json, renderPassResolution);
    BuildRenderPass(renderPassBuildInfo);
    BuildFrameBuffer(renderPassBuildInfo);

    GPUImport import = GPUImport{ .TextureList = Vector<Texture> { inputTexture } };
    Vector<VkVertexInputBindingDescription> vertexBinding = NullVertex::GetBindingDescriptions();
    Vector<VkVertexInputAttributeDescription> vertexAttribute = NullVertex::GetAttributeDescriptions();
    JsonPipelineList.emplace_back(JsonPipeline("../Pipelines/FrameBufferPipeline.json", RenderPass, import, vertexBinding, vertexAttribute, sizeof(SceneDataBuffer), renderPassResolution));
    ClearValueList = renderPassBuildInfo.ClearValueList;
    RenderPassInfo = VkRenderPassBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = RenderPass,
        .framebuffer = FrameBufferList[cRenderer.ImageIndex],
        .renderArea = renderPassBuildInfo.RenderArea.RenderArea,
        .clearValueCount = static_cast<uint32>(ClearValueList.size()),
        .pClearValues = ClearValueList.data()
    };
}

FrameBufferRenderPass::~FrameBufferRenderPass()
{
}
