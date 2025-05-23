#pragma once
extern "C"
{
#include "CVulkanRenderer.h"
}
#include "pixel.h"
#include "Typedef.h"
#include "VkGuid.h"
#include "json.h"
#include "CoreVulkanRenderer.h"
#include "VulkanBufferFuncs.h"

typedef enum ColorChannelUsed
{
    ChannelR = 1,
    ChannelRG,
    ChannelRGB,
    ChannelRGBA
};

typedef enum TextureUsageEnum
{
    kUse_Undefined,
    kUse_2DImageTexture,
    kUse_2DDataTexture,
    kUse_2DRenderedTexture,
    kUse_2DDepthTexture,
    kUse_3DImageTexture,
    kUse_3DDataTexture,
    kUse_3DRenderedTexture,
    kUse_3DDepthTexture,
    kUse_CubeMapTexture,
    kUse_CubeMapDepthTexture
};

typedef enum TextureTypeEnum
{
    kType_UndefinedTexture,
    kType_TextureAtlas,
    kType_RenderedColorTexture,
    kType_RenderedDepthTexture,
    kType_ReadableTexture,
    kType_DiffuseTextureMap,
    kType_SpecularTextureMap,
    kType_AlbedoTextureMap,
    kType_MetallicTextureMap,
    kType_RoughnessTextureMap,
    kType_AmbientOcclusionTextureMap,
    kType_NormalTextureMap,
    kType_DepthTextureMap,
    kType_AlphaTextureMap,
    kType_EmissionTextureMap,
    kType_PaletteRotationMap,
    kType_CubeMapTexture,
    kType_CubeMapDepthTexture,
    kType_EnvironmentTexture,
    kType_RenderedCubeMap,
    kType_BakedTexture
};

struct Texture
{
    VkGuid textureId;
    int width = 1;
    int height = 1;
    int depth = 1;
    uint32 mipMapLevels = 1;
    uint32 textureBufferIndex = 0;

    VkImage textureImage = VK_NULL_HANDLE;
    VkDeviceMemory textureMemory = VK_NULL_HANDLE;
    VkImageView textureView = VK_NULL_HANDLE;
    VkSampler textureSampler = VK_NULL_HANDLE;
    VkDescriptorSet ImGuiDescriptorSet = VK_NULL_HANDLE;

    TextureUsageEnum textureUsage = TextureUsageEnum::kUse_Undefined;
    TextureTypeEnum textureType = TextureTypeEnum::kType_UndefinedTexture;
    VkFormat textureByteFormat = VK_FORMAT_UNDEFINED;
    VkImageLayout textureImageLayout = VK_IMAGE_LAYOUT_UNDEFINED;
    VkSampleCountFlagBits sampleCount = VK_SAMPLE_COUNT_1_BIT;
    ColorChannelUsed colorChannels = ColorChannelUsed::ChannelRGBA;
};

    DLL_EXPORT Texture Texture_LoadTexture(const RendererState& rendererState, const String& jsonPath);
    DLL_EXPORT Texture Texture_CreateTexture(const RendererState& rendererState, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo);
    DLL_EXPORT Texture Texture_CreateTexture(const RendererState& rendererState, const String& texturePath, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps);
    DLL_EXPORT Texture Texture_CreateTexture(const RendererState& rendererState, Pixel& clearColor, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps);
    DLL_EXPORT void Texture_UpdateTextureSize(const RendererState& rendererState, Texture& texture, VkImageAspectFlags imageType, vec2& TextureResolution);
    DLL_EXPORT void Texture_UpdateTextureBufferIndex(Texture& texture, uint32 bufferIndex);
    DLL_EXPORT void Texture_GetTexturePropertiesBuffer(Texture& texture, Vector<VkDescriptorImageInfo>& textureDescriptorList);
    DLL_EXPORT void Texture_DestroyTexture(const RendererState& rendererState, Texture& texture);

    void Texture_CreateTextureImage(const RendererState& rendererState, Texture & texture, const Pixel & clearColor);
    void Texture_CreateTextureImage(const RendererState& rendererState, Texture & texture, const String & filePath);
    void Texture_CreateTextureImage(const RendererState& rendererState, Texture & texture, VkImageCreateInfo & createImageInfo);


#ifdef __cplusplus
extern "C" {
#endif
    
        DLL_EXPORT void Texture_UpdateCmdTextureLayout(const RendererState& rendererState, VkCommandBuffer& commandBuffer, Texture& texture, VkImageLayout& newImageLayout);
        DLL_EXPORT void Texture_UpdateTextureLayout(const RendererState& rendererState, Texture& texture, VkImageLayout& newImageLayout);

        VkResult Texture_UpdateImage(const RendererState& rendererState, Texture & texture);
        VkResult Texture_CreateImage(const RendererState& rendererState, Texture & texture, VkImageCreateInfo& imageCreateInfo);
        VkResult Texture_CreateTextureView(const RendererState& rendererState, Texture & texture, VkImageAspectFlags imageAspectFlags);
        VkResult Texture_CreateSpriteTextureSampler(const RendererState& rendererState, VkSampler& smapler);
        VkResult Texture_CreateRenderedTextureSampler(const RendererState& rendererState, VkSampler& smapler);
        VkResult Texture_CreateDepthTextureSampler(const RendererState& rendererState, VkSampler& smapler);

        VkResult Texture_TransitionImageLayout(const RendererState& rendererState, VkCommandBuffer& commandBuffer, Texture& texture, VkImageLayout newLayout);
        VkResult Texture_QuickTransitionImageLayout(const RendererState& rendererState, Texture& texture, VkImageLayout& newLayout);
        VkResult Texture_CommandBufferTransitionImageLayout(const RendererState& rendererState, VkCommandBuffer commandBuffer, Texture& texture, VkImageLayout newLayout);

        VkResult Texture_CopyBufferToTexture(const RendererState& rendererState, Texture & texture, VkBuffer buffer);
        VkResult Texture_GenerateMipmaps(const RendererState& rendererState, Texture & texture);
    
#ifdef __cplusplus
}
#endif