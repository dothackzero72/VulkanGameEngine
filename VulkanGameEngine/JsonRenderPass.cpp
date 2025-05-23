#include "JsonRenderPass.h"
#include "CVulkanRenderer.h"
#include <stdexcept>
#include "RenderSystem.h"
#include <VulkanPipeline.h>
#include "GameSystem.h"
#include "TextureSystem.h"
#include "json.h"

JsonRenderPass::JsonRenderPass()
{
}

JsonRenderPass::JsonRenderPass(VkGuid& levelId, RenderPassBuildInfoModel& model, ivec2& renderPassResolution)
{
    RenderPassId = model.RenderPassId;
    SampleCount = VK_SAMPLE_COUNT_1_BIT;

    renderSystem.renderPassBuildInfoList[RenderPassId] = model;
    renderSystem.RenderPassResolutionList[RenderPassId] = renderPassResolution;
    UseFrameBufferResolution = renderSystem.renderPassBuildInfoList[RenderPassId].IsRenderedToSwapchain;

    BuildRenderPass(renderSystem.renderPassBuildInfoList[RenderPassId]);
    BuildFrameBuffer(renderSystem.renderPassBuildInfoList[RenderPassId]);
    BuildCommandBuffer();

    for (auto& renderPipeline : model.RenderPipelineList)
    {
        uint id = renderSystem.RenderPipelineList.size();
        nlohmann::json json = Json::ReadJson(renderPipeline);
        RenderPipelineModel renderPipelineModel = RenderPipelineModel::from_json(json);
        GPUIncludes include =
        {
            .vertexProperties = renderSystem.GetVertexPropertiesBuffer(),
            .indexProperties = renderSystem.GetIndexPropertiesBuffer(),
            //        .transformProperties = renderSystem.GetTransformPropertiesBuffer(gpuImport.MeshList),
            .meshProperties = renderSystem.GetMeshPropertiesBuffer(levelId),
            .texturePropertiesList = renderSystem.GetTexturePropertiesBuffer(RenderPassId, textureSystem.InputTextureList[id]),
            .materialProperties = renderSystem.GetMaterialPropertiesBuffer()
        };
        Vector<VkPipelineShaderStageCreateInfo> pipelineShaderStageCreateInfoList = Vector<VkPipelineShaderStageCreateInfo>
        {
            shaderSystem.CreateShader(cRenderer.Device, renderPipelineModel.VertexShaderPath, VK_SHADER_STAGE_VERTEX_BIT),
            shaderSystem.CreateShader(cRenderer.Device, renderPipelineModel.FragmentShaderPath, VK_SHADER_STAGE_FRAGMENT_BIT)
        };
        renderSystem.RenderPipelineList[RenderPassId].emplace_back(Pipeline_CreateRenderPipeline(cRenderer.Device, RenderPassId, id, renderPipelineModel, RenderPass, sizeof(SceneDataBuffer), renderPassResolution, include, pipelineShaderStageCreateInfoList));
    }
    renderSystem.SpriteBatchLayerList[RenderPassId].emplace_back(SpriteBatchLayer(RenderPassId));

    renderSystem.ClearValueList[RenderPassId] = renderSystem.renderPassBuildInfoList[RenderPassId].ClearValueList;
    renderArea = renderSystem.renderPassBuildInfoList[RenderPassId].RenderArea.RenderArea;
}

