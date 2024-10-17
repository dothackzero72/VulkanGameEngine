#include "Scene.h"
#include "VulkanRenderer.h"
#include "Texture.h"
#include "SceneDataBuffer.h"
#include "implot.h"
#include "BakedTexture.h"
#include <stb/stb_image_write.h>
#include <stb/stb_image.h>

void Scene::StartUp()
{
	std::vector<uint32> SpriteIndexList = {
	   0, 1, 3,
	   1, 2, 3
	};

	//Timer timer;
	//timer.Time = 0.0f;

	texture = std::make_shared<BakedTexture>(BakedTexture("../Textures/awesomeface.png", VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum::kType_DiffuseTextureMap));
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
	VkCommandBuffer commandBuffer = renderer.BeginSingleTimeCommands();
	mesh->BufferUpdate(commandBuffer, deltaTime);
	renderer.EndSingleTimeCommands(commandBuffer);

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

	//BakeCubeTextureAtlus("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/asdfa43.bmp", texture);
	//ExportColorTexture(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, "C:/Users/dotha/Documents/GitHub/VulkanGameEngine/asdfa.bmp", renderPass2D.GetRenderedTexture(), Bake_BMP, 4);
}

void Scene::BakeCubeTextureAtlus(const std::string& FilePath, std::shared_ptr<BakedTexture> texture)
{
	
	auto	bakeTexture = std::make_shared<BakedTexture>(BakedTexture(Pixel(255, 0, 0, 255), ivec2(texture->Width, texture->Height), VkFormat::VK_FORMAT_B8G8R8A8_UNORM));
	

	VkCommandBuffer commandBuffer = VulkanRenderer::BeginSingleTimeCommands();

	texture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL);
	
	
		bakeTexture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_UNDEFINED, VK_IMAGE_LAYOUT_GENERAL);
		bakeTexture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_GENERAL, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);


			VkImageCopy copyImage{};
			copyImage.srcSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
			copyImage.srcSubresource.layerCount = 1;

			copyImage.dstSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
			copyImage.dstSubresource.layerCount = 1;

			copyImage.dstOffset.x = 0;
			copyImage.dstOffset.y = 0;
			copyImage.dstOffset.z = 0;

			copyImage.extent.width = texture->Width;
			copyImage.extent.height = texture->Height;
			copyImage.extent.depth = 1;

			vkCmdCopyImage(commandBuffer, texture->Image, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL, bakeTexture->Image, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &copyImage);
		
		
	

			bakeTexture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, VK_IMAGE_LAYOUT_GENERAL);

		texture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL, VK_IMAGE_LAYOUT_PRESENT_SRC_KHR);
	
	VulkanRenderer::EndSingleTimeCommands(commandBuffer);


		VkImageSubresource subResource{ VK_IMAGE_ASPECT_COLOR_BIT, 0, 0 };
		VkSubresourceLayout subResourceLayout;
		vkGetImageSubresourceLayout(cRenderer.Device, bakeTexture->Image, &subResource, &subResourceLayout);

		const char* data;
		vkMapMemory(cRenderer.Device, bakeTexture->Memory, 0, VK_WHOLE_SIZE, 0, (void**)&data);

		std::string textureloc = FilePath;
		textureloc.append("asdfasd.bmp");
		stbi_write_bmp(textureloc.c_str(), bakeTexture->Width, bakeTexture->Height, STBI_rgb_alpha, data);

		bakeTexture->Destroy();
	
}

