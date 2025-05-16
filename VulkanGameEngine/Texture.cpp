#include "Texture.h"
#include "CVulkanRenderer.h"
#include "VulkanError.h"
#include <CBuffer.h>
#include "pixel.h"
#include "VulkanRenderer.h"
#include "CoreVulkanRenderer.h"

#include <stb/stb_image.h>
#include <stb/stb_image_write.h>


#ifdef max 
#undef max
#endif

Texture::Texture()
{

}

Texture::Texture(VkGuid& textureID, Pixel clearColor, int width, int height, VkFormat textureByteFormat, VkImageAspectFlags imageType, TextureTypeEnum textureType, bool useMipMaps)
{
	TextureId = textureID;
	Width = width;
	Height = height;
	TextureType = textureType;
	TextureByteFormat = textureByteFormat;

	if (useMipMaps)
	{
		MipMapLevels = static_cast<uint32>(std::floor(std::log2(std::max(Width, Height)))) + 1;
	}


	CreateImageTexture(clearColor, useMipMaps);
	Texture_CreateTextureView(cRenderer.Device, &View, Image, TextureByteFormat, imageType, MipMapLevels);
	CreateTextureSampler();
	//ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

Texture::Texture(VkGuid& textureID, const String& filePath, VkFormat textureByteFormat, VkImageAspectFlags imageType, TextureTypeEnum textureType, bool useMipMaps)
{
	TextureId = textureID;
	TextureType = textureType;
	TextureByteFormat = textureByteFormat;

	if (useMipMaps)
	{
		MipMapLevels = static_cast<uint32>(std::floor(std::log2(std::max(Width, Height)))) + 1;
	}

	CreateImageTexture(filePath, useMipMaps);
	Texture_CreateTextureView(cRenderer.Device, &View, Image, TextureByteFormat, imageType, MipMapLevels);
	CreateTextureSampler();
	//ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

Texture::~Texture()
{

}

void Texture::CreateImageTexture(const Pixel& clearColor, bool useMipMaps)
{
	int* width = &Width;
	int* height = &Height;
	int* depth = &Depth;
	int colorChannels = 0;
	MipMapLevels = 1;

	VkDeviceMemory* textureMemory = &Memory;
	VkFormat* textureByteFormat = &TextureByteFormat;
	VkImage* textureImage = &Image;
	VkImageLayout* textureImageLayout = &TextureImageLayout;
	ColorChannelUsed* colorChannelUsed = &ColorChannels;
	TextureUsageEnum textureUsage = kUse_2DImageTexture;

	Texture_CreateImageTexture(cRenderer.Device,
		cRenderer.PhysicalDevice,
		cRenderer.CommandPool,
		cRenderer.SwapChain.GraphicsQueue,
		width,
		height,
		depth,
		TextureByteFormat,
		MipMapLevels,
		textureImage,
		textureMemory,
		textureImageLayout,
		colorChannelUsed,
		textureUsage,
		clearColor,
		useMipMaps);

	Memory = *textureMemory;
	TextureByteFormat = *textureByteFormat;
	Image = *textureImage;
	TextureImageLayout = *textureImageLayout;
	ColorChannels = *colorChannelUsed;
}

void Texture::CreateImageTexture(const String& filePath, bool useMipMaps)
{
	int* width = &Width;
	int* height = &Height;
	int* depth = &Depth;
	int colorChannels = 0;
	MipMapLevels = 1;

	VkDeviceMemory* textureMemory = &Memory;
	VkFormat* textureByteFormat = &TextureByteFormat;
	VkImage* textureImage = &Image;
	VkImageLayout* textureImageLayout = &TextureImageLayout;
	ColorChannelUsed* colorChannelUsed = &ColorChannels;
	TextureUsageEnum textureUsage = kUse_2DImageTexture;

	Texture_CreateImageTexture(cRenderer.Device,
		cRenderer.PhysicalDevice,
		cRenderer.CommandPool,
		cRenderer.SwapChain.GraphicsQueue,
		width,
		height,
		depth,
		TextureByteFormat,
		MipMapLevels,
		textureImage,
		textureMemory,
		textureImageLayout,
		colorChannelUsed,
		textureUsage,
		filePath,
		useMipMaps);

	Memory = *textureMemory;
	TextureByteFormat = *textureByteFormat;
	Image = *textureImage;
	TextureImageLayout = *textureImageLayout;
	ColorChannels = *colorChannelUsed;
}

void Texture::CreateTextureSampler()
{
	VkSamplerCreateInfo textureImageSamplerInfo =
	{
		.sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
		.magFilter = VK_FILTER_NEAREST,
		.minFilter = VK_FILTER_NEAREST,
		.mipmapMode = VK_SAMPLER_MIPMAP_MODE_NEAREST,
		.addressModeU = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE,
		.addressModeV = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE,
		.addressModeW = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE,
		.mipLodBias = 0,
		.anisotropyEnable = VK_FALSE,
		.maxAnisotropy = 1.0f,
		.compareEnable = VK_FALSE,
		.compareOp = VK_COMPARE_OP_ALWAYS,
		.minLod = 0,
		.maxLod = (float)MipMapLevels,
		.borderColor = VK_BORDER_COLOR_INT_OPAQUE_BLACK,
		.unnormalizedCoordinates = VK_FALSE,
	};
	VULKAN_RESULT(CreateTextureSampler(textureImageSamplerInfo));
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

//void Texture::ImGuiShowTexture(const ImVec2& TextureDisplaySize)
//{
//	ImGui::Image(ImGuiDescriptorSet, TextureDisplaySize);
//}

void Texture::UpdateTextureLayout(VkImageLayout newImageLayout)
{
	Texture_UpdateTextureLayout(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, Image, &TextureImageLayout, &newImageLayout, MipMapLevels);
}

void Texture::UpdateTextureLayout(VkImageLayout newImageLayout, uint32_t mipLevel)
{
	Texture_UpdateTextureLayout(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, Image, &TextureImageLayout, &newImageLayout, mipLevel);
}

void Texture::UpdateTextureLayout(VkCommandBuffer& commandBuffer, VkImageLayout oldImageLayout, VkImageLayout newImageLayout)
{
	Texture_UpdateCmdTextureLayout(&commandBuffer, Image, oldImageLayout, &newImageLayout, MipMapLevels);
}

void Texture::UpdateTextureLayout(VkCommandBuffer& commandBuffer, VkImageLayout oldImageLayout, VkImageLayout newImageLayout, uint32_t mipLevel)
{
	Texture_UpdateCmdTextureLayout(&commandBuffer, Image, oldImageLayout, &newImageLayout, mipLevel);
}

void Texture::UpdateTextureLayout(VkCommandBuffer& commandBuffer, VkImageLayout newImageLayout)
{
	Texture_UpdateCmdTextureLayout(&commandBuffer, Image, TextureImageLayout, &newImageLayout, MipMapLevels);
}

void Texture::UpdateTextureLayout(VkCommandBuffer& commandBuffer, VkImageLayout newImageLayout, uint32_t mipLevel)
{
	Texture_UpdateCmdTextureLayout(&commandBuffer, Image, TextureImageLayout, &newImageLayout, mipLevel);
}

VkResult Texture::CreateImage(VkImageCreateInfo& imageCreateInfo)
{
	return Texture_CreateImage(cRenderer.Device, cRenderer.PhysicalDevice, &Image, &Memory, imageCreateInfo);
}

VkResult Texture::TransitionImageLayout(VkImageLayout newLayout)
{
	return Texture_QuickTransitionImageLayout(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, Image, MipMapLevels, &TextureImageLayout, &newLayout);
}

VkResult Texture::TransitionImageLayout(VkImageLayout& oldLayout, VkImageLayout newLayout)
{
	return Texture_QuickTransitionImageLayout(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, Image, MipMapLevels, &oldLayout, &newLayout);
}

VkResult Texture::TransitionImageLayout(VkCommandBuffer commandBuffer, VkImageLayout newLayout)
{
	return Texture_CommandBufferTransitionImageLayout(commandBuffer, Image, MipMapLevels, TextureImageLayout, newLayout);
}

VkResult Texture::TransitionImageLayout(VkCommandBuffer commandBuffer, VkImageLayout& oldLayout, VkImageLayout newLayout)
{
	return Texture_CommandBufferTransitionImageLayout(commandBuffer, Image, MipMapLevels, oldLayout, newLayout);
}

VkResult Texture::CopyBufferToTexture(VkBuffer buffer)
{
	return Texture_CopyBufferToTexture(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, Image, buffer, TextureUsage, Width, Height, Depth);
}

VkResult Texture::CreateTextureSampler(VkSamplerCreateInfo samplerCreateInfo)
{
	return Texture_CreateTextureSampler(cRenderer.Device, &samplerCreateInfo, &Sampler);
}


void Texture::SaveTexture(const char* filename, ExportTextureFormat textureFormat)
{
	//return Texture_SaveTexture(cRenderer.Device, cRenderer.CommandPool, cRenderer.SwapChain.GraphicsQueue, filename, SharedPtr<Texture>(this), textureFormat, ColorChannels);
}
