#include "Level2DRenderer.h"
#include "MemoryManager.h"
#include "InputComponent.h"
#include "AssetManager.h"

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

Level2DRenderer::~Level2DRenderer()
{
}

void Level2DRenderer::StartLevelRenderer()
{
    if (!LevelRenderer)
    {
        LevelRenderer = SharedPtr<Level2DRenderer>(this);
    }

    auto textureId = assetManager.LoadTexture("../Textures/TestTexture.json");
    auto materialId = assetManager.LoadMaterial("../Materials/Material1.json");
    auto vRAMID = assetManager.AddSpriteVRAM("../Sprites/TestSprite.json");

    const Material material = assetManager.MaterialList[materialId];
    TextureList.emplace_back(assetManager.TextureList[material.AlbedoMapId]);

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

    const Texture texture = assetManager.TextureList[material.AlbedoMapId];
    assetManager.VRAMSpriteList[0] = SpriteVRAM
    {
        .VRAMSpriteID = 0,
        .SpriteMaterialID = materialId,
        .SpriteLayer = 0,
        .SpriteColor = vec4(0.0f, 0.0f, 0.0f, 1.0f),
        .SpritePixelSize = ivec2(32),
        .SpriteScale = vec2(5.0f)
    };
    assetManager.VRAMSpriteList[vRAMID].SpriteSize = vec2(assetManager.VRAMSpriteList[vRAMID].SpritePixelSize.x * assetManager.VRAMSpriteList[vRAMID].SpriteScale.x, assetManager.VRAMSpriteList[vRAMID].SpritePixelSize.y * assetManager.VRAMSpriteList[vRAMID].SpriteScale.y),
    assetManager.VRAMSpriteList[vRAMID].SpriteCells = ivec2(texture.Width / assetManager.VRAMSpriteList[vRAMID].SpritePixelSize.x, texture.Height / assetManager.VRAMSpriteList[vRAMID].SpritePixelSize.y),
    assetManager.VRAMSpriteList[vRAMID].SpriteUVSize = vec2(1.0f / (float)assetManager.VRAMSpriteList[vRAMID].SpriteCells.x, 1.0f / (float)assetManager.VRAMSpriteList[vRAMID].SpriteCells.y);

    AddGameObject("Obj1", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, 0, vec2(300.0f, 40.0f));
    AddGameObject("Obj2", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, 0, vec2(300.0f, 20.0f));
    for (int x = 0; x < 20000; x++)
    {
        AddGameObject("Obj3", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, 0, vec2(300.0f, 80.0f));
    }
    JsonPipelineList.resize(1);
    SpriteLayerList.emplace_back(SpriteBatchLayer(GameObjectList, JsonPipelineList[0]));
    GPUImport gpuImport =
    {
        .MeshList = Vector<SpriteMesh>(GetMeshFromGameObjects()),
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

    JsonPipelineList[0] = JsonPipeline("../Pipelines/Default2DPipeline.json", RenderPass, gpuImport, vertexBinding, vertexAttribute, sizeof(SceneDataBuffer), RenderPassResolution);
    SpriteLayerList[0].SpriteRenderPipeline = JsonPipelineList[0];
}

void Level2DRenderer::AddGameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentTypeList, uint32 spriteID, vec2 objectPosition)
{
    GameObject gameObject = GameObject(name, Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, spriteID);
    GameObjectList.emplace_back(gameObject);

    Vector<GameObjectComponent> gameObjectComponentList;
    for (auto component : gameObjectComponentTypeList)
    {
        switch (component)
        {
            case kTransform2DComponent: assetManager.TransformComponentList[gameObject.GameObjectId] = Transform2DComponent(gameObject.GetId(), objectPosition, name); break;
           // case kInputComponent: gameObject->AddComponent(std::make_shared<InputComponent>(InputComponent(gameObject->GetId(), name))); break;
            case kSpriteComponent: assetManager.SpriteList[gameObject.GameObjectId] = Sprite(gameObject.GetId(), spriteID); break;
        }
    }

}

