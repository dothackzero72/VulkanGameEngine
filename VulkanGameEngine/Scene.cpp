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
	//Timer timer;
	//timer.Time = 0.0f;
	texture2 = std::make_shared<Texture>(Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\Textures\\awesomeface.png", VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum::kType_DiffuseTextureMap));
	//texture.emplace_back(std::make_shared<Texture>(Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\Textures\\awesomeface.png", VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum::kType_DiffuseTextureMap)));
	//texture.emplace_back(std::make_shared<Texture>(Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\Textures\\texture.jpg", VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum::kType_DiffuseTextureMap)));
	orthographicCamera = std::make_shared<OrthographicCamera>(OrthographicCamera(vec2((float)cRenderer.SwapChain.SwapChainResolution.width, (float)cRenderer.SwapChain.SwapChainResolution.height), vec3(0.0f, 0.0f, 5.0f)));
	gameObjectList.emplace_back(GameObject::CreateGameObject("adsfda", List<ComponentTypeEnum> { kRenderMesh2DComponent }));
	gameObjectList.emplace_back(GameObject::CreateGameObject("adsfda1", List<ComponentTypeEnum> { kRenderMesh2DComponent }));
	TextureList.emplace_back(Texture::CreateTexture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\Textures\\awesomeface.png", VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum::kType_DiffuseTextureMap));
	TextureList.emplace_back(Texture::CreateTexture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\Textures\\container2.png", VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum::kType_DiffuseTextureMap));

	//const auto conponent = gameObjectList.front()->GetComponentByComponentType(kRenderMesh2DComponent);
	//const auto meshRenderer = dynamic_cast<RenderMesh2DComponent*>(conponent.get());
	//meshRenderer->GetMesh2D()->MeshPosition = ivec3(1, 2, 0);

	MemoryManager::ViewMemoryMap();
	BuildRenderPasses();
}

void Scene::Update(const float& deltaTime)
{
	if (cRenderer.RebuildRendererFlag)
	{
		UpdateRenderPasses();
	}
	VkCommandBuffer commandBuffer = renderer.BeginSingleTimeCommands();
	for (auto gameObject : gameObjectList)
	{
		gameObject->BufferUpdate(commandBuffer, deltaTime);
	}
	renderer.EndSingleTimeCommands(commandBuffer);

	orthographicCamera->Update(sceneProperties);
	if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_W] == KS_PRESSED)
	{
		const auto conponent = gameObjectList.front()->GetComponentByComponentType(kRenderMesh2DComponent);
		const auto meshRenderer = dynamic_cast<RenderMesh2DComponent*>(conponent.get());
		meshRenderer->GetMesh2D()->MeshPosition.x += 1.0f * deltaTime;

		//orthographicCamera->Position.y -= 1.0f * deltaTime;
	}
	if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_A] == KS_PRESSED)
	{
		const auto conponent = gameObjectList.front()->GetComponentByComponentType(kRenderMesh2DComponent);
		const auto meshRenderer = dynamic_cast<RenderMesh2DComponent*>(conponent.get());
		meshRenderer->GetMesh2D()->MeshPosition.x -= 1.0f * deltaTime;

		//orthographicCamera->Position.x += 1.0f * deltaTime;
	}
	if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_S] == KS_PRESSED)
	{
		const auto conponent = gameObjectList.front()->GetComponentByComponentType(kRenderMesh2DComponent);
		const auto meshRenderer = dynamic_cast<RenderMesh2DComponent*>(conponent.get());
		meshRenderer->GetMesh2D()->MeshPosition.y -= 1.0f * deltaTime;

		//orthographicCamera->Position.y += 1.0f * deltaTime;
	}
	if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_D] == KS_PRESSED)
	{
		const auto conponent = gameObjectList.front()->GetComponentByComponentType(kRenderMesh2DComponent);
		const auto meshRenderer = dynamic_cast<RenderMesh2DComponent*>(conponent.get());
		meshRenderer->GetMesh2D()->MeshPosition.y += 1.0f * deltaTime;

		//orthographicCamera->Position.x -= 1.0f * deltaTime;
	}
	if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_D] == KS_PRESSED)
	{
		orthographicCamera->Zoom -= 1.0f * deltaTime;
	}
	if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_D] == KS_PRESSED)
	{
		orthographicCamera->Zoom += 1.0f * deltaTime;
	}
}

void Scene::ImGuiUpdate(const float& deltaTime)
{
	ImGui_ImplVulkan_NewFrame();
	ImGui_ImplGlfw_NewFrame();

	ImGui::NewFrame();
	ImGui::Begin("Button Window");
	ImGui::Text("Application average %.3f ms/frame (%.1f FPS)", 1000.0f / ImGui::GetIO().Framerate, ImGui::GetIO().Framerate);
	texture2.get()->ImGuiShowTexture(ImVec2(256, 128));
	ImGui::End();


	//ImGui::Begin("Basic Graph");

	//// Sample data for the graph
	//static double x_data[100], y_data[100];
	//for (int i = 0; i < 100; ++i) {
	//	x_data[i] = i * 0.1;     // x values from 0 to 9.9
	//	y_data[i] = sin(x_data[i]); // Example: sine function
	//}

	//// Create a plot
	//ImPlot::BeginPlot("Sine Wave");
	//ImPlot::PlotLine("sin(x)", x_data, y_data, 100);
	//ImPlot::EndPlot();

	//ImGui::End();


	ImGui::Render();
}

void Scene::BuildRenderPasses()
{
	renderPass2D.BuildRenderPass(texture, texture2);
	//renderPass3D.JsonCreateRenderPass("C://Users//dotha//Documents//GitHub//VulkanGameEngine//RenderPass//DefaultRenderPass.json");
	frameRenderPass.BuildRenderPass(renderPass2D.GetRenderedTexture());
}

void Scene::UpdateRenderPasses()
{
	renderer.RebuildSwapChain();
	renderPass2D.UpdateRenderPass(texture, texture2);
	frameRenderPass.UpdateRenderPass(renderPass2D.GetRenderedTexture());
	InterfaceRenderPass::RebuildSwapChain();
	cRenderer.RebuildRendererFlag = false;
}

void Scene::Draw()
{
	std::vector<VkCommandBuffer> CommandBufferSubmitList;

	VULKAN_RESULT(renderer.StartFrame());
	CommandBufferSubmitList.emplace_back(renderPass2D.Draw(gameObjectList, sceneProperties));
	CommandBufferSubmitList.emplace_back(frameRenderPass.Draw());
	CommandBufferSubmitList.emplace_back(InterfaceRenderPass::Draw());
	VULKAN_RESULT(renderer.EndFrame(CommandBufferSubmitList));

	//BakeCubeTextureAtlus("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/asdfa43.bmp", texture);
	//ExportColorTexture(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, "C:/Users/dotha/Documents/GitHub/VulkanGameEngine/asdfa.bmp", renderPass2D.GetRenderedTexture(), Bake_BMP, 4);
}

void Scene::Destroy()
{
	for (auto gameObject : gameObjectList)
	{
		gameObject->Destroy();
	}
	for (auto texture : TextureList)
	{
		texture->Destroy();
	}
	renderPass2D.Destroy();
	frameRenderPass.Destroy();
}