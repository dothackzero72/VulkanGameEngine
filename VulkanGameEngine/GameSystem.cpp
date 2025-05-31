#include "GameSystem.h"
#include <imgui/backends/imgui_impl_glfw.h>
#include "json.h"
#include "TextureSystem.h"
#include "ImGuiFunc.h"
#include "AssetManager.h"

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

void GameSystem::LoadLevel(const String& levelPath)
{
    OrthographicCamera = std::make_shared<OrthographicCamera2D>(OrthographicCamera2D(vec2((float)cRenderer.SwapChainResolution.width, (float)cRenderer.SwapChainResolution.height), vec3(0.0f, 0.0f, 0.0f)));

    VkGuid vramId;
    VkGuid tileSetId;

    nlohmann::json json = Json::ReadJson(levelPath);
    VkGuid LevelId = VkGuid(json["LevelID"].get<String>().c_str());
    for (int x = 0; x < json["LoadTextures"].size(); x++)
    {
        textureSystem.LoadTexture(json["LoadTextures"][x]);
    }
    for (int x = 0; x < json["LoadMaterials"].size(); x++)
    {
        assetManager.LoadMaterial(json["LoadMaterials"][x]);
    }
    for (int x = 0; x < json["LoadSpriteVRAM"].size(); x++)
    {
        vramId = assetManager.AddSpriteVRAM(json["LoadSpriteVRAM"][x]);
    }
    for (int x = 0; x < json["LoadTileSetVRAM"].size(); x++)
    {
        tileSetId = assetManager.AddTileSetVRAM(json["LoadTileSetVRAM"][x]);
    }
    for (int x = 0; x < json["GameObjectList"].size(); x++)
    {
        String objectJson = json["GameObjectList"][x]["GameObjectPath"];
        vec2   positionOverride = vec2(json["GameObjectList"][x]["GameObjectPositionOverride"][0], json["GameObjectList"][x]["GameObjectPositionOverride"][1]);
        assetManager.CreateGameObject(objectJson, positionOverride);
    }
    assetManager.LoadLevelLayout(json["LoadLevelLayout"]);

    Level = Level2D(LevelId, tileSetId, assetManager.levelLayout.LevelBounds, assetManager.levelLayout.LevelMapList);

    VkGuid dummyGuid = VkGuid();
    spriteRenderPass2DId = renderSystem.AddRenderPass(Level.LevelId, "../RenderPass/LevelShader2DRenderPass.json", ivec2(cRenderer.SwapChainResolution.width, cRenderer.SwapChainResolution.height));
    frameBufferId = renderSystem.AddRenderPass(dummyGuid, "../RenderPass/FrameBufferRenderPass.json", textureSystem.RenderedTextureList[spriteRenderPass2DId][0], ivec2(cRenderer.SwapChainResolution.width, cRenderer.SwapChainResolution.height));
}

void GameSystem::StartUp()
{
    renderSystem.StartUp();
    LoadLevel("../Levels/TestLevel.json");
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
    for (auto& levelLayer : assetManager.LevelLayerMeshList[Level.LevelId])
    {
        levelLayer.Update(commandBuffer, deltaTime);
    }
    renderer.EndSingleTimeCommands(commandBuffer);

    Level.Update(deltaTime);

}

void GameSystem::DebugUpdate(const float& deltaTime)
{
    ImGui_StartFrame();
    //ImGui::Begin("Button Window");
    //ImGui::Text("Application average %.3f ms/frame (%.1f FPS)", 1000.0f / ImGui::GetIO().Framerate, ImGui::GetIO().Framerate);
    //texture2.get()->ImGuiShowTexture(ImVec2(256, 128));
    ImGui_EndFrame();
}

void GameSystem::Draw(const float& deltaTime)
{
    VULKAN_RESULT(renderer.StartFrame());
    CommandBufferSubmitList.emplace_back(renderSystem.RenderLevel(spriteRenderPass2DId, Level.LevelId, deltaTime, SceneProperties));
    CommandBufferSubmitList.emplace_back(renderSystem.RenderFrameBuffer(frameBufferId));
    CommandBufferSubmitList.emplace_back(ImGui_Draw(cRenderer, renderSystem.imGuiRenderer));
    VULKAN_RESULT(renderer.EndFrame(CommandBufferSubmitList));
    CommandBufferSubmitList.clear();
}

void GameSystem::Destroy()
{
    assetManager.Destroy();
    renderSystem.Destroy();
}