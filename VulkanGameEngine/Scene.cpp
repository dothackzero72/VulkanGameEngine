#include "Scene.h"
#include "VulkanRenderer.h"
#include "Texture.h"
#include "SceneDataBuffer.h"
#include "implot.h"

void Scene::StartUp()
{
	std::vector<uint32> SpriteIndexList = {
	   0, 1, 3,
	   1, 2, 3
	};

	//Timer timer;
	//timer.Time = 0.0f;

	texture = std::make_shared<Texture>(Texture("../Textures/awesomeface.png", VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum::kType_DiffuseTextureMap));
	mesh = std::make_shared<Mesh2D>(Mesh2D(SpriteVertexList, SpriteIndexList));
	orthographicCamera = std::make_shared<OrthographicCamera>(OrthographicCamera(vec2((float)cRenderer.SwapChain.SwapChainResolution.width, (float)cRenderer.SwapChain.SwapChainResolution.height), vec3(0.0f, 0.0f, 5.0f)));

	BuildRenderPasses();
}

void Scene::Update(const float& deltaTime)
{
	if (cRenderer.RebuildRendererFlag)
	{
		UpdateRenderPasses();
	}
	VkCommandBuffer commandBuffer = renderer.BeginCommandBuffer();
	mesh->BufferUpdate(commandBuffer, deltaTime);
	renderer.EndCommandBuffer(commandBuffer);

	orthographicCamera->Update(sceneProperties);
	if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_W] == KS_PRESSED)
	{
		orthographicCamera->Position.y -= 1.0f * deltaTime;
	}
	if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_A] == KS_PRESSED)
	{
		orthographicCamera->Position.x += 1.0f * deltaTime;
	}
	if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_S] == KS_PRESSED)
	{
		orthographicCamera->Position.y += 1.0f * deltaTime;
	}
	if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_D] == KS_PRESSED)
	{
		orthographicCamera->Position.x -= 1.0f * deltaTime;
	}
}

void Scene::ImGuiUpdate(const float& deltaTime)
{
	ImGui_ImplVulkan_NewFrame();
	ImGui_ImplGlfw_NewFrame();
	
	ImGui::NewFrame();
		ImGui::Begin("Button Window");
		ImGui::Text("Application average %.3f ms/frame (%.1f FPS)", 1000.0f / ImGui::GetIO().Framerate, ImGui::GetIO().Framerate);
		texture.get()->ImGuiShowTexture(ImVec2(256, 128));
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
	renderPass2D.BuildRenderPass(mesh);
	frameRenderPass.BuildRenderPass(renderPass2D.GetRenderedTexture());
}

void Scene::UpdateRenderPasses()
{
	renderer.RebuildSwapChain();
	renderPass2D.UpdateRenderPass(mesh);
	frameRenderPass.UpdateRenderPass(renderPass2D.GetRenderedTexture());
	InterfaceRenderPass::RebuildSwapChain();
	cRenderer.RebuildRendererFlag = false;
}

void Scene::Draw()
{
	std::vector<VkCommandBuffer> CommandBufferSubmitList;

	VULKAN_RESULT(renderer.StartFrame());
	CommandBufferSubmitList.emplace_back(renderPass2D.Draw(mesh, sceneProperties));
	CommandBufferSubmitList.emplace_back(frameRenderPass.Draw());
	CommandBufferSubmitList.emplace_back(InterfaceRenderPass::Draw());
	VULKAN_RESULT(renderer.EndFrame(CommandBufferSubmitList));
}

void Scene::Destroy()
{
	mesh->Destroy();
	texture->Destroy();
	renderPass2D.Destroy();
	frameRenderPass.Destroy();
}
