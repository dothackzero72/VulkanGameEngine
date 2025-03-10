#include "Scene.h"
#include "VulkanRenderer.h"
#include <TextureCPP.h>
#include "SceneDataBuffer.h"
#include "implot.h"
#include "BakedTexture.h"
#include <stb/stb_image_write.h>
#include <stb/stb_image.h>
#include "MemoryManager.h"

void Scene::StartUp()
{
	orthographicCamera = std::make_shared<OrthographicCamera2D>(OrthographicCamera2D(vec2((float)cRenderer.SwapChain.SwapChainResolution.width, (float)cRenderer.SwapChain.SwapChainResolution.height), vec3(0.0f, 0.0f, 0.0f)));
	//MemoryManager::ViewMemoryMap();
	BuildRenderPasses();
}

void Scene::Input(float deltaTime)
{
	for (auto gameObject : GameObjectList)
	{
		gameObject->Input(deltaTime);
	}
}

void Scene::Update(const float& deltaTime)
{
	if (cRenderer.RebuildRendererFlag)
	{
		UpdateRenderPasses();
	}
	orthographicCamera->Update(sceneProperties);
	levelRenderer->Update(deltaTime);
}

void Scene::ImGuiUpdate(const float& deltaTime)
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

void Scene::BuildRenderPasses()
{
	levelRenderer = std::make_shared<Level2DRenderer>(Level2DRenderer("../RenderPass/DefaultRenderPass.json", ivec2(cRenderer.SwapChain.SwapChainResolution.width, cRenderer.SwapChain.SwapChainResolution.height)));
	levelRenderer->StartLevelRenderer();

	frameRenderPass = std::make_shared<FrameBufferRenderPass>(FrameBufferRenderPass("../RenderPass/FrameBufferRenderPass.json", levelRenderer->RenderedColorTextureList[0], ivec2(cRenderer.SwapChain.SwapChainResolution.width, cRenderer.SwapChain.SwapChainResolution.height)));
}

void Scene::UpdateRenderPasses()
{
	renderer.RebuildSwapChain();
	//frameRenderPass->UpdateRenderPass(levelRenderer->RenderedColorTextureList[0]);
	InterfaceRenderPass::RebuildSwapChain();
	cRenderer.RebuildRendererFlag = false;
}

void Scene::Draw()
{
	VULKAN_RESULT(renderer.StartFrame());
	CommandBufferSubmitList.emplace_back(levelRenderer->Draw(GameObjectList, sceneProperties));
	CommandBufferSubmitList.emplace_back(frameRenderPass->Draw());
	CommandBufferSubmitList.emplace_back(InterfaceRenderPass::Draw());
	VULKAN_RESULT(renderer.EndFrame(CommandBufferSubmitList));
	CommandBufferSubmitList.clear();
}

void Scene::Destroy()
{
	levelRenderer->Destroy();
	frameRenderPass->Destroy();
}