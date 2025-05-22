#pragma once
extern "C"
{
    #include "CVulkanRenderer.h"
}

#include "pixel.h"
#include "Typedef.h"
#include "VkGuid.h"
#include "json.h"

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

DLL_EXPORT Texture Texture_CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue queue, const String& jsonPath);
DLL_EXPORT Texture Texture_CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue queue, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo);
DLL_EXPORT Texture Texture_CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue queue, const String& texturePath, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps);
DLL_EXPORT Texture Texture_CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue queue, Pixel& clearColor, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps);
DLL_EXPORT void Texture_UpdateTextureBufferIndex(Texture& texture, uint32 bufferIndex);
DLL_EXPORT void Texture_UpdateTextureSize(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, Texture& texture, VkImageAspectFlags imageType, vec2& TextureResolution);
DLL_EXPORT void Texture_GetTexturePropertiesBuffer(Texture& texture, std::vector<VkDescriptorImageInfo>& textureDescriptorList);
DLL_EXPORT void Texture_DestroyTexture(VkDevice device, Texture& texture);

DLL_EXPORT void Texture_CreateTextureImage(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, Texture& texture, const Pixel& clearColor);
DLL_EXPORT void Texture_CreateTextureImage(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, Texture& texture, const String& filePath);
DLL_EXPORT void Texture_CreateTextureImage(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, Texture& texture, VkImageCreateInfo& createImageInfo);

#ifdef __cplusplus
extern "C" {
#endif
    DLL_EXPORT VkResult Texture_UpdateImage(VkDevice device, VkPhysicalDevice physicalDevice, Texture& texture);
    DLL_EXPORT VkResult Texture_CreateImage(VkDevice device, VkPhysicalDevice physicalDevice, Texture& texture, VkImageCreateInfo& imageCreateInfo);
    DLL_EXPORT VkResult Texture_CreateTextureView(VkDevice device, Texture& texture, VkImageAspectFlags imageAspectFlags);
    DLL_EXPORT VkResult Texture_CreateSpriteTextureSampler(VkDevice device, VkSampler& smapler);
    DLL_EXPORT VkResult Texture_CreateRenderedTextureSampler(VkDevice device, VkSampler& smapler);
    DLL_EXPORT VkResult Texture_CreateDepthTextureSampler(VkDevice device, VkSampler& smapler);

    DLL_EXPORT VkResult Texture_TransitionImageLayout(VkCommandBuffer commandBuffer, Texture& texture, VkImageLayout newLayout);
    DLL_EXPORT VkResult Texture_QuickTransitionImageLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, Texture& texture, VkImageLayout& newLayout);
    DLL_EXPORT VkResult Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, Texture& texture, VkImageLayout newLayout);

    DLL_EXPORT void Texture_UpdateCmdTextureLayout(VkCommandBuffer& commandBuffer, Texture& texture, VkImageLayout& newImageLayout);
    DLL_EXPORT void Texture_UpdateTextureLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, Texture& texture, VkImageLayout& newImageLayout);

    DLL_EXPORT VkResult Texture_CopyBufferToTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, Texture& texture, VkBuffer buffer);
    DLL_EXPORT VkResult Texture_GenerateMipmaps(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, Texture& texture);

#ifdef __cplusplus
}
#endif