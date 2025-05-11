#include "GameSystem.h"
#include <imgui/backends/imgui_impl_glfw.h>


GameSystem gameSystem = GameSystem();

void GameSystem::DestroyDeadGameObjects()
{
    if (assetManager.GameObjectList.empty())
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

GameSystem::GameSystem()
{
}

GameSystem::~GameSystem()
{
}

void GameSystem::LoadLevel()
{
    OrthographicCamera = std::make_shared<OrthographicCamera2D>(OrthographicCamera2D(vec2((float)cRenderer.SwapChain.SwapChainResolution.width, (float)cRenderer.SwapChain.SwapChainResolution.height), vec3(0.0f, 0.0f, 0.0f)));

    renderSystem.LoadTexture("../Textures/TestTexture.json");
    renderSystem.LoadTexture("../Textures/SparkManTexture.json");
    renderSystem.LoadMaterial("../Materials/Material1.json");
    renderSystem.LoadMaterial("../Materials/SparkManTileSetMaterial.json");
    auto vramId = renderSystem.AddSpriteVRAM("../Sprites/TestSprite.json");
    TileSetId = renderSystem.AddTileSetVRAM("../TileSets/SparkManTileSet.json");
    renderSystem.LoadLevelLayout("../LevelMapLevelLayout/TestMapLevelLayout.json");

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

    for (int x = 0; x < 5; x++)
    {
        assetManager.CreateGameObject("Obj3", Vector<ComponentTypeEnum> { kTransform2DComponent, kInputComponent, kSpriteComponent }, vramId, vec2((32 * x), (32 * x)));
    }

    Level = Level2D(VkGuid::GenerateGUID(), TileSetId, renderSystem.levelLayout.LevelBounds, renderSystem.levelLayout.LevelMapList);

   // levelRenderPass2DId = renderSystem.AddRenderPass("../RenderPass/LevelShader2DRenderPass.json", ivec2(cRenderer.SwapChain.SwapChainResolution.width, cRenderer.SwapChain.SwapChainResolution.height));
    spriteRenderPass2DId = renderSystem.AddRenderPass("../RenderPass/Default2DRenderPass.json", ivec2(cRenderer.SwapChain.SwapChainResolution.width, cRenderer.SwapChain.SwapChainResolution.height));
    frameBufferId = renderSystem.AddRenderPass("../RenderPass/FrameBufferRenderPass.json", renderSystem.RenderedTextureList[spriteRenderPass2DId][0], ivec2(cRenderer.SwapChain.SwapChainResolution.width, cRenderer.SwapChain.SwapChainResolution.height));
}

void GameSystem::StartUp()
{
    renderSystem.StartUp();
    LoadLevel();
}

void GameSystem::Input(const float& deltaTime)
{
    for (auto& input : assetManager.InputComponentList)
    {
        Sprite& sprite = assetManager.SpriteList[input.first];
        Transform2DComponent& transform = assetManager.TransformComponentList[input.first];
        if (vulkanWindow->keyboard.KeyPressed[KEY_A] == KS_PRESSED ||
            vulkanWindow->keyboard.KeyPressed[KEY_A] == KS_HELD)
        {
            transform.GameObjectPosition.x -= 200.0f * deltaTime;
            sprite.SetSpriteAnimation(Sprite::SpriteAnimationEnum::kWalking);
            sprite.FlipSprite.x = 0;
        }
        else if (vulkanWindow->keyboard.KeyPressed[KEY_D] == KS_PRESSED ||
                 vulkanWindow->keyboard.KeyPressed[KEY_D] == KS_HELD)
        {
            transform.GameObjectPosition.x += 200.0f * deltaTime;
            sprite.SetSpriteAnimation(Sprite::SpriteAnimationEnum::kWalking);
            sprite.FlipSprite.x = 1;
        }
        else
        {
            sprite.SetSpriteAnimation(Sprite::SpriteAnimationEnum::kStanding);
        }
    }
}

void GameSystem::Update(const float& deltaTime)
{
    //DestroyDeadGameObjects();
    gameSystem.OrthographicCamera->Update(SceneProperties);
    renderSystem.Update(deltaTime);
    
    VkCommandBuffer commandBuffer = renderer.BeginSingleTimeCommands();
    for (auto& levelLayer : renderSystem.LevelLayerMeshList[LevelId])
    {
        levelLayer.Update(commandBuffer, deltaTime);
    }
    renderer.EndSingleTimeCommands(commandBuffer);

   // Level.Update(deltaTime);

}

void GameSystem::DebugUpdate(const float& deltaTime)
{
    //ImGui_ImplVulkan_NewFrame();
    //ImGui_ImplGlfw_NewFrame();

    //ImGui::NewFrame();
    //ImGui::Begin("Button Window");
    //ImGui::Text("Application average %.3f ms/frame (%.1f FPS)", 1000.0f / ImGui::GetIO().Framerate, ImGui::GetIO().Framerate);
    ////texture2.get()->ImGuiShowTexture(ImVec2(256, 128));
    //ImGui::End();
    //ImGui::Render();
}

void GameSystem::Draw(const float& deltaTime)
{
    VULKAN_RESULT(renderer.StartFrame());
 //   CommandBufferSubmitList.emplace_back(renderSystem.RenderLevel(LevelId, levelRenderPass2DId, deltaTime, SceneProperties));
    CommandBufferSubmitList.emplace_back(renderSystem.RenderSprites(spriteRenderPass2DId, deltaTime, SceneProperties));
    CommandBufferSubmitList.emplace_back(renderSystem.RenderFrameBuffer(frameBufferId));
   // CommandBufferSubmitList.emplace_back(InterfaceRenderPass::Draw());
    VULKAN_RESULT(renderer.EndFrame(CommandBufferSubmitList));
    CommandBufferSubmitList.clear();
}

void GameSystem::Destroy()
{
    assetManager.Destroy();
    renderSystem.Destroy();
}
