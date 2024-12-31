#include "Texture.h"
#define STB_IMAGE_IMPLEMENTATION
#define STB_IMAGE_WRITE_IMPLEMENTATION
#include <stb/stb_image.h>
#include <stb/stb_image_write.h>
#include <pixel.h>

#ifdef max 
#undef max
#endif
#include "MemoryManager.h"

Texture::Texture()
{
	TextureSetUp();
}

Texture::Texture(const Pixel& clearColor, int width, int height, VkFormat textureByteFormat, TextureTypeEnum textureType)
{
	TextureSetUp();
	Width = width;
	Height = height;
	TextureType = textureType;
	TextureByteFormat = textureByteFormat;

	CreateImageTexture(clearColor);
	CreateTextureView();
	CreateTextureSampler();
	ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

Texture::Texture(const String& filePath, VkFormat textureByteFormat, TextureTypeEnum textureType)
{
	TextureSetUp();
	MipMapLevels = static_cast<uint32>(std::floor(std::log2(std::max(Width, Height)))) + 1;
	TextureType = textureType;
	TextureByteFormat = textureByteFormat;

	CreateImageTexture(filePath);
	CreateTextureView();
	CreateTextureSampler();
	ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

Texture::Texture(VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo)
{
	TextureSetUp();
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

Texture::~Texture()
{

}

SharedPtr<Texture> Texture::CreateTexture(const Pixel& clearColor, int width, int height, VkFormat textureByteFormat, TextureTypeEnum textureType)
{
	SharedPtr<Texture> texture = MemoryManager::AllocateNewTexture();
	new (texture.get()) Texture(clearColor, width, height, textureByteFormat, textureType);
	return texture;
}

SharedPtr<Texture> Texture::CreateTexture(const String& filePath, VkFormat textureByteFormat, TextureTypeEnum TextureType)
{
	SharedPtr<Texture> texture = MemoryManager::AllocateNewTexture();
	new (texture.get()) Texture(filePath, textureByteFormat, TextureType);
	return texture;
}

SharedPtr<Texture> Texture::CreateTexture(VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo)
{
	SharedPtr<Texture> texture = MemoryManager::AllocateNewTexture();
	new (texture.get()) Texture(createImageInfo, samplerCreateInfo);
	return texture;
}

void Texture::TextureSetUp()
{
	Name = "Texture";
	TextureBufferIndex = 0;
	Width = 1;
	Height = 1;
	Depth = 1;
	MipMapLevels = 1;

	ImGuiDescriptorSet = VK_NULL_HANDLE;
	Image = VK_NULL_HANDLE;
	Memory = VK_NULL_HANDLE;
	View = VK_NULL_HANDLE;
	Sampler = VK_NULL_HANDLE;

	TextureUsage = TextureUsageEnum::kUse_Undefined;
	TextureType = TextureTypeEnum::kType_UndefinedTexture;
	TextureByteFormat = VK_FORMAT_UNDEFINED;
	TextureImageLayout = VK_IMAGE_LAYOUT_UNDEFINED;
	SampleCount = VK_SAMPLE_COUNT_1_BIT;
}

void Texture::UpdateTextureSize(glm::vec2 TextureResolution)
{
	renderer.DestroyImageView(View);
	renderer.DestroySampler(Sampler);
	renderer.DestroyImage(Image);
	renderer.FreeDeviceMemory(Memory);
	ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

void Texture::UpdateTextureBufferIndex(uint64_t bufferIndex)
{
	TextureBufferIndex = bufferIndex;
}

void Texture::Destroy()
{
	renderer.DestroyImageView(View);
	renderer.DestroySampler(Sampler);
	renderer.DestroyImage(Image);
	renderer.FreeDeviceMemory(Memory);
}

void Texture::CreateImageTexture(const Pixel& clearColor)
{
	ColorChannels = ColorChannelUsed::ChannelRGBA;
	uint32 size = Width * Height * ColorChannels;

	std::vector<Pixel> pixels(Width * Height, clearColor);

	VulkanBuffer<byte> stagingBuffer((void*)pixels.data(), pixels.size(), VK_BUFFER_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, false);

	VULKAN_RESULT(NewTextureImage());
	VULKAN_RESULT(TransitionImageLayout(VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL));
	VULKAN_RESULT(CopyBufferToTexture(stagingBuffer.Buffer));
	stagingBuffer.DestroyBuffer();

}

void Texture::CreateImageTexture(const String& FilePath)
{
	int* width = &Width;
	int* height = &Height;
	int colorChannels = 0;
	byte* data = stbi_load(FilePath.c_str(), width, height, &colorChannels, 0);
	VulkanBuffer<byte> stagingBuffer((void*)data, Width * Height * colorChannels, VK_BUFFER_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, false);

	VULKAN_RESULT(CreateTextureImage());
	VULKAN_RESULT(TransitionImageLayout(VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL));
	VULKAN_RESULT(CopyBufferToTexture(stagingBuffer.Buffer));
	VULKAN_RESULT(GenerateMipmaps());
	stagingBuffer.DestroyBuffer();

	ColorChannels = (ColorChannelUsed)colorChannels;
	stbi_image_free(data);
}

void Texture::CreateTextureSampler()
{
	VkSamplerCreateInfo TextureImageSamplerInfo = 
	{
		.sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
		.magFilter = VK_FILTER_NEAREST,
		.minFilter = VK_FILTER_NEAREST,
		.mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR,
		.addressModeU = VK_SAMPLER_ADDRESS_MODE_REPEAT,
		.addressModeV = VK_SAMPLER_ADDRESS_MODE_REPEAT,
		.addressModeW = VK_SAMPLER_ADDRESS_MODE_REPEAT,
		.mipLodBias = 0,
		.anisotropyEnable = VK_TRUE,
		.maxAnisotropy = 16.0f,
		.compareEnable = VK_FALSE,
		.compareOp = VK_COMPARE_OP_ALWAYS,
		.minLod = 0,
		.maxLod = static_cast<float>(MipMapLevels),
		.borderColor = VK_BORDER_COLOR_INT_OPAQUE_BLACK,
		.unnormalizedCoordinates = VK_FALSE,
	};
	VULKAN_RESULT(CreateTextureSampler(TextureImageSamplerInfo));
}

void Texture::GetTexturePropertiesBuffer(std::vector<VkDescriptorImageInfo>& textureDescriptorList)
{
	VkDescriptorImageInfo textureDescriptor =
	{
		.sampler = Sampler,
		.imageView = View,
		.imageLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL
	};
	textureDescriptorList.emplace_back(textureDescriptor);
}

void Texture::ImGuiShowTexture(const ImVec2& TextureDisplaySize)
{
	ImGui::Image(ImGuiDescriptorSet, TextureDisplaySize);
}

void Texture::UpdateImageLayout(VkImageLayout newImageLayout)
{
	VkImageSubresourceRange ImageSubresourceRange{};
	ImageSubresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
	ImageSubresourceRange.baseMipLevel = 0;
	ImageSubresourceRange.levelCount = VK_REMAINING_MIP_LEVELS;
	ImageSubresourceRange.layerCount = 1;

	VkImageMemoryBarrier ImageMemoryBarrier = {};
	ImageMemoryBarrier.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
	ImageMemoryBarrier.oldLayout = TextureImageLayout;
	ImageMemoryBarrier.newLayout = newImageLayout;
	ImageMemoryBarrier.image = Image;
	ImageMemoryBarrier.subresourceRange = ImageSubresourceRange;
	ImageMemoryBarrier.srcAccessMask = 0;
	ImageMemoryBarrier.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;

	auto SingleCommand = Renderer_BeginSingleUseCommandBuffer(cRenderer.Device, cRenderer.CommandPool);
	vkCmdPipelineBarrier(SingleCommand, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, nullptr, 0, nullptr, 1, &ImageMemoryBarrier);
	VkResult result = Renderer_EndSingleUseCommandBuffer(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, SingleCommand);
	if (result == VK_SUCCESS)
	{
		TextureImageLayout = newImageLayout;
	}
}

void Texture::UpdateImageLayout(VkImageLayout newImageLayout, uint32_t MipLevel)
{
	VkImageSubresourceRange ImageSubresourceRange{};
	ImageSubresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
	ImageSubresourceRange.baseMipLevel = MipLevel;
	ImageSubresourceRange.levelCount = VK_REMAINING_MIP_LEVELS;
	ImageSubresourceRange.layerCount = 1;

	VkImageMemoryBarrier ImageMemoryBarrier = {};
	ImageMemoryBarrier.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
	ImageMemoryBarrier.oldLayout = TextureImageLayout;
	ImageMemoryBarrier.newLayout = newImageLayout;
	ImageMemoryBarrier.image = Image;
	ImageMemoryBarrier.subresourceRange = ImageSubresourceRange;
	ImageMemoryBarrier.srcAccessMask = 0;
	ImageMemoryBarrier.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;

	auto SingleCommand = Renderer_BeginSingleUseCommandBuffer(cRenderer.Device, cRenderer.CommandPool);
	vkCmdPipelineBarrier(SingleCommand, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, nullptr, 0, nullptr, 1, &ImageMemoryBarrier);
	VkResult result = Renderer_EndSingleUseCommandBuffer(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, SingleCommand);
	if (result == VK_SUCCESS)
	{
		TextureImageLayout = newImageLayout;
	}
}

void Texture::UpdateImageLayout(VkCommandBuffer& commandBuffer, VkImageLayout oldImageLayout, VkImageLayout newImageLayout)
{
	VkImageSubresourceRange ImageSubresourceRange{};
	ImageSubresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
	ImageSubresourceRange.baseMipLevel = 0;
	ImageSubresourceRange.levelCount = VK_REMAINING_MIP_LEVELS;
	ImageSubresourceRange.layerCount = 1;

	VkImageMemoryBarrier barrier = {};
	barrier.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
	barrier.oldLayout = oldImageLayout;
	barrier.newLayout = newImageLayout;
	barrier.image = Image;
	barrier.subresourceRange = ImageSubresourceRange;
	barrier.srcAccessMask = 0;
	barrier.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;

	vkCmdPipelineBarrier(commandBuffer, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, nullptr, 0, nullptr, 1, &barrier);
	TextureImageLayout = newImageLayout;
}

void Texture::UpdateImageLayout(VkCommandBuffer& commandBuffer, VkImageLayout oldImageLayout, VkImageLayout newImageLayout, uint32_t MipLevel)
{
	VkImageSubresourceRange ImageSubresourceRange{};
	ImageSubresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
	ImageSubresourceRange.baseMipLevel = MipLevel;
	ImageSubresourceRange.levelCount = VK_REMAINING_MIP_LEVELS;
	ImageSubresourceRange.layerCount = 1;

	VkImageMemoryBarrier barrier = {};
	barrier.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
	barrier.oldLayout = oldImageLayout;
	barrier.newLayout = newImageLayout;
	barrier.image = Image;
	barrier.subresourceRange = ImageSubresourceRange;
	barrier.srcAccessMask = 0;
	barrier.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;

	vkCmdPipelineBarrier(commandBuffer, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, nullptr, 0, nullptr, 1, &barrier);
	TextureImageLayout = newImageLayout;
}

void Texture::UpdateImageLayout(VkCommandBuffer& commandBuffer, VkImageLayout newImageLayout)
{
	VkImageSubresourceRange ImageSubresourceRange{};
	ImageSubresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
	ImageSubresourceRange.baseMipLevel = 0;
	ImageSubresourceRange.levelCount = VK_REMAINING_MIP_LEVELS;
	ImageSubresourceRange.layerCount = 1;

	VkImageMemoryBarrier barrier = {};
	barrier.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
	barrier.oldLayout = TextureImageLayout;
	barrier.newLayout = newImageLayout;
	barrier.image = Image;
	barrier.subresourceRange = ImageSubresourceRange;
	barrier.srcAccessMask = 0;
	barrier.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;

	vkCmdPipelineBarrier(commandBuffer, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, nullptr, 0, nullptr, 1, &barrier);
	TextureImageLayout = newImageLayout;
}

void Texture::UpdateImageLayout(VkCommandBuffer& commandBuffer, VkImageLayout newImageLayout, uint32_t MipLevel)
{
	VkImageSubresourceRange ImageSubresourceRange{};
	ImageSubresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
	ImageSubresourceRange.baseMipLevel = MipLevel;
	ImageSubresourceRange.levelCount = VK_REMAINING_MIP_LEVELS;
	ImageSubresourceRange.layerCount = 1;

	VkImageMemoryBarrier barrier = {};
	barrier.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
	barrier.oldLayout = TextureImageLayout;
	barrier.newLayout = newImageLayout;
	barrier.image = Image;
	barrier.subresourceRange = ImageSubresourceRange;
	barrier.srcAccessMask = 0;
	barrier.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;

	vkCmdPipelineBarrier(commandBuffer, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, nullptr, 0, nullptr, 1, &barrier);
	TextureImageLayout = newImageLayout;
}


VkResult Texture::NewTextureImage()
{
	return Texture_NewTextureImage(cRenderer.Device, cRenderer.PhysicalDevice, &Image, &Memory, Width, Height, 1, TextureByteFormat);
}

VkResult Texture::CreateTextureImage(VkImageCreateInfo& imageCreateInfo)
{
	return Texture_BaseCreateTextureImage(cRenderer.Device, cRenderer.PhysicalDevice, &Image, &Memory, imageCreateInfo);
}

VkResult Texture::CreateTextureImage()
{
	return Texture_CreateTextureImage(cRenderer.Device, cRenderer.PhysicalDevice, &Image, &Memory, Width, Height, MipMapLevels, TextureByteFormat);
}

VkResult Texture::TransitionImageLayout(VkImageLayout newLayout)
{
	return Texture_QuickTransitionImageLayout(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, Image, MipMapLevels, &TextureImageLayout, &newLayout);
}

VkResult Texture::TransitionImageLayout(VkImageLayout oldLayout, VkImageLayout newLayout)
{
	return Texture_QuickTransitionImageLayout(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, Image, MipMapLevels, &oldLayout, &newLayout);
}

VkResult Texture::TransitionImageLayout(VkCommandBuffer commandBuffer, VkImageLayout newLayout)
{
	return Texture_CommandBufferTransitionImageLayout(commandBuffer, Image, MipMapLevels, TextureImageLayout, newLayout);
}

VkResult Texture::TransitionImageLayout(VkCommandBuffer commandBuffer, VkImageLayout oldLayout, VkImageLayout newLayout)
{
	return Texture_CommandBufferTransitionImageLayout(commandBuffer, Image, MipMapLevels, oldLayout, newLayout);
}

VkResult Texture::CopyBufferToTexture(VkBuffer buffer)
{
	return Texture_CopyBufferToTexture(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, Image, buffer, TextureUsage, Width, Height, Depth);
}

VkResult Texture::GenerateMipmaps()
{
	return Texture_GenerateMipmaps(cRenderer.Device, cRenderer.PhysicalDevice, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, Image, &TextureByteFormat, MipMapLevels, Width, Height);
}

VkResult Texture::CreateTextureView()
{
	return Texture_CreateTextureView(cRenderer.Device, &View, Image, TextureByteFormat, MipMapLevels);
}

VkResult Texture::CreateTextureSampler(VkSamplerCreateInfo samplerCreateInfo)
{
	return Texture_CreateTextureSampler(cRenderer.Device, &samplerCreateInfo, &Sampler);
}