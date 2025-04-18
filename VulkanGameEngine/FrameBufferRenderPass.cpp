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
