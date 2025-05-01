#include "JsonRenderPass.h"
#include <CVulkanRenderer.h>
#include <stdexcept>
#include <VulkanRenderPass.h>
#include "RenderSystem.h"
#include "JsonPipeline.h"

JsonRenderPass::JsonRenderPass()
{
}

JsonRenderPass::JsonRenderPass(uint renderPassIndex, const String& jsonPath, ivec2& renderPassResolution)
{
    RenderPassId = renderPassIndex;
    RenderPassResolution = renderPassResolution;
    SampleCount = VK_SAMPLE_COUNT_1_BIT;
    FrameBufferList.resize(cRenderer.SwapChain.SwapChainImageCount);

    nlohmann::json json = Json::ReadJson(jsonPath);
    RenderPassBuildInfoModel renderPassBuildInfo = RenderPassBuildInfoModel::from_json(json, renderPassResolution);
    BuildRenderPass(renderPassBuildInfo);
    BuildFrameBuffer(renderPassBuildInfo);
    CreateCommandBuffer();

    auto textureId = renderSystem.LoadTexture("../Textures/TestTexture.json");
    auto materialId = renderSystem.LoadMaterial("../Materials/Material1.json");
    auto vramId = renderSystem.AddSpriteVRAM("../Sprites/TestSprite.json");

    assetManager.AnimationFrameList[0] = Vector<ivec2>
    {
        ivec2(0, 0),
        ivec2(1, 0)
    };

    assetManager.AnimationFrameList[1] = Vector<ivec2>
    {
        ivec2(3, 0),
        ivec2(4, 0),
        ivec2(5, 0),
        ivec2(4, 0)
    };

    assetManager.AnimationList[0] = Animation2D
    {
        .FrameHoldTime = 0.2f,
        .AnimationFrameId = 0
    };
    assetManager.AnimationList[1] = Animation2D
    {
        .FrameHoldTime = 0.2f,
        .AnimationFrameId = 1
    };

    for (int x = 0; x < 20000; x++)
    {
        assetManager.CreateGameObject(RenderPassId, "Obj3", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, vramId, vec2((32 * x), (32 * x)));
        renderSystem.SpriteBatchLayerObjectList[RenderPassId].emplace_back(assetManager.GameObjectList[x + 1].GameObjectId);
    }

    renderSystem.SpriteBatchLayerList[RenderPassId].emplace_back(SpriteBatchLayer(RenderPassId));

    Vector<SpriteMesh> meshList;
    for (auto& spriteLayer : renderSystem.SpriteBatchLayerList[RenderPassId])
    {
        meshList.emplace_back(assetManager.MeshList[spriteLayer.SpriteLayerMeshId]);
    }

    uint id = renderSystem.RenderPipelineList.size();
    renderSystem.RenderPipelineList[RenderPassId].emplace_back(JsonPipeline(id, "../Pipelines/Default2DPipeline.json", RenderPass, sizeof(SceneDataBuffer), RenderPassResolution));

    renderSystem.ClearValueList[RenderPassId] = renderPassBuildInfo.ClearValueList;
    renderArea = renderPassBuildInfo.RenderArea.RenderArea;
    renderSystem.RenderPassInfoList[RenderPassId] = VkRenderPassBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = RenderPass,
        .framebuffer = FrameBufferList[cRenderer.ImageIndex],
        .renderArea = renderPassBuildInfo.RenderArea.RenderArea,
        .clearValueCount = static_cast<uint32>(renderSystem.ClearValueList[RenderPassId].size()),
        .pClearValues = renderSystem.ClearValueList[RenderPassId].data()
    };
}

