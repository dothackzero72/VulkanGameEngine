#include "RenderedTexture.h"
#include <stb/stb_image_write.h>
#include <stb/stb_image.h>
RenderedTexture::RenderedTexture()
{

}

RenderedTexture::RenderedTexture(glm::ivec2 TextureResolution) : Texture()
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

RenderedTexture::RenderedTexture(glm::ivec2 TextureResolution, VkSampleCountFlagBits sampleCount) : Texture()
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

RenderedTexture::~RenderedTexture()
{
}

void RenderedTexture::CreateTextureImage()
{
	VkImageCreateInfo TextureInfo = {};
	TextureInfo.sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO;
	TextureInfo.imageType = VK_IMAGE_TYPE_2D;
	TextureInfo.extent.width = Width;
	TextureInfo.extent.height = Height;
	TextureInfo.extent.depth = 1;
	TextureInfo.mipLevels = 1;
	TextureInfo.arrayLayers = 1;
	TextureInfo.initialLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
	TextureInfo.samples = SampleCount;
	TextureInfo.tiling = VK_IMAGE_TILING_OPTIMAL;
	TextureInfo.usage = VK_IMAGE_USAGE_DEPTH_STENCIL_ATTACHMENT_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT | VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT;
	TextureInfo.format = TextureByteFormat;
	vkCreateImage(cRenderer.Device, &TextureInfo, NULL, &Image);

	VkMemoryRequirements memRequirements;
	vkGetImageMemoryRequirements(cRenderer.Device, Image, &memRequirements);

	VkMemoryAllocateInfo allocInfo =
	{
		.sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
		.allocationSize = memRequirements.size,
		.memoryTypeIndex = Renderer_GetMemoryType(cRenderer.PhysicalDevice, memRequirements.memoryTypeBits, VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT)
	};
	vkAllocateMemory(cRenderer.Device, &allocInfo, NULL, &Memory);
	vkBindImageMemory(cRenderer.Device, Image, Memory, 0);
}

void RenderedTexture::CreateTextureView()
{
	VkImageViewCreateInfo TextureImageViewInfo = {};
	TextureImageViewInfo.sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO;
	TextureImageViewInfo.viewType = VK_IMAGE_VIEW_TYPE_2D;
	TextureImageViewInfo.subresourceRange = {};
	TextureImageViewInfo.subresourceRange.baseMipLevel = 0;
	TextureImageViewInfo.subresourceRange.levelCount = 1;
	TextureImageViewInfo.subresourceRange.baseArrayLayer = 0;
	TextureImageViewInfo.subresourceRange.layerCount = 1;
	TextureImageViewInfo.image = Image;
	TextureImageViewInfo.format = TextureByteFormat;
	TextureImageViewInfo.subresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;

	if (vkCreateImageView(cRenderer.Device, &TextureImageViewInfo, nullptr, &View)) {
		throw std::runtime_error("Failed to create Image View.");
	}
}

