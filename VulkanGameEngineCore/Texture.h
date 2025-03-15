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
#include "VulkanBuffer.h"

class Texture
{
private:
	static uint32 NextTextureId;
	uint32 TextureId = 0;
	uint32 TextureBufferIndex;

	void	 TextureSetUp();
	VkResult GenerateMipmaps();

protected:
	VkResult CreateImage(VkImageCreateInfo& imageCreateInfo);
	virtual void CreateImageTexture(const Pixel& clearColor);
	virtual void CreateImageTexture(const String& FilePath);
	virtual void CreateTextureSampler();
	VkResult CopyBufferToTexture(VkBuffer buffer);
	virtual VkResult CreateTextureView();
	VkResult CreateTextureSampler(VkSamplerCreateInfo samplerCreateInfo);

public:
	String Name;
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
	void UpdateTextureBufferIndex(uint64_t bufferIndex);
	virtual void Destroy();

	VkResult TransitionImageLayout(VkImageLayout newLayout);
	VkResult TransitionImageLayout(VkImageLayout& oldLayout, VkImageLayout newLayout);
	VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, VkImageLayout newLayout);
	VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, VkImageLayout& oldLayout, VkImageLayout newLayout);

	void UpdateTextureLayout(VkImageLayout newImageLayout);
	void UpdateTextureLayout(VkImageLayout newImageLayout, uint32_t mipLevel);
	void UpdateTextureLayout(VkCommandBuffer& commandBuffer, VkImageLayout newImageLayout);
	void UpdateTextureLayout(VkCommandBuffer& commandBuffer, VkImageLayout newImageLayout, uint32_t mipLevel);
	void UpdateTextureLayout(VkCommandBuffer& commandBuffer, VkImageLayout oldImageLayout, VkImageLayout newImageLayout);
	void UpdateTextureLayout(VkCommandBuffer& commandBuffer, VkImageLayout oldImageLayout, VkImageLayout newImageLayout, uint32_t mipLevel);

	void GetTexturePropertiesBuffer(std::vector<VkDescriptorImageInfo>& textureDescriptorList);
	//void ImGuiShowTexture(const ImVec2& TextureDisplaySize);

	const VkFormat GetTextureByteFormat() { return TextureByteFormat; }
	const VkSampleCountFlagBits GetSampleCount() { return SampleCount; }
	const uint64 GetTextureBufferIndex() { return TextureBufferIndex; }
};

void Texture_CreateImageTexture(VkDevice device,
	VkPhysicalDevice physicalDevice,
	VkCommandPool commandPool,
	VkQueue graphicsQueue,
	int* width,
	int* height,
	int* depth,
	VkFormat textureByteFormat,
	uint mipmapLevels,
	VkImage* textureImage,
	VkDeviceMemory* textureMemory,
	VkImageLayout* textureImageLayout,
	enum ColorChannelUsed* colorChannelUsed,
	enum TextureUsageEnum textureUsage,
	const Pixel& clearColor);

void Texture_CreateImageTexture(VkDevice device,
	VkPhysicalDevice physicalDevice,
	VkCommandPool commandPool,
	VkQueue graphicsQueue,
	int* width,
	int* height,
	int* depth,
	VkFormat textureByteFormat,
	uint mipmapLevels,
	VkImage* textureImage,
	VkDeviceMemory* textureMemory,
	VkImageLayout* textureImageLayout,
	enum ColorChannelUsed* colorChannelUsed,
	enum TextureUsageEnum textureUsage,
	const String& filePath);