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

    TextureJsonLoader textureJsonLoader = TextureJsonLoader(texturePath);
	TextureList[textureId] = Texture_LoadTexture(cRenderer, textureJsonLoader);
    return textureId;
}

void TextureSystem::Update(const float& deltaTime)
{
    int x = 0;
    for (auto& [id, texture] : TextureList)
    {
        UpdateTextureBufferIndex(texture, x);
        x++;
    }
}

Texture TextureSystem::CreateTexture(VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo)
{
	return Texture_CreateTexture(cRenderer, imageType, createImageInfo, samplerCreateInfo);
}

Texture TextureSystem::CreateTexture(const String& texturePath, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps)
{
	return Texture_CreateTexture(cRenderer, texturePath, imageType, createImageInfo, samplerCreateInfo, useMipMaps);
}

Texture TextureSystem::CreateTexture(Pixel& clearColor, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps)
{
	return Texture_CreateTexture(cRenderer, clearColor, imageType, createImageInfo, samplerCreateInfo, useMipMaps);
}

void TextureSystem::UpdateTextureBufferIndex(Texture& texture, uint32 bufferIndex)
{
	Texture_UpdateTextureBufferIndex(texture, bufferIndex);
}

void TextureSystem::UpdateTextureSize(Texture& texture, VkImageAspectFlags imageType, vec2& TextureResolution)
{
	Texture_UpdateTextureSize(cRenderer, texture, imageType, TextureResolution);
}

void TextureSystem::GetTexturePropertiesBuffer(Texture& texture, Vector<VkDescriptorImageInfo>& textureDescriptorList)
{
	Texture_GetTexturePropertiesBuffer(texture, textureDescriptorList);
}

void TextureSystem::UpdateTextureLayout(Texture& texture, VkCommandBuffer& commandBuffer, VkImageLayout newImageLayout)
{
	Texture_UpdateCmdTextureLayout(cRenderer, commandBuffer, texture, newImageLayout);
}

void TextureSystem::DestroyTexture(Texture& texture)
{
	Texture_DestroyTexture(cRenderer, texture);
}
