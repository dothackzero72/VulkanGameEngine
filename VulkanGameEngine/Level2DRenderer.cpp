#include "Level2DRenderer.h"
#include "MemoryManager.h"
Level2DRenderer::Level2DRenderer() : JsonRenderPass()
{
}

Level2DRenderer::Level2DRenderer(String jsonPath, ivec2 renderPassResolution) : JsonRenderPass()
{
    RenderPassResolution = renderPassResolution;
    SampleCount = VK_SAMPLE_COUNT_1_BIT;
    FrameBufferList.resize(cRenderer.SwapChain.SwapChainImageCount);


    TextureList.emplace_back(Texture("../Textures/MegaMan_diffuse.png", VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum::kType_DiffuseTextureMap));
    TextureList.emplace_back(Texture("../Textures/container2.png", VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum::kType_DiffuseTextureMap));

    MaterialList.emplace_back(Material("Material1"));
    MaterialList.back()->SetAlbedoMap(MemoryManager::GetTextureList()[0]);

    MaterialList.emplace_back(Material("Material2"));
    MaterialList.back()->SetAlbedoMap(MemoryManager::GetTextureList()[1]);

    GameObjectList.emplace_back(GameObject("Obj1", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, MaterialList[0], 0));
    GameObjectList.emplace_back(GameObject("Obj2", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, MaterialList[1], 0));
    //GameObjectList.emplace_back(GameObject::CreateGameObject("Obj3", List<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, SpriteList[2]));

    VULKAN_RESULT(renderer.CreateCommandBuffer(CommandBuffer));

    nlohmann::json json = Json::ReadJson("../RenderPass/Default2DRenderPass.json");
    RenderPassBuildInfoModel renderPassBuildInfo = RenderPassBuildInfoModel::from_json(json, renderPassResolution);
    BuildRenderPass(renderPassBuildInfo);
    BuildFrameBuffer();

    JsonPipelineList.emplace_back(JsonPipeline::CreateJsonRenderPass("../Pipelines/Default2DPipeline.json", RenderPass, sizeof(SceneDataBuffer)));
    JsonPipelineList.emplace_back(JsonPipeline::CreateJsonRenderPass("../Pipelines/SpriteInstancePipeline.json", RenderPass, sizeof(SceneDataBuffer)));

    SpriteLayerRenderList.emplace_back(SpriteBatchLayer(JsonPipelineList[1], SpriteList));
}

Level2DRenderer::~Level2DRenderer()
{
}

void Level2DRenderer::Input(const float& deltaTime)
{
    for (auto gameObject : GameObjectList)
    {
        gameObject->Input(deltaTime);
    }
}

void Level2DRenderer::Update(const float& deltaTime)
{
    for (auto gameObject : GameObjectList)
    {
        gameObject->Update(deltaTime);
    }
    for (auto& spriteLayer : SpriteLayerRenderList)
    {
        spriteLayer->Update(deltaTime);
    }
}

VkCommandBuffer Level2DRenderer::Draw(Vector<SharedPtr<GameObject>> meshList, SceneDataBuffer& sceneProperties)
{
    std::vector<VkClearValue> clearValues
    {
        VkClearValue{.color = { {0.0f, 0.0f, 0.0f, 1.0f} } },
        VkClearValue{.depthStencil = { 1.0f, 0 } }
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

    VkViewport viewport = VkViewport
    {
        .x = 0.0f,
        .y = 0.0f,
        .width = static_cast<float>(RenderPassResolution.x),
        .height = static_cast<float>(RenderPassResolution.y),
        .minDepth = 0.0f,
        .maxDepth = 1.0f
    };

    VkRect2D scissor = VkRect2D
    {
        .offset = VkOffset2D(0, 0),
        .extent = VkExtent2D
        {
          .width = static_cast<uint32>(RenderPassResolution.x),
          .height = static_cast<uint32>(RenderPassResolution.y)
        }
    };

    VkCommandBufferBeginInfo CommandBufferBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
        .flags = VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
    };

    VULKAN_RESULT(vkResetCommandBuffer(CommandBuffer, 0));
    VULKAN_RESULT(vkBeginCommandBuffer(CommandBuffer, &CommandBufferBeginInfo));
    vkCmdBeginRenderPass(CommandBuffer, &renderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
    vkCmdSetViewport(CommandBuffer, 0, 1, &viewport);
    vkCmdSetScissor(CommandBuffer, 0, 1, &scissor);
    for (auto spriteLayer : SpriteLayerRenderList)
    {
        spriteLayer->Draw(CommandBuffer, sceneProperties);
    }
    vkCmdEndRenderPass(CommandBuffer);
    vkEndCommandBuffer(CommandBuffer);
    return CommandBuffer;
}

void Level2DRenderer::Destroy()
{
    for (auto gameObject : GameObjectList)
    {
        gameObject->Destroy();
    }
    for (auto spriteLayer : SpriteLayerRenderList)
    {
        spriteLayer->Destroy();
    }
    for (auto texture : TextureList)
    {
        texture->Destroy();
    }
    JsonRenderPass::Destroy();
}
