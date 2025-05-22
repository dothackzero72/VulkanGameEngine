#include "TextureSystem.h"

TextureSystem textureSystem = TextureSystem();

Texture TextureSystem::CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue queue, const String& jsonPath)
{
	return Texture_CreateTexture(device, physicalDevice, commandPool, queue, jsonPath);
}

Texture TextureSystem::CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue queue, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo)
{
	return Texture_CreateTexture(device, physicalDevice, commandPool, queue, imageType, createImageInfo, samplerCreateInfo);
}

Texture TextureSystem::CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue queue, const String& texturePath, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps)
{
	return Texture_CreateTexture(device, physicalDevice, commandPool, queue, texturePath, imageType, createImageInfo, samplerCreateInfo, useMipMaps);
}

Texture TextureSystem::CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue queue, Pixel& clearColor, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps)
{
	return Texture_CreateTexture(device, physicalDevice, commandPool, queue, clearColor, imageType, createImageInfo, samplerCreateInfo, useMipMaps);
}

void TextureSystem::UpdateTextureBufferIndex(Texture& texture, uint32 bufferIndex)
{
	Texture_UpdateTextureBufferIndex(texture, bufferIndex);
}

void TextureSystem::UpdateTextureSize(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, Texture& texture, VkImageAspectFlags imageType, vec2& TextureResolution)
{
	Texture_UpdateTextureSize( device,  physicalDevice,  commandPool,  graphicsQueue, texture,  imageType, TextureResolution);
}

void TextureSystem::GetTexturePropertiesBuffer(Texture& texture, Vector<VkDescriptorImageInfo>& textureDescriptorList)
{
	Texture_GetTexturePropertiesBuffer(texture, textureDescriptorList);
}

void TextureSystem::UpdateTextureLayout(Texture& texture, VkCommandBuffer& commandBuffer, VkImageLayout newImageLayout)
{
	Texture_UpdateCmdTextureLayout(commandBuffer, texture, newImageLayout);
}

void TextureSystem::DestroyTexture(VkDevice device, Texture& texture)
{
	Texture_DestroyTexture(device, texture);
}
