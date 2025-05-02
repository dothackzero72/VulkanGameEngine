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
        assetManager.CreateGameObject("Obj3", Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, vramId, vec2((32 * x), (32 * x)));
    }

    renderPass2DId = renderSystem.AddRenderPass("../RenderPass/Default2DRenderPass.json", ivec2(cRenderer.SwapChain.SwapChainResolution.width, cRenderer.SwapChain.SwapChainResolution.height));
    frameBufferId = renderSystem.AddRenderPass("../RenderPass/FrameBufferRenderPass.json", renderSystem.RenderedTextureList[renderPass2DId][0], ivec2(cRenderer.SwapChain.SwapChainResolution.width, cRenderer.SwapChain.SwapChainResolution.height));
}

void GameSystem::StartUp()
{
    renderSystem.StartUp();
    LoadLevel();
}

void GameSystem::Input(const float& deltaTime)
{
    if (vulkanWindow->keyboard.KeyPressed[KEY_G] == KS_PRESSED)
    {
        auto sf = 234;
    }
}

void GameSystem::Update(const float& deltaTime)
{
    //DestroyDeadGameObjects();
    gameSystem.OrthographicCamera->Update(SceneProperties);
    renderSystem.Update(deltaTime);
}

void GameSystem::DebugUpdate(const float& deltaTime)
{
    ImGui_ImplVulkan_NewFrame();
    ImGui_ImplGlfw_NewFrame();

    ImGui::NewFrame();
    ImGui::Begin("Button Window");
    ImGui::Text("Application average %.3f ms/frame (%.1f FPS)", 1000.0f / ImGui::GetIO().Framerate, ImGui::GetIO().Framerate);
    //texture2.get()->ImGuiShowTexture(ImVec2(256, 128));
    ImGui::End();
    ImGui::Render();
}

void GameSystem::Draw(const float& deltaTime)
{
    VULKAN_RESULT(renderer.StartFrame());
    CommandBufferSubmitList.emplace_back(renderSystem.RenderSprites(renderPass2DId, deltaTime, SceneProperties));
    CommandBufferSubmitList.emplace_back(renderSystem.RenderFrameBuffer(frameBufferId));
    CommandBufferSubmitList.emplace_back(InterfaceRenderPass::Draw());
    VULKAN_RESULT(renderer.EndFrame(CommandBufferSubmitList));
    CommandBufferSubmitList.clear();
}

void GameSystem::Destroy()
{
    assetManager.Destroy();
    renderSystem.Destroy();
}
