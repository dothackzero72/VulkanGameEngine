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
#include <objbase.h>
#include "VkGuid.h"

enum ExportTextureFormat
{
	Export_BMP,
	Export_JPG,
	Export_PNG,
	Export_TGA
};

class Texture
{
private:
	uint32 TextureBufferIndex = 0;

protected:
	VkResult CreateImage(VkImageCreateInfo& imageCreateInfo);
	virtual void CreateImageTexture(const Pixel& clearColor, bool useMipMaps);
	virtual void CreateImageTexture(const String& FilePath, bool useMipMaps);
	virtual void CreateTextureSampler();
	VkResult CreateTextureSampler(VkSamplerCreateInfo samplerCreateInfo);
	VkResult CopyBufferToTexture(VkBuffer buffer);

public:
	String Name = "Texture";
	VkGuid TextureId;
	int Width = 1;
	int Height = 1;
	int Depth = 1;
	ColorChannelUsed ColorChannels = ColorChannelUsed::ChannelRGBA;
	uint32 MipMapLevels = 1;

	TextureUsageEnum TextureUsage = TextureUsageEnum::kUse_Undefined;
	TextureTypeEnum TextureType = TextureTypeEnum::kType_UndefinedTexture;
	VkFormat TextureByteFormat = VK_FORMAT_UNDEFINED;
	VkImageLayout TextureImageLayout = VK_IMAGE_LAYOUT_UNDEFINED;
	VkSampleCountFlagBits SampleCount = VK_SAMPLE_COUNT_1_BIT;

	VkDescriptorSet ImGuiDescriptorSet = VK_NULL_HANDLE;
	VkImage Image = VK_NULL_HANDLE;
	VkDeviceMemory Memory = VK_NULL_HANDLE;
	VkImageView View = VK_NULL_HANDLE;
	VkSampler Sampler = VK_NULL_HANDLE;

	Texture();
	Texture(VkGuid textureID, Pixel clearColor, int width, int height, VkFormat textureByteFormat, VkImageAspectFlags imageType, TextureTypeEnum textureType, bool useMipMaps);
	Texture(VkGuid textureID, const String& filePath, VkFormat textureByteFormat, VkImageAspectFlags imageType, TextureTypeEnum TextureType, bool useMipMaps);
	virtual ~Texture();

	void UpdateTextureBufferIndex(uint64_t bufferIndex);
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
	void SaveTexture(const char* filename, ExportTextureFormat textureFormat);
	//void ImGuiShowTexture(const ImVec2& TextureDisplaySize);
	virtual void Destroy();

	const VkFormat GetTextureByteFormat() { return TextureByteFormat; }
	const VkSampleCountFlagBits GetSampleCount() { return SampleCount; }
	const uint32 GetTextureBufferIndex() { return TextureBufferIndex; }
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
	const Pixel& clearColor,
	bool usingMipMap);

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
	const String& filePath,
	bool usingMipMap);

void Texture_SaveTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, const char* filename, SharedPtr<Texture> texture, ExportTextureFormat textureFormat, uint32 channels);