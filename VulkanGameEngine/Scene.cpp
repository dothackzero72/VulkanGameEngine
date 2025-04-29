#include "Scene.h"
#include "VulkanRenderer.h"
#include <Texture.h>
#include "SceneDataBuffer.h"
#include "implot.h"
#include <stb/stb_image_write.h>
#include <stb/stb_image.h>
#include "RenderSystem.h"

void Scene::StartUp()
{
	orthographicCamera = std::make_shared<OrthographicCamera2D>(OrthographicCamera2D(vec2((float)cRenderer.SwapChain.SwapChainResolution.width, (float)cRenderer.SwapChain.SwapChainResolution.height), vec3(0.0f, 0.0f, 0.0f)));
	//MemoryManager::ViewMemoryMap();
	BuildRenderPasses();
}

void Scene::Input(float deltaTime)
{
	//for (auto gameObject : GameObjectList)
	//{
	//	gameObject->Input(deltaTime);
	//}
}

void Scene::Update(const float& deltaTime)
{
	if (cRenderer.RebuildRendererFlag)
	{
		UpdateRenderPasses();
	}
	orthographicCamera->Update(sceneProperties);
	renderSystem.Update(deltaTime);
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
	renderSystem.RenderSystemStartUp();
	 renderPass2DId = renderSystem.AddRenderPass("../RenderPass/Default2DRenderPass.json", ivec2(cRenderer.SwapChain.SwapChainResolution.width, cRenderer.SwapChain.SwapChainResolution.height));
	 frameBufferId = renderSystem.AddRenderPass("../RenderPass/FrameBufferRenderPass.json", renderSystem.RenderedTextureList[renderPass2DId][0], ivec2(cRenderer.SwapChain.SwapChainResolution.width, cRenderer.SwapChain.SwapChainResolution.height));
}

void Scene::UpdateRenderPasses()
{
	renderer.RebuildSwapChain();
	//frameRenderPass->UpdateRenderPass(levelRenderer->RenderedColorTextureList[0]);
	InterfaceRenderPass::RebuildSwapChain();
	cRenderer.RebuildRendererFlag = false;
}

void Scene::Draw(const float& deltaTime)
{
	VULKAN_RESULT(renderer.StartFrame());
	CommandBufferSubmitList.emplace_back(renderSystem.RenderSprites(renderPass2DId, deltaTime, sceneProperties));
	CommandBufferSubmitList.emplace_back(renderSystem.RenderFrameBuffer(frameBufferId));
	CommandBufferSubmitList.emplace_back(InterfaceRenderPass::Draw());
	VULKAN_RESULT(renderer.EndFrame(CommandBufferSubmitList));
	CommandBufferSubmitList.clear();
}

void Scene::Destroy()
{
	levelRenderer->Destroy();
	//frameRenderPass->Destroy();
}