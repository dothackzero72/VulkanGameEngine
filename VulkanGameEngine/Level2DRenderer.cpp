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


    TextureList.emplace_back(MemoryManager::AddTexture(Texture("../Textures/MegaMan_diffuse.bmp", VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum::kType_DiffuseTextureMap)));
   // TextureList.emplace_back(MemoryManager::AddTexture(Texture("../Textures/container2.png", VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum::kType_DiffuseTextureMap)));

    MaterialList.emplace_back(MemoryManager::AddMaterial(Material("Material1")));
    MaterialList.back()->SetAlbedoMap(MemoryManager::GetTextureList()[0]);

    //MaterialList.emplace_back(MemoryManager::AddMaterial(Material("Material2")));
    //MaterialList.back()->SetAlbedoMap(MemoryManager::GetTextureList()[1]);

    ivec2 size = ivec2(32);
    SpriteSheet spriteSheet = SpriteSheet(MaterialList[0], size, 0);

    GameObjectList.emplace_back(MemoryManager::AddGameObject(GameObject("Obj1", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, spriteSheet)));
   // GameObjectList.emplace_back(MemoryManager::AddGameObject(GameObject("Obj2", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, spriteSheet)));
   // GameObjectList.emplace_back(MemoryManager::AddGameObject(GameObject("Obj2", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, spriteSheet)));

    VULKAN_RESULT(renderer.CreateCommandBuffer(CommandBuffer));

    nlohmann::json json = Json::ReadJson("../RenderPass/Default2DRenderPass.json");
    RenderPassBuildInfoModel renderPassBuildInfo = RenderPassBuildInfoModel::from_json(json, renderPassResolution);
    BuildRenderPass(renderPassBuildInfo);
    BuildFrameBuffer();

    JsonPipelineList.emplace_back(MemoryManager::AddJsonPipeline(JsonPipeline("../Pipelines/Default2DPipeline.json", RenderPass, sizeof(SceneDataBuffer))));
    JsonPipelineList.emplace_back(MemoryManager::AddJsonPipeline(JsonPipeline("../Pipelines/SpriteInstancePipeline.json", RenderPass, sizeof(SceneDataBuffer))));

    std::dynamic_pointer_cast<Transform2DComponent>(MemoryManager::GetGameObjectList()[0]->GetComponentByComponentType(ComponentTypeEnum::kTransform2DComponent))->GameObjectPosition = vec2(300.0f, 40.0f);
   // std::dynamic_pointer_cast<Transform2DComponent>(MemoryManager::GetGameObjectList()[1]->GetComponentByComponentType(ComponentTypeEnum::kTransform2DComponent))->GameObjectPosition = vec2(300.0f, 20.0f);
   // std::dynamic_pointer_cast<Transform2DComponent>(MemoryManager::GetGameObjectList()[2]->GetComponentByComponentType(ComponentTypeEnum::kTransform2DComponent))->GameObjectPosition = vec2(300.0f, 80.0f);

    SpriteLayerList.emplace_back(MemoryManager::AddSpriteBatchLayer(SpriteBatchLayer(JsonPipelineList[1])));
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
    DestroyDeadGameObjects();

    for (auto gameObject : GameObjectList)
    {
        gameObject->Update(deltaTime);
    }
    for (auto& spriteLayer : SpriteLayerList)
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
    for (auto spriteLayer : SpriteLayerList)
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
    for (auto spriteLayer : SpriteLayerList)
    {
        spriteLayer->Destroy();
    }
    for (auto material : MaterialList)
    {
        material->Destroy();
    }
    for (auto texture : TextureList)
    {
        texture->Destroy();
    }
    JsonRenderPass::Destroy();
}

void Level2DRenderer::DestroyDeadGameObjects()
{
    Vector<SharedPtr<GameObject>> deadGameObjectList;
    for (auto it = GameObjectList.begin(); it != GameObjectList.end(); ++it) {
        if (!(*it)->GameObjectAlive) {
            deadGameObjectList.push_back(*it);
        }
    }

    if (deadGameObjectList.size())
    {
        for (auto& gameObject : deadGameObjectList) {
            if (SharedPtr spriteComponent = gameObject->GetComponentByComponentType(kSpriteComponent)) {
                SharedPtr sprite = std::dynamic_pointer_cast<SpriteComponent>(spriteComponent);
                if (sprite) {
                    SharedPtr spriteObject = sprite->GetSprite();
                    SpriteLayerList[0]->RemoveSprite(spriteObject);
                }
            }
            gameObject->Destroy();
        }

        GameObjectList.erase(std::remove_if(GameObjectList.begin(), GameObjectList.end(),
            [&](const SharedPtr<GameObject>& gameObject) {
                return !gameObject->GameObjectAlive;
            }),
            GameObjectList.end());
    }
}