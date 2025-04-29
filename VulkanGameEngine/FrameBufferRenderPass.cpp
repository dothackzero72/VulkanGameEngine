#include "FrameBufferRenderPass.h"
#include <CVulkanRenderer.h>
#include <stdexcept>
#include <VulkanRenderPass.h>
#include "RenderSystem.h"
#include "JsonPipeline.h"

JsonRenderPass::JsonRenderPass()
{
}

JsonRenderPass::JsonRenderPass(uint renderPassIndex, const String& jsonPath, Texture& inputTexture, ivec2 renderPassResolution)
{
    RenderPassId = renderPassIndex;
    RenderPassResolution = renderPassResolution;
    SampleCount = VK_SAMPLE_COUNT_1_BIT;
    FrameBufferList.resize(cRenderer.SwapChain.SwapChainImageCount);

    nlohmann::json json = Json::ReadJson(jsonPath);
    RenderPassBuildInfoModel renderPassBuildInfo = RenderPassBuildInfoModel::from_json(json, renderPassResolution);
    BuildRenderPass(renderPassBuildInfo);
    BuildFrameBuffer(renderPassBuildInfo);

    GPUImport import = GPUImport{ .TextureList = Vector<Texture> { inputTexture } };
    Vector<VkVertexInputBindingDescription> vertexBinding = NullVertex::GetBindingDescriptions();
    Vector<VkVertexInputAttributeDescription> vertexAttribute = NullVertex::GetAttributeDescriptions();

    renderSystem.RenderPipelineList[RenderPassId].emplace_back(JsonPipeline(0, "../Pipelines/FrameBufferPipeline.json", RenderPass, import, vertexBinding, vertexAttribute, sizeof(SceneDataBuffer), renderPassResolution));

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

JsonRenderPass::~JsonRenderPass()
{
}

void JsonRenderPass::Update(const float& deltaTime)
{
}

void JsonRenderPass::BuildRenderPipelines(const RenderPassBuildInfoModel& renderPassBuildInfo, GPUImport& renderGraphics, SceneDataBuffer& sceneDataBuffer)
{
    for (int x = 0; x < renderPassBuildInfo.RenderPipelineList.size(); x++)
    {
        Vector<VkVertexInputBindingDescription> vertexBinding = NullVertex::GetBindingDescriptions();
        Vector<VkVertexInputAttributeDescription> vertexAttribute = NullVertex::GetAttributeDescriptions();
        renderSystem.RenderPipelineList[RenderPassId] = Vector<JsonPipeline>{ JsonPipeline(1,renderPassBuildInfo.RenderPipelineList[x], RenderPass, renderGraphics, vertexBinding, vertexAttribute, sizeof(SceneDataBuffer), RenderPassResolution) };
    }
}

void JsonRenderPass::BuildRenderPass(const RenderPassBuildInfoModel& renderPassBuildInfo)
{
    Vector<RenderedTexture> RenderedColorTextureList;
    for (auto& texture : renderPassBuildInfo.RenderedTextureInfoModelList)
    {
        VkImageCreateInfo imageCreateInfo = texture.ImageCreateInfo;
        VkSamplerCreateInfo samplerCreateInfo = texture.SamplerCreateInfo;
        switch (texture.TextureType)
        {
            case ColorRenderedTexture: renderSystem.RenderedTextureList[RenderPassId].emplace_back(RenderedTexture(VK_IMAGE_ASPECT_COLOR_BIT, imageCreateInfo, samplerCreateInfo)); break;
            case InputAttachmentTexture: renderSystem.RenderedTextureList[RenderPassId].emplace_back(RenderedTexture(VK_IMAGE_ASPECT_COLOR_BIT, imageCreateInfo, samplerCreateInfo)); break;
            case ResolveAttachmentTexture: renderSystem.RenderedTextureList[RenderPassId].emplace_back(RenderedTexture(VK_IMAGE_ASPECT_COLOR_BIT, imageCreateInfo, samplerCreateInfo)); break;
            case DepthRenderedTexture: renderSystem.DepthTextureList[RenderPassId] = DepthTexture(imageCreateInfo, samplerCreateInfo); break;
            default:
            {
                throw std::runtime_error("Case doesn't exist: RenderedTextureType");
            }
        };
    }

    RenderPass = RenderPass_BuildRenderPass(cRenderer.Device, renderPassBuildInfo);
}

void JsonRenderPass::BuildFrameBuffer(const RenderPassBuildInfoModel& renderPassBuildInfo)
{
    Vector<VkImageView> imageViewList;
    for (int x = 0; x < renderSystem.RenderedTextureList.size(); x++)
    {
        imageViewList.emplace_back(renderSystem.RenderedTextureList[RenderPassId][x].View);
    }

    SharedPtr<VkImageView> depthTextureView = nullptr;
    if (renderSystem.DepthTextureList.find(RenderPassId) != renderSystem.DepthTextureList.end())
    {
        depthTextureView = std::make_shared<VkImageView>(renderSystem.DepthTextureList[RenderPassId].View);
    }

    VkRenderPass& renderPass = RenderPass;
    FrameBufferList = RenderPass_BuildFrameBuffer(cRenderer.Device, renderPass, renderPassBuildInfo, imageViewList, depthTextureView.get(), cRenderer.SwapChain.SwapChainImageViews, RenderPassResolution);
}

VkCommandBuffer JsonRenderPass::DrawFrameBuffer()
{
    RenderPassInfo.clearValueCount = static_cast<uint32>(ClearValueList.size());
    RenderPassInfo.pClearValues = ClearValueList.data();
    RenderPassInfo.framebuffer = FrameBufferList[cRenderer.ImageIndex];

    VkCommandBufferBeginInfo CommandBufferBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
        .flags = VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
    };

    VULKAN_RESULT(vkResetCommandBuffer(renderSystem.CommandBuffer, 0));
    VULKAN_RESULT(vkBeginCommandBuffer(renderSystem.CommandBuffer, &CommandBufferBeginInfo));
    vkCmdBeginRenderPass(renderSystem.CommandBuffer, &RenderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
    vkCmdBindPipeline(renderSystem.CommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, renderSystem.RenderPipelineList[RenderPassId][0].Pipeline);
    vkCmdBindDescriptorSets(renderSystem.CommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, renderSystem.RenderPipelineList[RenderPassId][0].PipelineLayout, 0, renderSystem.RenderPipelineList[RenderPassId][0].DescriptorSetList.size(), renderSystem.RenderPipelineList[RenderPassId][0].DescriptorSetList.data(), 0, nullptr);
    vkCmdDraw(renderSystem.CommandBuffer, 6, 1, 0, 0);
    vkCmdEndRenderPass(renderSystem.CommandBuffer);
    vkEndCommandBuffer(renderSystem.CommandBuffer);
    return renderSystem.CommandBuffer;
}

VkCommandBuffer JsonRenderPass::Draw(Vector<SharedPtr<GameObject>> meshList, SceneDataBuffer& sceneDataBuffer)
{
    RenderPassInfo.clearValueCount = static_cast<uint32>(ClearValueList.size());
    RenderPassInfo.pClearValues = ClearValueList.data();
    RenderPassInfo.framebuffer = FrameBufferList[cRenderer.ImageIndex];

    VkCommandBufferBeginInfo CommandBufferBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
        .flags = VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
    };

    VULKAN_RESULT(vkResetCommandBuffer(renderSystem.CommandBuffer, 0));
    VULKAN_RESULT(vkBeginCommandBuffer(renderSystem.CommandBuffer, &CommandBufferBeginInfo));
    vkCmdBeginRenderPass(renderSystem.CommandBuffer, &RenderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
    for (auto mesh : meshList)
    {
        mesh->Draw(renderSystem.CommandBuffer, renderSystem.RenderPipelineList[RenderPassId][0].Pipeline, renderSystem.RenderPipelineList[RenderPassId][0].PipelineLayout, renderSystem.RenderPipelineList[RenderPassId][0].DescriptorSetList);
    }
    vkCmdEndRenderPass(renderSystem.CommandBuffer);
    vkEndCommandBuffer(renderSystem.CommandBuffer);
    return renderSystem.CommandBuffer;
}

void JsonRenderPass::Destroy()
{
    for (auto renderedTexture : renderSystem.RenderedTextureList[RenderPassId])
    {
        renderedTexture.Destroy();
    }

    renderer.DestroyRenderPass(RenderPass);
    renderer.DestroyCommandBuffers(renderSystem.CommandBuffer);
    renderer.DestroyFrameBuffers(FrameBufferList);
    FrameBufferList.clear();
}