JsonRenderPass::JsonRenderPass(VkGuid& levelId, RenderPassBuildInfoModel& model, Texture& inputTexture, ivec2& renderPassResolution)
{
    RenderPassId = model.RenderPassId;
    SampleCount = VK_SAMPLE_COUNT_1_BIT;

    renderSystem.renderPassBuildInfoList[RenderPassId] = model;
    renderSystem.RenderPassResolutionList[RenderPassId] = renderPassResolution;
    UseFrameBufferResolution = renderSystem.renderPassBuildInfoList[RenderPassId].IsRenderedToSwapchain;

    BuildRenderPass(renderSystem.renderPassBuildInfoList[RenderPassId]);
    BuildFrameBuffer(renderSystem.renderPassBuildInfoList[RenderPassId]);
    BuildCommandBuffer();

    uint id = renderSystem.RenderPipelineList.size();
    nlohmann::json json = Json::ReadJson(model.RenderPipelineList[0]);
    RenderPipelineModel renderPipelineModel = RenderPipelineModel::from_json(json);
    textureSystem.InputTextureList[id].emplace_back(std::make_shared<Texture>(inputTexture));

    GPUIncludes include =
    {
        .vertexProperties = renderSystem.GetVertexPropertiesBuffer(),
        .indexProperties = renderSystem.GetIndexPropertiesBuffer(),
        //        .transformProperties = renderSystem.GetTransformPropertiesBuffer(gpuImport.MeshList),
        .meshProperties = renderSystem.GetMeshPropertiesBuffer(levelId),
        .texturePropertiesList = renderSystem.GetTexturePropertiesBuffer(RenderPassId, textureSystem.InputTextureList[id]),
        .materialProperties = renderSystem.GetMaterialPropertiesBuffer()
    };


    Vector<VkPipelineShaderStageCreateInfo> pipelineShaderStageCreateInfoList = Vector<VkPipelineShaderStageCreateInfo>
    {
        shaderSystem.CreateShader(cRenderer.Device, renderPipelineModel.VertexShaderPath, VK_SHADER_STAGE_VERTEX_BIT),
        shaderSystem.CreateShader(cRenderer.Device, renderPipelineModel.FragmentShaderPath, VK_SHADER_STAGE_FRAGMENT_BIT)
    };


    renderSystem.RenderPipelineList[RenderPassId].emplace_back(Pipeline_CreateRenderPipeline(cRenderer.Device, RenderPassId, id, renderPipelineModel, RenderPass, sizeof(SceneDataBuffer), renderPassResolution, include, pipelineShaderStageCreateInfoList));
    renderArea = renderSystem.renderPassBuildInfoList[RenderPassId].RenderArea.RenderArea;
    renderSystem.ClearValueList[RenderPassId] = renderSystem.renderPassBuildInfoList[RenderPassId].ClearValueList;
}

JsonRenderPass::~JsonRenderPass()
{
}

void JsonRenderPass::RecreateSwapchain(int newWidth, int newHeight)
{
    renderer.DestroyRenderPass(RenderPass);
    renderer.DestroyCommandBuffers(CommandBuffer);
    renderer.DestroyFrameBuffers(FrameBufferList);
    FrameBufferList.clear();

    Vector<VkImageView> imageViewList;
    for (auto& renderedTexture : textureSystem.RenderedTextureList[RenderPassId])
    {
        imageViewList.emplace_back(renderedTexture.textureView);
    }
    VkImageView depthTexture = textureSystem.DepthTextureList[RenderPassId].textureView;

    RenderPass = RenderPass_BuildRenderPass(cRenderer.Device, renderSystem.renderPassBuildInfoList[RenderPassId]);
    FrameBufferList = RenderPass_BuildFrameBuffer(cRenderer.Device, RenderPass, renderSystem.renderPassBuildInfoList[RenderPassId], imageViewList, &depthTexture, cRenderer.SwapChainImageViews, renderSystem.RenderPassResolutionList[RenderPassId]);
    BuildCommandBuffer();

    for (auto& pipeline : renderSystem.RenderPipelineList[RenderPassId])
    {
        //pipeline.RecreateSwapchain(RenderPass, sizeof(SceneDataBuffer), newWidth, newHeight);
    }

    renderSystem.ClearValueList[RenderPassId] = renderSystem.renderPassBuildInfoList[RenderPassId].ClearValueList;
    renderSystem.RenderPassInfoList[RenderPassId] = VkRenderPassBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = RenderPass,
        .framebuffer = FrameBufferList[cRenderer.ImageIndex],
        .renderArea = renderSystem.renderPassBuildInfoList[RenderPassId].RenderArea.RenderArea,
        .clearValueCount = static_cast<uint32>(renderSystem.ClearValueList[RenderPassId].size()),
        .pClearValues = renderSystem.ClearValueList[RenderPassId].data()
    };
}

