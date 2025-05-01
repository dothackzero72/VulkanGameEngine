#include "Level2DRenderer.h"
#include "InputComponent.h"
#include "AssetManager.h"
#include "RenderSystem.h"
#include <VulkanRenderPass.h>

Level2DRenderer::Level2DRenderer() : JsonRenderPass()
{
}

Level2DRenderer::Level2DRenderer(const String& jsonPath, ivec2 renderPassResolution) : JsonRenderPass(2, jsonPath, renderPassResolution)
{
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

    assetManager.CreateGameObject(RenderPassId, "Obj1", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, vramId, vec2(300.0f, 40.0f));
    assetManager.CreateGameObject(RenderPassId, "Obj2", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, vramId, vec2(300.0f, 20.0f));
    for (int x = 0; x < 20000; x++)
    {
        assetManager.CreateGameObject(RenderPassId, "Obj3", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, vramId, vec2(300.0f, 80.0f));
    }
    renderSystem.SpriteBatchLayerList[RenderPassId].emplace_back(SpriteBatchLayer(RenderPassId));
    renderSystem.RenderPipelineList[RenderPassId].emplace_back(JsonPipeline(RenderPassId, "../Pipelines/Default2DPipeline.json", RenderPass, sizeof(SceneDataBuffer), RenderPassResolution));
}

Level2DRenderer::~Level2DRenderer()
{
}

void Level2DRenderer::StartLevelRenderer()
{

}

void Level2DRenderer::Update(const float& deltaTime)
{
   // DestroyDeadGameObjects();
    VkCommandBuffer commandBuffer = renderer.BeginSingleTimeCommands();
    for (auto& spriteLayer : renderSystem.SpriteBatchLayerList[RenderPassId])
    {
        spriteLayer.Update(commandBuffer, deltaTime);
    }
    renderer.EndSingleTimeCommands(commandBuffer);
}

void Level2DRenderer::UpdateBufferIndex()
{
    renderSystem.UpdateBufferIndex();
}

VkCommandBuffer Level2DRenderer::DrawSprites(Vector<SpriteBatchLayer> spriteLayerList, SceneDataBuffer& sceneDataBuffer)
{
    return renderSystem.RenderSprites(RenderPassId, 0.0f, sceneDataBuffer);
}

void Level2DRenderer::Destroy()
{

    for (auto& spriteLayer : renderSystem.SpriteBatchLayerList[RenderPassId])
    {
        spriteLayer.Destroy();
    }
    //for (auto& texture : TextureList)
    //{
    //    texture.Destroy();
    //}
  /*  for (auto& material : MaterialList)
    {
        material->Destroy();
    }*/
  //  JsonRenderPass::Destroy();
}

//Vector<SpriteMesh> Level2DRenderer::GetMeshFromGameObjects()
//{
//    Vector<SpriteMesh> meshList;
//    for (auto& spriteLayer : renderSystem.SpriteBatchLayerList[RenderPassId])
//    {
//        meshList.emplace_back(assetManager.MeshList[spriteLayer.SpriteLayerMeshId]);
//    }
//
//    return meshList;
//}
