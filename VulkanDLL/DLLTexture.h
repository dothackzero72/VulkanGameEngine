//#pragma once
//#include "DLL.h"
//
//#include <CTexture.h>
//
//
//	DLL_EXPORT VkResult DLL_Texture_TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage* image, uint32_t mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout);
//	DLL_EXPORT VkResult DLL_Texture_CreateTextureImage(VkDevice device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, int width, int height, uint32_t mipmapLevels, VkFormat textureByteFormat);
//	DLL_EXPORT VkResult DLL_Texture_QuickTransitionImageLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, uint32_t mipmapLevels, VkImageLayout* oldLayout, VkImageLayout* newLayout);
//	DLL_EXPORT VkResult DLL_Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint32_t mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout);
//	DLL_EXPORT VkResult DLL_Texture_CopyBufferToTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkBuffer buffer, int textureType, int width, int height, int depth);
//	DLL_EXPORT VkResult DLL_Texture_GenerateMipmaps(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkFormat* textureByteFormat, uint32_t mipmapLevels, int width, int height);
//	DLL_EXPORT VkResult DLL_Texture_CreateTextureView(VkDevice device, VkImageView* view, VkImage image, VkFormat format, uint32_t mipmapLevels);
//	DLL_EXPORT VkResult DLL_Texture_CreateTextureSampler(VkDevice device, VkSamplerCreateInfo* samplerCreateInfo, VkSampler* sampler);
//	DLL_EXPORT void DLL_stbi_write_bmp(const char* filename, int w, int h, int comp, const void* data);
