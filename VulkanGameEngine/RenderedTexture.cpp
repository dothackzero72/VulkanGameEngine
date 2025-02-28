#include "RenderedTexture.h"
#include <stb/stb_image_write.h>
#include <stb/stb_image.h>

RenderedTexture::RenderedTexture()
{

}

RenderedTexture::RenderedTexture(glm::ivec2 TextureResolution) 
{
	Width = TextureResolution.x;
	Height = TextureResolution.y;
	Depth = 1;
	TextureByteFormat = VK_FORMAT_R8G8B8A8_UNORM;
	TextureImageLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
	SampleCount = VK_SAMPLE_COUNT_1_BIT;


	CreateTextureImage();
	CreateTextureView();
	CreateTextureSampler();

	ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

RenderedTexture::RenderedTexture(glm::ivec2 TextureResolution, VkSampleCountFlagBits sampleCount) 
{
	Width = TextureResolution.x;
	Height = TextureResolution.y;
	Depth = 1;
	TextureByteFormat = VK_FORMAT_R8G8B8A8_UNORM;
	TextureImageLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
	SampleCount = sampleCount;

	CreateTextureImage();
	CreateTextureView();
	CreateTextureSampler();

	ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

RenderedTexture::RenderedTexture(glm::ivec2 TextureResolution, VkSampleCountFlagBits sampleCount, VkFormat format)
{
	Width = TextureResolution.x;
	Height = TextureResolution.y;
	Depth = 1;
	TextureByteFormat = format;
	TextureImageLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
	SampleCount = sampleCount;

	CreateTextureImage();
	CreateTextureView();
	CreateTextureSampler();

	ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

RenderedTexture::RenderedTexture(VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo)
{
	Width = (int)createImageInfo.extent.width;
	Height = (int)createImageInfo.extent.height;
	Depth = (int)createImageInfo.extent.depth;
	TextureByteFormat = createImageInfo.format;
	TextureImageLayout = createImageInfo.initialLayout;
	SampleCount = createImageInfo.samples;

	CreateTextureImage(createImageInfo);
	CreateTextureView();
	CreateTextureSampler(samplerCreateInfo);
	ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

RenderedTexture::~RenderedTexture()
{
}

VkResult RenderedTexture::CreateTextureView()
{
	VkImageViewCreateInfo textureImageViewInfo =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
		.image = Image,
		.viewType = VK_IMAGE_VIEW_TYPE_2D,
		.format = TextureByteFormat,
		.subresourceRange = VkImageSubresourceRange
		 {
			.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
			.baseMipLevel = 0,
			.levelCount = MipMapLevels,
			.baseArrayLayer = 0,
			.layerCount = 1,
		 }
	};

	if (vkCreateImageView(cRenderer.Device, &textureImageViewInfo, nullptr, &View)) {
		throw std::runtime_error("Failed to create Image View.");
	}
}

void RenderedTexture::RecreateRendererTexture(glm::vec2 TextureResolution)
{
	Width = TextureResolution.x;
	Height = TextureResolution.y;

	Texture::Destroy();
	CreateTextureImage();
	CreateTextureView();
	CreateTextureSampler();

	ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

SharedPtr<BakedTexture> RenderedTexture::BakeColorTexture(const char* filename, BakeTextureFormat textureFormat)
{
	SharedPtr<BakedTexture> bakeTexture = std::make_shared<BakedTexture>(BakedTexture(Pixel(255, 0, 0, 255), glm::ivec2(1280, 720), VkFormat::VK_FORMAT_R8G8B8A8_UNORM));

	VkCommandBuffer commandBuffer = VulkanRenderer::BeginSingleTimeCommands();

	bakeTexture->UpdateTextureLayout(commandBuffer, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
	UpdateTextureLayout(commandBuffer, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL);

	VkImageCopy copyImage{};
	copyImage.srcSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
	copyImage.srcSubresource.layerCount = 1;

	copyImage.dstSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
	copyImage.dstSubresource.layerCount = 1;

	copyImage.dstOffset.x = 0;
	copyImage.dstOffset.y = 0;
	copyImage.dstOffset.z = 0;

	copyImage.extent.width = this->Width;
	copyImage.extent.height = this->Height;
	copyImage.extent.depth = 1;

	vkCmdCopyImage(commandBuffer, this->Image, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL, bakeTexture->Image, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &copyImage);

	bakeTexture->UpdateTextureLayout(commandBuffer, VK_IMAGE_LAYOUT_GENERAL);
	UpdateTextureLayout(commandBuffer, VK_IMAGE_LAYOUT_PRESENT_SRC_KHR);
	VulkanRenderer::EndSingleTimeCommands(commandBuffer);

	VkImageSubresource subResource{ VK_IMAGE_ASPECT_COLOR_BIT, 0, 0 };
	VkSubresourceLayout subResourceLayout;
	vkGetImageSubresourceLayout(cRenderer.Device, bakeTexture->Image, &subResource, &subResourceLayout);

	const char* data;
	vkMapMemory(cRenderer.Device, bakeTexture->Memory, 0, VK_WHOLE_SIZE, 0, (void**)&data);

	bakeTexture->Destroy();
	return bakeTexture;
}

std::vector<byte> ExportColorTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, const char* filename, SharedPtr<Texture> texture, BakeTextureFormat textureFormat, uint32 channels)
{
	SharedPtr<BakedTexture> bakeTexture = std::make_shared<BakedTexture>(BakedTexture(Pixel(0x00, 0x00, 0x00, 0xFF), glm::ivec2(texture->Width, texture->Height), texture->TextureByteFormat));

	VkCommandBufferAllocateInfo allocInfo{};
	allocInfo.sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
	allocInfo.level = VK_COMMAND_BUFFER_LEVEL_PRIMARY;
	allocInfo.commandPool = commandPool;
	allocInfo.commandBufferCount = 1;

	VkCommandBuffer commandBuffer;
	vkAllocateCommandBuffers(device, &allocInfo, &commandBuffer);

	VkCommandBufferBeginInfo beginInfo{};
	beginInfo.sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
	beginInfo.flags = VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT;

	vkBeginCommandBuffer(commandBuffer, &beginInfo);

	bakeTexture->UpdateTextureLayout(commandBuffer, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
	texture->UpdateTextureLayout(commandBuffer, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL);

	VkImageCopy copyImage{};
	copyImage.srcSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
	copyImage.srcSubresource.layerCount = 1;
	copyImage.dstSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
	copyImage.dstSubresource.layerCount = 1;
	copyImage.dstOffset = { 0, 0, 0 };
	copyImage.extent.width = texture->Width;
	copyImage.extent.height = texture->Height;
	copyImage.extent.depth = 1;

	vkCmdCopyImage(commandBuffer, texture->Image, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL, bakeTexture->Image, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &copyImage);

	bakeTexture->UpdateTextureLayout(commandBuffer, VK_IMAGE_LAYOUT_GENERAL);
	texture->UpdateTextureLayout(commandBuffer, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);

	vkEndCommandBuffer(commandBuffer);

	VkSubmitInfo submitInfo{};
	submitInfo.sType = VK_STRUCTURE_TYPE_SUBMIT_INFO;
	submitInfo.commandBufferCount = 1;
	submitInfo.pCommandBuffers = &commandBuffer;

	VkResult result = vkQueueSubmit(graphicsQueue, 1, &submitInfo, VK_NULL_HANDLE);
	result = vkQueueWaitIdle(graphicsQueue);
	vkFreeCommandBuffers(device, commandPool, 1, &commandBuffer);

	const char* data = nullptr;
	vkMapMemory(device, bakeTexture->Memory, 0, VK_WHOLE_SIZE, 0, (void**)&data);

	size_t dataSize = bakeTexture->Width * bakeTexture->Height * channels;

	std::vector<uint8_t> pixelData(dataSize);
	memcpy(pixelData.data(), data, dataSize);
	vkUnmapMemory(device, bakeTexture->Memory);

	String outputFilename = String(filename);
	switch (textureFormat)
	{
	case BakeTextureFormat::Bake_BMP:
		outputFilename += ".bmp";
		stbi_write_bmp(outputFilename.c_str(), bakeTexture->Width, bakeTexture->Height, channels, pixelData.data());
		break;
	case BakeTextureFormat::Bake_JPG:
		outputFilename += ".jpg";
		stbi_write_jpg(outputFilename.c_str(), bakeTexture->Width, bakeTexture->Height, channels, pixelData.data(), 100);
		break;
	case BakeTextureFormat::Bake_PNG:
		outputFilename += ".png";
		stbi_write_png(outputFilename.c_str(), bakeTexture->Width, bakeTexture->Height, channels, pixelData.data(), channels * bakeTexture->Width);
		break;
	case BakeTextureFormat::Bake_TGA:
		outputFilename += ".tga";
		stbi_write_tga(outputFilename.c_str(), bakeTexture->Width, bakeTexture->Height, channels, pixelData.data());
		break;
	}

	bakeTexture->Destroy();
	return pixelData;
}