#include "GameSystem.h"
#include <imgui/backends/imgui_impl_glfw.h>
#include "json.h"
#include "TextureSystem.h"
#include "ImGuiFunc.h"
#include "AssetManager.h"
#include "LevelSystem.h"
#include "MeshSystem.h"

GameSystem gameSystem = GameSystem();

GameSystem::GameSystem()
{
}

GameSystem::~GameSystem()
{
}

void GameSystem::StartUp()
{
    renderSystem.StartUp();
    levelSystem.LoadLevel("../Levels/TestLevel.json");
}

void GameSystem::Input(const float& deltaTime)
{
    for (auto& input : assetManager.InputComponentList)
    {
        Sprite& sprite = levelSystem.SpriteList[input.first];
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
    renderSystem.Update(deltaTime);
    
    VkCommandBuffer commandBuffer = renderer.BeginSingleTimeCommands();
    meshSystem.Update(commandBuffer, deltaTime);
    renderer.EndSingleTimeCommands(commandBuffer);

    

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
    levelSystem.Draw(CommandBufferSubmitList, deltaTime);
    CommandBufferSubmitList.emplace_back(ImGui_Draw(cRenderer, renderSystem.imGuiRenderer));
    VULKAN_RESULT(renderer.EndFrame(CommandBufferSubmitList));
    CommandBufferSubmitList.clear();
}

void GameSystem::Destroy()
{
    assetManager.Destroy();
    renderSystem.Destroy();
}