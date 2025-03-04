#include "Texture.h"
#include <pixel.h>
#include <TextureCPP.h>

#ifdef max 
#undef max
#endif
#include "MemoryManager.h"

uint32 Texture::NextTextureId = 0;

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
	TextureBufferIndex = 0;

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
	TextureBufferIndex = 0;

	CreateImageTexture(filePath);
	CreateTextureView();
	CreateTextureSampler();
	ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

Texture::~Texture()
{

}

void Texture::TextureSetUp()
{
	Name = "Texture";
	TextureId = ++NextTextureId;
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
							   clearColor);

	Memory = *textureMemory;
	TextureByteFormat = *textureByteFormat;
	Image = *textureImage;
	TextureImageLayout = *textureImageLayout;
	ColorChannels = *colorChannelUsed;
}

void Texture::CreateImageTexture(const String& filePath)
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
							   filePath);

	Memory = *textureMemory;
	TextureByteFormat = *textureByteFormat;
	Image = *textureImage;
	TextureImageLayout = *textureImageLayout;
	ColorChannels = *colorChannelUsed;
}

void Texture::CreateTextureSampler()
{
	VkSamplerCreateInfo TextureImageSamplerInfo = 
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
		.maxLod = 0.0f,
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