void Level2DRenderer::RemoveGameObject(SharedPtr<GameObject> gameObject)
{
    gameObject->GameObjectAlive = false;
}

void Level2DRenderer::Input(const float& deltaTime)
{

}

void Level2DRenderer::Update(const float& deltaTime)
{
    DestroyDeadGameObjects();
    VkCommandBuffer commandBuffer = renderer.BeginSingleTimeCommands();
    for (auto& spriteLayer : SpriteLayerList)
    {
        spriteLayer.Update(commandBuffer, deltaTime);
    }
    renderer.EndSingleTimeCommands(commandBuffer);
}

void Level2DRenderer::UpdateBufferIndex()
{
    for (int x = 0; x < TextureList.size(); x++)
    {
        TextureList[x].UpdateTextureBufferIndex(x);
    }
    int xz = 0;
    for (auto& [id, material] : assetManager.MaterialList) {
        material.UpdateMaterialBufferIndex(xz);
        ++xz;
    }
}

VkCommandBuffer Level2DRenderer::DrawSprites(Vector<SpriteBatchLayer> spriteLayerList, SceneDataBuffer& sceneDataBuffer)
{
    RenderPassInfo.clearValueCount = static_cast<uint32>(ClearValueList.size());
    RenderPassInfo.pClearValues = ClearValueList.data();
    RenderPassInfo.framebuffer = FrameBufferList[cRenderer.ImageIndex];

    VkCommandBufferBeginInfo CommandBufferBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
        .flags = VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
    };

    VULKAN_RESULT(vkResetCommandBuffer(CommandBuffer, 0));
    VULKAN_RESULT(vkBeginCommandBuffer(CommandBuffer, &CommandBufferBeginInfo));
    vkCmdBeginRenderPass(CommandBuffer, &RenderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
    for (auto spriteLayer : spriteLayerList)
    {
        spriteLayer.Draw(CommandBuffer, sceneDataBuffer);
    }
    vkCmdEndRenderPass(CommandBuffer);
    vkEndCommandBuffer(CommandBuffer);
    return CommandBuffer;
}

void Level2DRenderer::Destroy()
{

    for (auto& spriteLayer : SpriteLayerList)
    {
        spriteLayer.Destroy();
    }
    for (auto& texture : TextureList)
    {
        texture.Destroy();
    }
  /*  for (auto& material : MaterialList)
    {
        material->Destroy();
    }*/
    JsonRenderPass::Destroy();
}

void Level2DRenderer::DestroyDeadGameObjects()
{
    if (GameObjectList.empty()) 
    {
        return;
    }

    //Vector<SharedPtr<GameObject>> deadGameObjectList;
    //for (auto it = GameObjectList.begin(); it != GameObjectList.end(); ++it) {
    //    if (!(*it)->GameObjectAlive) {
    //        deadGameObjectList.push_back(*it);
    //    }
    //}

    //if (!deadGameObjectList.empty())
    //{
    //    for (auto& gameObject : deadGameObjectList) {
    //        if (SharedPtr spriteComponent = gameObject->GetComponentByComponentType(kSpriteComponent)) {
    //            SharedPtr sprite = std::dynamic_pointer_cast<SpriteComponent>(spriteComponent);
    //            if (sprite) {
    //                SharedPtr spriteObject = sprite->GetSprite();
    //                SpriteLayerList[0]->RemoveSprite(spriteObject);
    //            }
    //        }
    //        gameObject->Destroy();
    //    }

    //    GameObjectList.erase(std::remove_if(GameObjectList.begin(), GameObjectList.end(),
    //        [&](const SharedPtr<GameObject>& gameObject) {
    //            return !gameObject->GameObjectAlive;
    //        }),
    //        GameObjectList.end());
    //}
}

Vector<SpriteMesh> Level2DRenderer::GetMeshFromGameObjects()
{
    Vector<SpriteMesh> meshList;
    for (auto& spriteLayer : SpriteLayerList) 
    {
        meshList.emplace_back(assetManager.MeshList[spriteLayer.SpriteLayerMeshId]);
    }

    return meshList;
}
