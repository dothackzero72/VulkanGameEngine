#include "DLL.h"
#include <CTexture.h>

DLL_EXPORT VkResult DLL_Texture_TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage* image, uint32 mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout);
DLL_EXPORT VkResult DLL_Texture_BaseCreateTextureImage(VkDevice device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, VkImageCreateInfo imageCreateInfo);
DLL_EXPORT VkResult DLL_Texture_NewTextureImage(VkDevice device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, int width, int height, uint32 mipmapLevels, VkFormat textureByteFormat);
DLL_EXPORT VkResult DLL_Texture_CreateTextureImage(VkDevice Device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, int width, int height, uint32 mipmapLevels, VkFormat textureByteFormat);
DLL_EXPORT VkResult DLL_Texture_QuickTransitionImageLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, uint32 mipmapLevels, VkImageLayout* oldLayout, VkImageLayout* newLayout);
DLL_EXPORT VkResult DLL_Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint32 mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout);
DLL_EXPORT VkResult DLL_Texture_CopyBufferToTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkBuffer buffer, enum TextureUsageEnum textureType, int width, int height, int depth);
DLL_EXPORT VkResult DLL_Texture_GenerateMipmaps(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkFormat* textureByteFormat, uint32 mipmapLevels, int width, int height);
DLL_EXPORT VkResult DLL_Texture_CreateTextureView(VkDevice device, VkImageView* view, VkImage image, VkFormat format, uint32 mipmapLevels);
DLL_EXPORT VkResult DLL_Texture_CreateTextureSampler(VkDevice device, VkSamplerCreateInfo* samplerCreateInfo, VkSampler* smapler);