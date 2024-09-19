#include "Texture.h"
#define STB_IMAGE_IMPLEMENTATION
#define STB_IMAGE_WRITE_IMPLEMENTATION
#include <stb/stb_image.h>

#ifdef max 
#undef max
#endif

Texture::Texture()
{
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

Texture::Texture(const std::string& filePath, VkFormat textureByteFormat, TextureTypeEnum textureType)
{
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
	TextureType = textureType;
	TextureByteFormat = textureByteFormat;
	TextureImageLayout = VK_IMAGE_LAYOUT_UNDEFINED;
	SampleCount = VK_SAMPLE_COUNT_1_BIT;

	CreateImageTexture(filePath);
	CreateTextureView();
	CreateTextureSampler();
	ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

Texture::~Texture()
{

}

VkDescriptorImageInfo* Texture::GetTextureBuffer()
{
	textureBuffer = VkDescriptorImageInfo
	{
		.sampler = Sampler,
		.imageView = View,
		.imageLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL
	};
	return &textureBuffer;
}

void Texture::UpdateTextureSize(glm::vec2 TextureResolution)
{
	renderer.DestroyImageView(View);
	renderer.DestroySampler(Sampler);
	renderer.DestroyImage(Image);
	renderer.FreeMemory(Memory);
	ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

void Texture::Destroy()
{
	renderer.DestroyImageView(View);
	renderer.DestroySampler(Sampler);
	renderer.DestroyImage(Image);
	renderer.FreeMemory(Memory);
}

void Texture::CreateImageTexture(const std::string& FilePath)
{
	int* width = &Width;
	int* height = &Height;
	int colorChannels = 0;
	byte* data = stbi_load(FilePath.c_str(), width, height, &colorChannels, 0);
	VulkanBuffer<byte*> buffer(data, Width * Height * colorChannels, VK_BUFFER_USAGE_TRANSFER_SRC_BIT, VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT);

	MipMapLevels = static_cast<uint32>(std::floor(std::log2(std::max(Width, Height)))) + 1;
	VULKAN_RESULT(CreateTextureImage());
	VULKAN_RESULT(TransitionImageLayout(VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL));
	VULKAN_RESULT(CopyBufferToTexture(buffer.Buffer));
	VULKAN_RESULT(GenerateMipmaps());
	buffer.DestroyBuffer();

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

void Texture::ImGuiShowTexture(const ImVec2& TextureDisplaySize)
{
	ImGui::Image(ImGuiDescriptorSet, TextureDisplaySize);
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