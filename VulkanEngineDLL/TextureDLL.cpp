#include "TextureDLL.h"

	void DLL_Texture_UpdateTextureLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkImageLayout* oldImageLayout, VkImageLayout* newImageLayout, uint32 mipLevel)
	{
		return Texture_UpdateTextureLayout(device, commandPool, graphicsQueue, image, oldImageLayout, newImageLayout, mipLevel);
	}

	void DLL_Texture_UpdateCmdTextureLayout(VkCommandBuffer* commandBuffer, VkImage image, VkImageLayout oldImageLayout, VkImageLayout* newImageLayout, uint32 mipLevel)
	{
		return Texture_UpdateCmdTextureLayout(commandBuffer, image, oldImageLayout, newImageLayout, mipLevel);
	}

	VkResult DLL_Texture_TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage* image, uint32 mipmapLevels, VkImageLayout* oldLayout, VkImageLayout newLayout)
	{
		return Texture_TransitionImageLayout(commandBuffer, image, mipmapLevels, oldLayout, newLayout);
	}

	VkResult DLL_Texture_BaseCreateTextureImage(VkDevice device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, VkImageCreateInfo imageCreateInfo)
	{
		return Texture_BaseCreateTextureImage(device, physicalDevice, image, memory, imageCreateInfo);
	}

	VkResult DLL_Texture_NewTextureImage(VkDevice device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, int width, int height, uint32 mipmapLevels, VkFormat textureByteFormat)
	{
		return Texture_NewTextureImage(device, physicalDevice, image, memory, width, height, mipmapLevels, textureByteFormat);
	}

	VkResult DLL_Texture_CreateTextureImage(VkDevice Device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, int width, int height, uint32 mipmapLevels, VkFormat textureByteFormat)
	{
		return Texture_CreateTextureImage(Device, physicalDevice, image, memory, width, height, mipmapLevels, textureByteFormat);
	}

	VkResult DLL_Texture_QuickTransitionImageLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, uint32 mipmapLevels, VkImageLayout* oldLayout, VkImageLayout* newLayout)
	{
		return Texture_QuickTransitionImageLayout(device, commandPool, graphicsQueue, image, mipmapLevels, oldLayout, newLayout);
	}

	VkResult DLL_Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint32 mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout)
	{
		return Texture_CommandBufferTransitionImageLayout(commandBuffer, image, mipmapLevels, oldLayout, newLayout);
	}

	VkResult DLL_Texture_CopyBufferToTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkBuffer buffer, enum TextureUsageEnum textureType, int width, int height, int depth)
	{
		return Texture_CopyBufferToTexture(device, commandPool, graphicsQueue, image, buffer, textureType, width, height, depth);
	}

	VkResult DLL_Texture_GenerateMipmaps(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkFormat* textureByteFormat, uint32 mipmapLevels, int width, int height)
	{
		return Texture_GenerateMipmaps(device, physicalDevice, commandPool, graphicsQueue, image, textureByteFormat, mipmapLevels, width, height);
	}

	VkResult DLL_Texture_CreateTextureView(VkDevice device, VkImageView* view, VkImage image, VkFormat format, uint32 mipmapLevels)
	{
		return Texture_CreateTextureView(device, view, image, format, mipmapLevels);
	}

	VkResult DLL_Texture_CreateTextureSampler(VkDevice device, VkSamplerCreateInfo* samplerCreateInfo, VkSampler* smapler)
	{
		return Texture_CreateTextureSampler(device, samplerCreateInfo, smapler);
	}