void RenderedTexture::CreateTextureSampler()
{
	VkSamplerCreateInfo TextureImageSamplerInfo = {};
	TextureImageSamplerInfo.sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO;
	TextureImageSamplerInfo.magFilter = VK_FILTER_LINEAR;
	TextureImageSamplerInfo.minFilter = VK_FILTER_LINEAR;
	TextureImageSamplerInfo.mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR;
	TextureImageSamplerInfo.addressModeU = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE;
	TextureImageSamplerInfo.addressModeV = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE;
	TextureImageSamplerInfo.addressModeW = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE;
	TextureImageSamplerInfo.mipLodBias = 0.0f;
	TextureImageSamplerInfo.maxAnisotropy = 1.0f;
	TextureImageSamplerInfo.minLod = 0.0f;
	TextureImageSamplerInfo.maxLod = 1.0f;
	TextureImageSamplerInfo.borderColor = VK_BORDER_COLOR_FLOAT_OPAQUE_WHITE;

	if (vkCreateSampler(cRenderer.Device, &TextureImageSamplerInfo, nullptr, &Sampler))
	{
		throw std::runtime_error("Failed to create Sampler.");
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

//void RenderedTexture::BakeDepthTexture(const char* filename, BakeTextureFormat textureFormat)
//{
//	std::shared_ptr<ReadableTexture> BakeTexture = std::make_shared<ReadableTexture>(ReadableTexture(glm::vec2(Width, Height), SampleCount));
//
//	VkCommandBuffer commandBuffer = VulkanRenderer::BeginSingleTimeCommands();
//
//	BakeTexture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
//	UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL);
//	Texture::CopyTexture(commandBuffer, this, BakeTexture.get());
//	BakeTexture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_GENERAL);
//	UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_PRESENT_SRC_KHR);
//	VulkanRenderer::EndSingleTimeCommands(commandBuffer);
//
//	VkImageSubresource subResource{ VK_IMAGE_ASPECT_DEPTH_BIT, 0, 0 };
//	VkSubresourceLayout subResourceLayout;
//	vkGetImageSubresourceLayout(VulkanRenderer::GetDevice(), BakeTexture->Image, &subResource, &subResourceLayout);
//
//	const char* data;
//	vkMapMemory(VulkanRenderer::GetDevice(), BakeTexture->Memory, 0, VK_WHOLE_SIZE, 0, (void**)&data);
//
//	switch (textureFormat)
//	{
//		case BakeTextureFormat::Bake_BMP: stbi_write_bmp(filename, Width, Height, STBI_grey, data); break;
//		case BakeTextureFormat::Bake_JPG: stbi_write_jpg(filename, Width, Height, STBI_grey, data, 100); break;
//		case BakeTextureFormat::Bake_PNG: stbi_write_png(filename, Width, Height, STBI_grey, data, STBI_grey * Width); break;
//		case BakeTextureFormat::Bake_TGA: stbi_write_tga(filename, Width, Height, STBI_grey, data); break;
//	}
//
//	BakeTexture->Destroy();
//}
//
std::shared_ptr<BakedTexture> RenderedTexture::BakeColorTexture(const char* filename, BakeTextureFormat textureFormat)
{
	//std::shared_ptr<Texture2D> BakeTexture = std::make_shared<Texture2D>(Texture2D(Pixel(255, 0, 0), glm::vec2(1280,720), VkFormat::VK_FORMAT_R8G8B8A8_UNORM, TextureTypeEnum::kTextureAtlus));
	std::shared_ptr<BakedTexture> BakeTexture = std::make_shared<BakedTexture>(BakedTexture(Pixel(255, 0, 0, 255), glm::ivec2(1280, 720), VkFormat::VK_FORMAT_R8G8B8A8_UNORM));

	VkCommandBuffer commandBuffer = VulkanRenderer::BeginSingleTimeCommands();

	BakeTexture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
	UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL);

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

	vkCmdCopyImage(commandBuffer, this->Image, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL, BakeTexture->Image, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &copyImage);

	BakeTexture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_GENERAL);
	UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_PRESENT_SRC_KHR);
	VulkanRenderer::EndSingleTimeCommands(commandBuffer);

	VkImageSubresource subResource{ VK_IMAGE_ASPECT_COLOR_BIT, 0, 0 };
	VkSubresourceLayout subResourceLayout;
	vkGetImageSubresourceLayout(cRenderer.Device, BakeTexture->Image, &subResource, &subResourceLayout);

	const char* data;
	vkMapMemory(cRenderer.Device, BakeTexture->Memory, 0, VK_WHOLE_SIZE, 0, (void**)&data);

	switch (textureFormat)
	{
	case BakeTextureFormat::Bake_BMP: stbi_write_bmp(filename, BakeTexture->Width, BakeTexture->Height, STBI_rgb_alpha, data); break;
	case BakeTextureFormat::Bake_JPG: stbi_write_jpg(filename, BakeTexture->Width, BakeTexture->Height, STBI_rgb_alpha, data, 100); break;
	case BakeTextureFormat::Bake_PNG: stbi_write_png(filename, BakeTexture->Width, BakeTexture->Height, STBI_rgb_alpha, data, STBI_rgb_alpha * Width); break;
	case BakeTextureFormat::Bake_TGA: stbi_write_tga(filename, BakeTexture->Width, BakeTexture->Height, STBI_rgb_alpha, data); break;
	}


	BakeTexture->Destroy();
	return BakeTexture;
}
//
//void RenderedTexture::BakeEnvironmentMapTexture(const char* filename)
//{
//	std::shared_ptr<ReadableTexture> BakeTexture = std::make_shared<ReadableTexture>(ReadableTexture(glm::vec2(Width, Height), SampleCount, VK_FORMAT_R32G32B32A32_SFLOAT));
//
//	VkCommandBuffer commandBuffer = VulkanRenderer::BeginSingleTimeCommands();
//
//	BakeTexture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
//	UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL);
//	Texture::CopyTexture(commandBuffer, this, BakeTexture.get());
//	BakeTexture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_GENERAL);
//	UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_PRESENT_SRC_KHR);
//	VulkanRenderer::EndSingleTimeCommands(commandBuffer);
//
//	VkImageSubresource subResource{ VK_IMAGE_ASPECT_COLOR_BIT, 0, 0 };
//	VkSubresourceLayout subResourceLayout;
//	vkGetImageSubresourceLayout(VulkanRenderer::GetDevice(), BakeTexture->Image, &subResource, &subResourceLayout);
//
//	const float* data;
//	vkMapMemory(VulkanRenderer::GetDevice(), BakeTexture->Memory, 0, VK_WHOLE_SIZE, 0, (void**)&data);
//	stbi_write_hdr(filename, Width, Height, STBI_rgb_alpha, data);
//	BakeTexture->Destroy();
//}
std::vector<byte> ExportColorTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, const char* filename, std::shared_ptr<Texture> texture, BakeTextureFormat textureFormat, uint32 channels)
{
	std::shared_ptr<BakedTexture> bakeTexture = std::make_shared<BakedTexture>(BakedTexture(Pixel(0x00, 0x00, 0x00, 0xFF), glm::ivec2(texture->Width, texture->Height), texture->TextureByteFormat));

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

	// Update image layouts
	bakeTexture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
	texture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL);

	// Set up the image copy operation
	VkImageCopy copyImage{};
	copyImage.srcSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
	copyImage.srcSubresource.layerCount = 1;
	copyImage.dstSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
	copyImage.dstSubresource.layerCount = 1;
	copyImage.dstOffset = { 0, 0, 0 };
	copyImage.extent.width = texture->Width;
	copyImage.extent.height = texture->Height;
	copyImage.extent.depth = 1;

	// Perform the image copy
	vkCmdCopyImage(commandBuffer, texture->Image, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL, bakeTexture->Image, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &copyImage);

	// Finalize layout transitions
	bakeTexture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_GENERAL);
	texture->UpdateImageLayout(commandBuffer, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);

	vkEndCommandBuffer(commandBuffer);

	VkSubmitInfo submitInfo{};
	submitInfo.sType = VK_STRUCTURE_TYPE_SUBMIT_INFO;
	submitInfo.commandBufferCount = 1;
	submitInfo.pCommandBuffers = &commandBuffer;

	VkResult result = vkQueueSubmit(graphicsQueue, 1, &submitInfo, VK_NULL_HANDLE);
	result = vkQueueWaitIdle(graphicsQueue);

	vkFreeCommandBuffers(device, commandPool, 1, &commandBuffer);

	// Map the memory to read pixel data
	const char* data = nullptr;
	vkMapMemory(device, bakeTexture->Memory, 0, VK_WHOLE_SIZE, 0, (void**)&data);

	// Calculate the size of the pixel data
	size_t dataSize = bakeTexture->Width * bakeTexture->Height * channels;

	// Create a vector to hold the pixel data
	std::vector<uint8_t> pixelData(dataSize);
	memcpy(pixelData.data(), data, dataSize); // Copy the mapped data to pixelData

	// Unmap memory after reading
	vkUnmapMemory(device, bakeTexture->Memory);

	// Create the output filename with the appropriate extension
	std::string outputFilename = std::string(filename);
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

	// Clean up the baked texture
	bakeTexture->Destroy();
	return pixelData;
}