void JsonRenderPass::BuildRenderPass(const RenderPassBuildInfoModel& renderPassBuildInfo)
{
    Vector<Texture> RenderedColorTextureList;
    for (auto& texture : renderPassBuildInfo.RenderedTextureInfoModelList)
    {
        VkImageCreateInfo imageCreateInfo = texture.ImageCreateInfo;
        VkSamplerCreateInfo samplerCreateInfo = texture.SamplerCreateInfo;
        switch (texture.TextureType)
        {
            case ColorRenderedTexture: textureSystem.RenderedTextureList[RenderPassId].emplace_back(textureSystem.CreateTexture(VK_IMAGE_ASPECT_COLOR_BIT, imageCreateInfo, samplerCreateInfo)); break;
            case InputAttachmentTexture: textureSystem.RenderedTextureList[RenderPassId].emplace_back(textureSystem.CreateTexture(VK_IMAGE_ASPECT_COLOR_BIT, imageCreateInfo, samplerCreateInfo)); break;
            case ResolveAttachmentTexture: textureSystem.RenderedTextureList[RenderPassId].emplace_back(textureSystem.CreateTexture(VK_IMAGE_ASPECT_COLOR_BIT, imageCreateInfo, samplerCreateInfo)); break;
            case DepthRenderedTexture: textureSystem.DepthTextureList[RenderPassId] = textureSystem.CreateTexture(VK_IMAGE_ASPECT_DEPTH_BIT, imageCreateInfo, samplerCreateInfo); break;
            default: throw std::runtime_error("Case doesn't exist: RenderedTextureType");
        };
    }

    RenderPass = RenderPass_BuildRenderPass(cRenderer.Device, renderPassBuildInfo);
}

void JsonRenderPass::BuildFrameBuffer(const RenderPassBuildInfoModel& renderPassBuildInfo)
{
    Vector<VkImageView> imageViewList;
    for (int x = 0; x < textureSystem.RenderedTextureList[RenderPassId].size(); x++)
    {
        imageViewList.emplace_back(textureSystem.RenderedTextureList[RenderPassId][x].textureView);
    }

    SharedPtr<VkImageView> depthTextureView = nullptr;
    if (textureSystem.DepthTextureList.find(RenderPassId) != textureSystem.DepthTextureList.end())
    {
        depthTextureView = std::make_shared<VkImageView>(textureSystem.DepthTextureList[RenderPassId].textureView);
    }

    VkRenderPass& renderPass = RenderPass;
    FrameBufferList.resize(cRenderer.SwapChainImageCount);
    FrameBufferList = RenderPass_BuildFrameBuffer(cRenderer.Device, renderPass, renderPassBuildInfo, imageViewList, depthTextureView.get(), cRenderer.SwapChainImageViews, renderSystem.RenderPassResolutionList[RenderPassId]);
}

void JsonRenderPass::RebuildFrameBuffer(const RenderPassBuildInfoModel& renderPassBuildInfo)
{
    VkRenderPass& renderPass = RenderPass;
    Vector<Texture> renderedTextureList = textureSystem.RenderedTextureList[RenderPassId];
    FrameBufferList.resize(cRenderer.SwapChainImageCount);
   // FrameBufferList = RenderPass_BuildFrameBuffer(cRenderer.Device, renderPass, renderPassBuildInfo, renderedTextureList, depthTextureView.get(), cRenderer.SwapChain.SwapChainImageViews, renderSystem.RenderPassResolutionList[RenderPassId]);
}

void JsonRenderPass::BuildCommandBuffer()
{
    RenderPass_CreateCommandBuffers(cRenderer.Device, cRenderer.CommandPool, &CommandBuffer, 1);
}

void JsonRenderPass::Destroy()
{
    for (auto renderedTexture : textureSystem.RenderedTextureList[RenderPassId])
    {
        textureSystem.DestroyTexture(renderedTexture);
    }

    renderer.DestroyRenderPass(RenderPass);
    renderer.DestroyCommandBuffers(CommandBuffer);
    renderer.DestroyFrameBuffers(FrameBufferList);
    FrameBufferList.clear();
}