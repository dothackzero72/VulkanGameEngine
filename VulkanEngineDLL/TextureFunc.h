#pragma once
#include "Typedef.h"

extern "C"
{
#include "CTexture.h"
#include "pixel.h"
}

DLL_EXPORT void Texture_CreateImageTexture(VkDevice device,
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

DLL_EXPORT void Texture_CreateImageTexture(VkDevice device,
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

//void Texture_SaveTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, const char* filename, SharedPtr<Texture> texture, ExportTextureFormat textureFormat, uint32 channels);