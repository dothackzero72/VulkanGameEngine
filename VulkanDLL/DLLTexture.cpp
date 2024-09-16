#include "DLLTexture.h"

VkResult DLL_Texture_TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint32 mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout)
{
	return Texture_TransitionImageLayout(commandBuffer, image, mipmapLevels, oldLayout, newLayout);
}

VkResult DLL_Texture_CreateTextureImage(VkDevice Device, VkImage* image, VkDeviceMemory memory, int width, int height, uint32 mipmapLevels, VkFormat textureByteFormat)
{
	return DLL_Texture_CreateTextureImage( Device, image,  memory,  width,  height,  mipmapLevels,  textureByteFormat);
}

VkResult DLL_Texture_QuickTransitionImageLayout(VkImage image, uint32 mipmapLevels, VkImageLayout* oldLayout, VkImageLayout* newLayout)
{
	return DLL_Texture_QuickTransitionImageLayout( image,  mipmapLevels,  oldLayout,   newLayout);
}

VkResult DLL_Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint32 mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout)
{
	return DLL_Texture_CommandBufferTransitionImageLayout( commandBuffer,  image,  mipmapLevels,  oldLayout,  newLayout);
}

VkResult DLL_Texture_CopyBufferToTexture(VkImage image, VkBuffer buffer, enum TextureUsageEnum textureType, int width, int height, int depth)
{
	return DLL_Texture_CopyBufferToTexture( image,  buffer, textureType,  width,  height,  depth);
}

VkResult DLL_Texture_GenerateMipmaps(VkPhysicalDevice physicalDevice, VkImage image, VkFormat* textureByteFormat, uint32 mipmapLevels, int width, int height)
{
	return DLL_Texture_GenerateMipmaps(physicalDevice, image, textureByteFormat, mipmapLevels, width, height);
}

VkResult DLL_Texture_CreateTextureView(VkDevice device, VkImageView* view, VkImage image, VkFormat format, uint32 mipmapLevels)
{
	return DLL_Texture_CreateTextureView(device, view, image, format, mipmapLevels);
}

VkResult DLL_Texture_CreateTextureSampler(VkDevice device, VkSamplerCreateInfo* samplerCreateInfo, VkSampler* smapler)
{
	return DLL_Texture_CreateTextureSampler(device, samplerCreateInfo, smapler);
}