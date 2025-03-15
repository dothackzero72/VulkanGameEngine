#include "Level2DRenderer.h"
#include "MemoryManager.h"
#include "InputComponent.h"

SharedPtr<Level2DRenderer> Level2DRenderer::LevelRenderer = nullptr;

Level2DRenderer::Level2DRenderer() : JsonRenderPass()
{
}

Level2DRenderer::Level2DRenderer(const String& jsonPath, ivec2 renderPassResolution)
{
    RenderPassResolution = renderPassResolution;
    SampleCount = VK_SAMPLE_COUNT_1_BIT;
    FrameBufferList.resize(cRenderer.SwapChain.SwapChainImageCount);

    VULKAN_RESULT(renderer.CreateCommandBuffer(CommandBuffer));

    nlohmann::json json = Json::ReadJson(jsonPath);
    RenderPassBuildInfoModel renderPassBuildInfo = RenderPassBuildInfoModel::from_json(json, renderPassResolution);
    BuildRenderPass(renderPassBuildInfo);
    BuildFrameBuffer(renderPassBuildInfo);
}

Level2DRenderer::~Level2DRenderer()
{
}

void Level2DRenderer::StartLevelRenderer()
{
    if (!LevelRenderer)
    {
        LevelRenderer = SharedPtr<Level2DRenderer>(this);
    }

    TextureList.emplace_back(std::make_shared<Texture>(Texture("../Textures/MegaMan_diffuse.bmp", VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum::kType_DiffuseTextureMap)));
    MaterialList.emplace_back(std::make_shared<Material>(Material("Material1")));
    MaterialList.back()->SetAlbedoMap(TextureList[0]);

    ivec2 size = ivec2(32);
    SpriteSheet spriteSheet = SpriteSheet(MaterialList[0], size, 0);

    AddGameObject("Obj1", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, spriteSheet, vec2(300.0f, 40.0f));
    AddGameObject("Obj2", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, spriteSheet, vec2(300.0f, 20.0f));
    AddGameObject("Obj3", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, spriteSheet, vec2(300.0f, 80.0f));

    JsonPipelineList.resize(1);
    SpriteLayerList.emplace_back(std::make_shared<SpriteBatchLayer>(SpriteBatchLayer(GameObjectList, JsonPipelineList[0])));
    GPUImport gpuImport =
    {
     .MeshList = Vector<SharedPtr<Mesh<Vertex2D>>>(GetMeshFromGameObjects()),
        .TextureList = Vector<SharedPtr<Texture>>(TextureList),
        .MaterialList = Vector<SharedPtr<Material>>(MaterialList)
    };

    Vector<VkVertexInputBindingDescription> vertexBinding = NullVertex::GetBindingDescriptions();
    for (auto& instanceVar : SpriteInstanceVertex2D::GetBindingDescriptions())
    {
        vertexBinding.emplace_back(instanceVar);
    }

    Vector<VkVertexInputAttributeDescription> vertexAttribute = NullVertex::GetAttributeDescriptions();
    for (auto& instanceVar : SpriteInstanceVertex2D::GetAttributeDescriptions())
    {
        vertexAttribute.emplace_back(instanceVar);
    }


    JsonPipelineList[0] = std::make_shared<JsonPipeline>(JsonPipeline("../Pipelines/Default2DPipeline.json", RenderPass, gpuImport, vertexBinding, vertexAttribute, sizeof(SceneDataBuffer)));
    SpriteLayerList[0]->SpriteRenderPipeline = JsonPipelineList[0];
}

void Level2DRenderer::AddGameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentTypeList, SpriteSheet& spriteSheet, vec2 objectPosition)
{
    SharedPtr<GameObject> gameObject = std::make_shared<GameObject>(GameObject(name, Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, spriteSheet));
    GameObjectList.emplace_back(gameObject);

    Vector<GameObjectComponent> gameObjectComponentList;
    for (auto component : gameObjectComponentTypeList)
    {
        switch (component)
        {
            case kTransform2DComponent: gameObject->AddComponent(std::make_shared<Transform2DComponent>(Transform2DComponent(gameObject->GetId(), objectPosition, name))); break;
            case kInputComponent: gameObject->AddComponent(std::make_shared<InputComponent>(InputComponent(gameObject->GetId(), name))); break;
            case kSpriteComponent: gameObject->AddComponent(std::make_shared<SpriteComponent>(SpriteComponent(gameObject->GetId(), name, spriteSheet))); break;
        }
    }
}

void Level2DRenderer::RemoveGameObject(SharedPtr<GameObject> gameObject)
{
    gameObject->GameObjectAlive = false;
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
    VkCommandBuffer commandBuffer = renderer.BeginSingleTimeCommands();
    for (auto gameObj : GameObjectList)
    {
        gameObj->Update(commandBuffer, deltaTime);
    }
    for (auto& spriteLayer : SpriteLayerList)
    {
        spriteLayer->Update(commandBuffer, deltaTime);
    }
    renderer.EndSingleTimeCommands(commandBuffer);
}

void Level2DRenderer::UpdateBufferIndex()
{
    for (int x = 0; x < TextureList.size(); x++)
    {
        TextureList[x]->UpdateTextureBufferIndex(x);
    }
    for (int x = 0; x < MaterialList.size(); x++)
    {
        MaterialList[x]->UpdateMaterialBufferIndex(x);
    }
}

VkCommandBuffer Level2DRenderer::Draw(Vector<SharedPtr<GameObject>> meshList, SceneDataBuffer& sceneDataBuffer)
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
        spriteLayer->Draw(CommandBuffer, sceneDataBuffer);
    }
    vkCmdEndRenderPass(CommandBuffer);
    vkEndCommandBuffer(CommandBuffer);
    return CommandBuffer;
}

void Level2DRenderer::Destroy()
{
    for (auto& gameObject : GameObjectList)
    {
        gameObject->Destroy();
    }
    for (auto& spriteLayer : SpriteLayerList)
    {
        spriteLayer->Destroy();
    }
    for (auto& texture : TextureList)
    {
        texture->Destroy();
    }
    for (auto& material : MaterialList)
    {
        material->Destroy();
    }
    JsonRenderPass::Destroy();
}

void Level2DRenderer::DestroyDeadGameObjects()
{
    if (GameObjectList.empty()) 
    {
        return;
    }

    Vector<SharedPtr<GameObject>> deadGameObjectList;
    for (auto it = GameObjectList.begin(); it != GameObjectList.end(); ++it) {
        if (!(*it)->GameObjectAlive) {
            deadGameObjectList.push_back(*it);
        }
    }

    if (!deadGameObjectList.empty())
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

Vector<SharedPtr<Mesh<Vertex2D>>> Level2DRenderer::GetMeshFromGameObjects()
{
    Vector<SharedPtr<Mesh<Vertex2D>>> meshList;
    for (auto& spriteLayer : SpriteLayerList) 
    {
        meshList.emplace_back(spriteLayer->GetSpriteLayerMesh());
    }

    return meshList;
}

SharedPtr<GameObject> Level2DRenderer::SearchGameObjectsById(uint32 id)
{
    if (GameObjectList.empty()) {
        return nullptr;
    }
    auto it = std::find_if(GameObjectList.begin(), GameObjectList.end(),
        [id](const std::shared_ptr<GameObject>& obj)
        {
            return obj && obj->GetId() == id;
        });
    return (it != GameObjectList.end()) ? *it : nullptr;
}