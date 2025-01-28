#include "Scene.h"
#include "VulkanRenderer.h"
#include "Texture.h"
#include "SceneDataBuffer.h"
#include "implot.h"
#include "BakedTexture.h"
#include <stb/stb_image_write.h>
#include <stb/stb_image.h>
#include "MemoryManager.h"

void Scene::StartUp()
{
	orthographicCamera = std::make_shared<OrthographicCamera2D>(OrthographicCamera2D(vec2((float)cRenderer.SwapChain.SwapChainResolution.width, (float)cRenderer.SwapChain.SwapChainResolution.height), vec3(0.0f, 0.0f, 0.0f)));
//	GameObjectList.emplace_back(GameObject::CreateGameObject("adsfda", List<ComponentTypeEnum> { kTransform2DComponent, kInputComponent, kRenderMesh2DComponent }));
	TextureList.emplace_back(Texture::CreateTexture("../Textures/awesomeface.png", VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum::kType_DiffuseTextureMap));
	TextureList.emplace_back(Texture::CreateTexture("../Textures/container2.png", VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum::kType_DiffuseTextureMap));

	material = Material::CreateMaterial("Material1");
	material->SetAlbedoMap(MemoryManager::GetTextureList()[0]);

	Material2 = Material::CreateMaterial("Material2");
	Material2->SetAlbedoMap(MemoryManager::GetTextureList()[1]);

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
	MemoryManager::Update(deltaTime);
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
	frameRenderPass.BuildRenderPass(levelRenderer->RenderedColorTextureList[0]);
}

void Scene::UpdateRenderPasses()
{
	renderer.RebuildSwapChain();
	frameRenderPass.UpdateRenderPass(levelRenderer->RenderedColorTextureList[0]);
	InterfaceRenderPass::RebuildSwapChain();
	cRenderer.RebuildRendererFlag = false;
}

void Scene::Draw()
{
	VULKAN_RESULT(renderer.StartFrame());
	CommandBufferSubmitList.emplace_back(levelRenderer->Draw(GameObjectList, sceneProperties));
	CommandBufferSubmitList.emplace_back(frameRenderPass.Draw());
	CommandBufferSubmitList.emplace_back(InterfaceRenderPass::Draw());
	VULKAN_RESULT(renderer.EndFrame(CommandBufferSubmitList));
	CommandBufferSubmitList.clear();
}

void Scene::Destroy()
{
	for (auto gameObject : GameObjectList)
	{
		gameObject->Destroy();
	}
	for (auto texture : TextureList)
	{
		texture->Destroy();
	}
	levelRenderer->Destroy();
	frameRenderPass.Destroy();
	GameObjectList.clear();
	TextureList.clear();
}