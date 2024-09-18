#include "DLLTexture.h"

// Transition the image layout directly
VkResult DLL_Texture_TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage* image, uint32_t mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout)
{
    return Texture_TransitionImageLayout(commandBuffer, image, mipmapLevels, oldLayout, newLayout);
}

// Create a texture image
VkResult DLL_Texture_CreateTextureImage(VkDevice device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, int width, int height, uint32_t mipmapLevels, VkFormat textureByteFormat)
{
    return Texture_CreateTextureImage(device, physicalDevice, image, memory, width, height, mipmapLevels, textureByteFormat);
}

// Quick transition of image layout
VkResult DLL_Texture_QuickTransitionImageLayout(VkImage image, uint32_t mipmapLevels, VkImageLayout* oldLayout, VkImageLayout* newLayout)
{
    return Texture_QuickTransitionImageLayout(image, mipmapLevels, oldLayout, newLayout);
}

// Transition of image layout using a command buffer
VkResult DLL_Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint32_t mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout)
{
    return Texture_CommandBufferTransitionImageLayout(commandBuffer, image, mipmapLevels, oldLayout, newLayout);
}

// Copy buffer to texture
VkResult DLL_Texture_CopyBufferToTexture(VkImage image, VkBuffer buffer, TextureUsageEnum textureType, int width, int height, int depth)
{
    return Texture_CopyBufferToTexture(image, buffer, textureType, width, height, depth);
}

// Generate mipmaps for the texture
VkResult DLL_Texture_GenerateMipmaps(VkPhysicalDevice physicalDevice, VkImage image, VkFormat* textureByteFormat, uint32_t mipmapLevels, int width, int height)
{
    return Texture_GenerateMipmaps(physicalDevice, image, textureByteFormat, mipmapLevels, width, height);
}

// Create a texture view
VkResult DLL_Texture_CreateTextureView(VkDevice device, VkImageView* view, VkImage image, VkFormat format, uint32_t mipmapLevels)
{
    return Texture_CreateTextureView(device, view, image, format, mipmapLevels);
}

// Create a texture sampler
VkResult DLL_Texture_CreateTextureSampler(VkDevice device, VkSamplerCreateInfo* samplerCreateInfo, VkSampler* sampler)
{
    return Texture_CreateTextureSampler(device, samplerCreateInfo, sampler);
}