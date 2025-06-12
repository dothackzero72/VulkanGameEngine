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

    auto it = textureSystem.TextureMap.find(textureId);
    if (it != textureSystem.TextureMap.end())
    {
        return textureId;
    }

    TextureMap[textureId] = Texture_LoadTexture(cRenderer, texturePath.c_str());
    return textureId;
}

void TextureSystem::Update(const float& deltaTime)
{
    int x = 0;
    for (auto& [id, texture] : TextureMap)
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

void TextureSystem::AddRenderedTexture(RenderPassGuid& vkGuid, Vector<Texture>& renderedTextureList)
{
    RenderedTextureListMap[vkGuid] = renderedTextureList;
}

void TextureSystem::AddDepthTexture(RenderPassGuid& vkGuid, Texture& depthTexture)
{
    DepthTextureMap[vkGuid] = depthTexture;
}

void TextureSystem::AddInputTexture(UM_RenderPipelineID& pipelineId, TextureGuid& inputTexture)
{
    InputTextureListMap[pipelineId].emplace_back(inputTexture);
}

Texture& TextureSystem::FindTexture(const RenderPassGuid& guid)
{
    auto it = TextureMap.find(guid);
    if (it != TextureMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("TextureMap not found for given GUID");
}

Texture& TextureSystem::FindDepthTexture(const RenderPassGuid& guid)
{
    auto it = DepthTextureMap.find(guid);
    if (it != DepthTextureMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("DepthTextureMap not found for given GUID");
}

Texture& TextureSystem::FindRenderedTexture(const RenderPassGuid& guid, const TextureGuid& textureGuid)
{
    auto it = RenderedTextureListMap.find(guid);
    if (it != RenderedTextureListMap.end())
    {
        for (auto& texture : it->second)
        {
            return texture;
        }
    }
    throw std::out_of_range("RenderedTexture not found for given GUID");
}

Vector<Texture>& TextureSystem::FindRenderedTextureList(const RenderPassGuid& guid)
{
    auto it = RenderedTextureListMap.find(guid);
    if (it != RenderedTextureListMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("RenderedTextureList not found for given GUID");
}

 Vector<Texture>& TextureSystem::FindInputTextureList(const RenderPassGuid& guid, const UM_RenderPipelineID& pipelineId)
{
    auto it = InputTextureListMap.find(pipelineId);
    if (it != InputTextureListMap.end())
    {
        Vector<TextureGuid> textureGuidList = it->second;
        Vector<Texture> textureList;
        for (auto& textureGuid : textureGuidList)
        {
            textureList.emplace_back(FindRenderedTexture(guid, textureGuid));
        }
        return textureList;
    }
    throw std::out_of_range("InputTextureList not found for given GUID");
}

const Vector<Texture> TextureSystem::TextureList()
{
    Vector<Texture> textureList;
    for (const auto& texture : TextureMap)
    {
        textureList.emplace_back(texture.second);
    }
    return textureList;
}

const Vector<Texture> TextureSystem::DepthTextureList()
{
    Vector<Texture> depthTextureList;
    for (const auto& depthTextureMesh : DepthTextureMap)
    {
        depthTextureList.emplace_back(depthTextureMesh.second);
    }
    return depthTextureList;
}

const Vector<Texture> TextureSystem::InputTextureList(const RenderPassGuid& guid, const UM_RenderPipelineID& renderPipelineId)
{
    Vector<Texture> textureList;
    const Vector<TextureGuid> inputTextures = InputTextureListMap[renderPipelineId];
    if (!InputTextureListMap[renderPipelineId].empty())
    {
        return textureList;
    }

    auto it = RenderedTextureListMap.find(guid);
    if (it != RenderedTextureListMap.end())
    {
        for (auto& texture : it->second)
        {
            textureList.emplace_back(texture);
        }
    }

    return textureList;
}

void TextureSystem::DestroyAllTextures()
{
    for (auto& texture : TextureMap)
    {
        Texture_DestroyTexture(cRenderer, texture.second);
    }
    for (auto& texture : DepthTextureMap)
    {
        Texture_DestroyTexture(cRenderer, texture.second);
    }
    for (auto& textureList : RenderedTextureListMap)
    {
        for (auto& texture : textureList.second)
        {
            Texture_DestroyTexture(cRenderer, texture);
        }
    }

    TextureMap.clear();
    DepthTextureMap.clear();
    RenderedTextureListMap.clear();
}