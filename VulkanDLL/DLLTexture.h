#pragma once
#include "DLL.h"
#include <Texture.h>

extern "C"
{
	DLL_EXPORT VkResult DLL_Texture_TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage* image, uint32 mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout);
	DLL_EXPORT VkResult DLL_Texture_CreateTextureImage(VkDevice Device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, int width, int height, uint32 mipmapLevels, VkFormat textureByteFormat);
	DLL_EXPORT VkResult DLL_Texture_QuickTransitionImageLayout(VkImage image, uint32 mipmapLevels, VkImageLayout* oldLayout, VkImageLayout* newLayout);
	DLL_EXPORT VkResult DLL_Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint32 mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout);
	DLL_EXPORT VkResult DLL_Texture_CopyBufferToTexture(VkImage image, VkBuffer buffer, enum TextureUsageEnum textureType, int width, int height, int depth);
	DLL_EXPORT VkResult DLL_Texture_GenerateMipmaps(VkPhysicalDevice physicalDevice, VkImage image, VkFormat* textureByteFormat, uint32 mipmapLevels, int width, int height);
	DLL_EXPORT VkResult DLL_Texture_CreateTextureView(VkDevice device, VkImageView* view, VkImage image, VkFormat format, uint32 mipmapLevels);
	DLL_EXPORT VkResult DLL_Texture_CreateTextureSampler(VkDevice device, VkSamplerCreateInfo* samplerCreateInfo, VkSampler* smapler);
}