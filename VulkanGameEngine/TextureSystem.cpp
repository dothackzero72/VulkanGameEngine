#include "TextureSystem.h"

TextureSystem textureSystem = TextureSystem();

VkGuid TextureSystem::LoadTexture(const String& texturePath)
{
    if (texturePath.empty() ||
        texturePath == "")
    {
        return VkGuid();
    }

    nlohmann::json json = Json::ReadJson(texturePath);
    VkGuid textureId = VkGuid(json["TextureId"].get<String>().c_str());

    auto it = textureSystem.TextureList.find(textureId);
    if (it != textureSystem.TextureList.end())
    {
        return textureId;
    }

	TextureList[textureId] = Texture_LoadTexture(cRenderer.Device, cRenderer.PhysicalDevice, cRenderer.CommandPool, cRenderer.GraphicsQueue, texturePath);
    return textureId;
}

Texture TextureSystem::CreateTexture(VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo)
{
	return Texture_CreateTexture(cRenderer.Device, cRenderer.PhysicalDevice, cRenderer.CommandPool, cRenderer.GraphicsQueue, imageType, createImageInfo, samplerCreateInfo);
}

Texture TextureSystem::CreateTexture(const String& texturePath, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps)
{
	return Texture_CreateTexture(cRenderer.Device, cRenderer.PhysicalDevice, cRenderer.CommandPool, cRenderer.GraphicsQueue, texturePath, imageType, createImageInfo, samplerCreateInfo, useMipMaps);
}

Texture TextureSystem::CreateTexture(Pixel& clearColor, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps)
{
	return Texture_CreateTexture(cRenderer.Device, cRenderer.PhysicalDevice, cRenderer.CommandPool, cRenderer.GraphicsQueue, clearColor, imageType, createImageInfo, samplerCreateInfo, useMipMaps);
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
