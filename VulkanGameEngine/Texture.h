#pragma once
extern "C"
{
#include <CTexture.h>
#include <pixel.h>
}
#include <cmath>
#include <algorithm>
#include <string>
#include "typedef.h"
#include "DynamicVulkanBuffer.h"
#include <Imgui/imgui.h>
#include <Imgui/backends/imgui_impl_vulkan.h>

enum ColorChannelUsed
{
	ChannelR = 1,
	ChannelRG,
	ChannelRGB,
	ChannelRGBA
};

class Texture
{
private:
	uint64 TextureBufferIndex;

	void	 TextureSetUp();
	VkResult GenerateMipmaps();

protected:
	VkResult NewTextureImage();
	VkResult CreateTextureImage();
	VkResult CreateTextureImage(VkImageCreateInfo& imageCreateInfo);
	virtual void CreateImageTexture(const Pixel& clearColor);
	virtual void CreateImageTexture(const String& FilePath);
	virtual void CreateTextureSampler();
	VkResult CopyBufferToTexture(VkBuffer buffer);
	virtual VkResult CreateTextureView();
	VkResult CreateTextureSampler(VkSamplerCreateInfo samplerCreateInfo);

public:
	int Width;
	int Height;
	int Depth;
	ColorChannelUsed ColorChannels;
	uint32 MipMapLevels;

	TextureUsageEnum TextureUsage;
	TextureTypeEnum TextureType;
	VkFormat TextureByteFormat;
	VkImageLayout TextureImageLayout;
	VkSampleCountFlagBits SampleCount;

	VkDescriptorSet ImGuiDescriptorSet;
	VkImage Image;
	VkDeviceMemory Memory;
	VkImageView View;
	VkSampler Sampler;
	VkDescriptorImageInfo textureBuffer;

	Texture();
	Texture(const Pixel& clearColor, int width, int height, VkFormat textureByteFormat, TextureTypeEnum textureType);
	Texture(const String& filePath, VkFormat textureByteFormat, TextureTypeEnum TextureType);
	Texture(VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo);
	virtual ~Texture();
	virtual void UpdateTextureSize(vec2 TextureResolution);
	virtual void Destroy();

	VkResult TransitionImageLayout(VkImageLayout newLayout);
	VkResult TransitionImageLayout(VkImageLayout oldLayout, VkImageLayout newLayout);
	VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, VkImageLayout newLayout);
	VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, VkImageLayout oldLayout, VkImageLayout newLayout);

	void UpdateImageLayout(VkImageLayout newImageLayout);
	void UpdateImageLayout(VkImageLayout newImageLayout, uint32_t MipLevel);
	void UpdateImageLayout(VkCommandBuffer& commandBuffer, VkImageLayout newImageLayout);
	void UpdateImageLayout(VkCommandBuffer& commandBuffer, VkImageLayout newImageLayout, uint32_t MipLevel);
	void UpdateImageLayout(VkCommandBuffer& commandBuffer, VkImageLayout oldImageLayout, VkImageLayout newImageLayout);
	void UpdateImageLayout(VkCommandBuffer& commandBuffer, VkImageLayout oldImageLayout, VkImageLayout newImageLayout, uint32_t MipLevel);

	void ImGuiShowTexture(const ImVec2& TextureDisplaySize);
	VkDescriptorImageInfo* GetTextureBuffer();

	const VkFormat GetTextureByteFormat() { return TextureByteFormat; }
	const VkSampleCountFlagBits GetSampleCount() { return SampleCount; }
};