JsonRenderPass::JsonRenderPass(uint renderPassIndex, const String& jsonPath, Texture& inputTexture, ivec2& renderPassResolution)
{
    RenderPassId = renderPassIndex;
    RenderPassResolution = renderPassResolution;
    SampleCount = VK_SAMPLE_COUNT_1_BIT;
    FrameBufferList.resize(cRenderer.SwapChain.SwapChainImageCount);

    nlohmann::json json = Json::ReadJson(jsonPath);
    RenderPassBuildInfoModel renderPassBuildInfo = RenderPassBuildInfoModel::from_json(json, renderPassResolution);
    BuildRenderPass(renderPassBuildInfo);
    BuildFrameBuffer(renderPassBuildInfo);
    CreateCommandBuffer();

    uint id = renderSystem.RenderPipelineList.size();
    renderSystem.InputTextureList[id].emplace_back(std::make_shared<Texture>(inputTexture));
    renderSystem.RenderPipelineList[RenderPassId].emplace_back(JsonPipeline(id, "../Pipelines/FrameBufferPipeline.json", RenderPass, sizeof(SceneDataBuffer), renderPassResolution));
    renderArea = renderPassBuildInfo.RenderArea.RenderArea;
    renderSystem.ClearValueList[RenderPassId] = renderPassBuildInfo.ClearValueList;
    renderSystem.RenderPassInfoList[RenderPassId] = VkRenderPassBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = RenderPass,
        .framebuffer = FrameBufferList[cRenderer.ImageIndex],
        .renderArea = renderPassBuildInfo.RenderArea.RenderArea,
        .clearValueCount = static_cast<uint32>(renderSystem.ClearValueList[RenderPassId].size()),
        .pClearValues = renderSystem.ClearValueList[RenderPassId].data()
    };
}

JsonRenderPass::~JsonRenderPass()
{
}

void JsonRenderPass::Update(const float& deltaTime)
{
}

void JsonRenderPass::BuildRenderPipelines(const RenderPassBuildInfoModel& renderPassBuildInfo, SceneDataBuffer& sceneDataBuffer)
{
    for (int x = 0; x < renderPassBuildInfo.RenderPipelineList.size(); x++)
    {
        renderSystem.RenderPipelineList[RenderPassId] = Vector<JsonPipeline>{ JsonPipeline(1, renderPassBuildInfo.RenderPipelineList[x], RenderPass, sizeof(SceneDataBuffer), RenderPassResolution) };
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
    for (int x = 0; x < renderSystem.RenderedTextureList[RenderPassId].size(); x++)
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

VkCommandBuffer JsonRenderPass::Draw(Vector<SharedPtr<GameObject>> meshList, SceneDataBuffer& sceneDataBuffer)
{
//    RenderPassInfo.clearValueCount = static_cast<uint32>(renderSystem.ClearValueList[RenderPassId].size());
//    RenderPassInfo.pClearValues = renderSystem.ClearValueList[RenderPassId].data();
//    RenderPassInfo.framebuffer = FrameBufferList[cRenderer.ImageIndex];
//
//    VkCommandBufferBeginInfo CommandBufferBeginInfo
//    {
//        .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
//        .flags = VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
//    };
//
//    VULKAN_RESULT(vkResetCommandBuffer(CommandBuffer, 0));
//    VULKAN_RESULT(vkBeginCommandBuffer(CommandBuffer, &CommandBufferBeginInfo));
//    vkCmdBeginRenderPass(CommandBuffer, &RenderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
//    for (auto mesh : meshList)
//    {
//        mesh->Draw(CommandBuffer, renderSystem.RenderPipelineList[RenderPassId][0].Pipeline, renderSystem.RenderPipelineList[RenderPassId][0].PipelineLayout, renderSystem.RenderPipelineList[RenderPassId][0].DescriptorSetList);
//    }
//    vkCmdEndRenderPass(CommandBuffer);
//    vkEndCommandBuffer(CommandBuffer);
    return CommandBuffer;
}

void JsonRenderPass::CreateCommandBuffer()
{
    Renderer_CreateCommandBuffers(cRenderer.Device, cRenderer.CommandPool, &CommandBuffer, 1);
}

void JsonRenderPass::Destroy()
{
    for (auto renderedTexture : renderSystem.RenderedTextureList[RenderPassId])
    {
        renderedTexture.Destroy();
    }

    renderer.DestroyRenderPass(RenderPass);
    renderer.DestroyCommandBuffers(CommandBuffer);
    renderer.DestroyFrameBuffers(FrameBufferList);
    FrameBufferList.clear();
}