//void Scene::WriteTextureToFile(const std::string& filePath, std::shared_ptr<Texture> texture)
//{
//	Pixel pixel = Pixel(0xFF, 0x00, 0x00, 0xFF);
//	BakedTexture newTexture(pixel, glm::ivec2(texture->Width, texture->Height), VK_FORMAT_R8G8B8A8_UNORM);
//
//	VkCommandBuffer commandBuffer;
//	VkCommandBufferAllocateInfo allocInfo = {};
//	allocInfo.sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
//	allocInfo.commandPool = cRenderer.CommandPool;
//	allocInfo.level = VK_COMMAND_BUFFER_LEVEL_PRIMARY;
//	allocInfo.commandBufferCount = 1;
//
//	VULKAN_RESULT(vkAllocateCommandBuffers(cRenderer.Device, &allocInfo, &commandBuffer));
//
//	VkCommandBufferBeginInfo beginInfo = {};
//	beginInfo.sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
//	beginInfo.flags = VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT;
//
//	VULKAN_RESULT(vkBeginCommandBuffer(commandBuffer, &beginInfo));
//
//	// Transition source texture to transfer source
//	texture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL);
//
//	// Transition new texture to destination layout
//	newTexture.UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
//
//	// Copy texture data
//	VkImageCopy copyRegion = {};
//	copyRegion.srcSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
//	copyRegion.srcSubresource.mipLevel = 0;
//	copyRegion.srcSubresource.baseArrayLayer = 0;
//	copyRegion.srcSubresource.layerCount = 1;
//	copyRegion.srcOffset = { 0, 0, 0 };
//	copyRegion.dstSubresource = copyRegion.srcSubresource;
//	copyRegion.dstOffset = { 0, 0, 0 };
//	copyRegion.extent.width = texture->Width;
//	copyRegion.extent.height = texture->Height;
//	copyRegion.extent.depth = 1;
//
//	vkCmdCopyImage(commandBuffer,
//		texture->Image, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL,
//		newTexture.Image, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
//		1, &copyRegion);
//
//	VULKAN_RESULT(vkEndCommandBuffer(commandBuffer));
//
//	VkSubmitInfo submitInfo = {};
//	submitInfo.sType = VK_STRUCTURE_TYPE_SUBMIT_INFO;
//	submitInfo.commandBufferCount = 1;
//	submitInfo.pCommandBuffers = &commandBuffer;
//
//	VULKAN_RESULT(vkQueueSubmit(cRenderer.SwapChain.GraphicsQueue, 1, &submitInfo, VK_NULL_HANDLE));
//	VULKAN_RESULT(vkQueueWaitIdle(cRenderer.SwapChain.GraphicsQueue));
//	vkFreeCommandBuffers(cRenderer.Device, cRenderer.CommandPool, 1, &commandBuffer);
//
//	// Map new texture's memory and prepare to write to BMP
//	void* mappedData;
//	VULKAN_RESULT(vkMapMemory(cRenderer.Device, newTexture.Memory, 0, VK_WHOLE_SIZE, 0, &mappedData));
//
//	// Prepare BMP data
//	std::string textureFilePath = filePath + "/texture.bmp";
//	size_t rowSize = 4 * newTexture.Width; // 4 bytes for RGBA
//	size_t alignedRowSize = (rowSize + 3) & ~3; // Align row size to multiple of 4
//
//	// Optionally allocate a buffer for writing out the image
//	std::vector<uint8_t> bmpData(alignedRowSize * newTexture.Height);
//
//	// Copy pixels to BMP data, taking care of alignment
//	for (int y = 0; y < newTexture.Height; ++y) {
//		uint8_t* srcRow = static_cast<uint8_t*>(mappedData) + (y * rowSize);
//		uint8_t* destRow = bmpData.data() + (y * alignedRowSize);
//		std::memcpy(destRow, srcRow, rowSize);
//	}
//
//	// Write BMP file
//	stbi_write_bmp(textureFilePath.c_str(), newTexture.Width, newTexture.Height, STBI_rgb_alpha, bmpData.data());
//
//	// Clean up
//	vkUnmapMemory(cRenderer.Device, newTexture.Memory);
//}

void Scene::Destroy()
{

	mesh->Destroy();
	texture->Destroy();
	renderPass2D.Destroy();
	frameRenderPass.Destroy();
}
