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

struct TextureStruct
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

DLL_EXPORT TextureStruct Texture_CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue queue, const String& jsonPath);
DLL_EXPORT TextureStruct Texture_CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue queue, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo);
DLL_EXPORT TextureStruct Texture_CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue queue, const String& texturePath, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps);
DLL_EXPORT TextureStruct Texture_CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue queue, Pixel& clearColor, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps);
DLL_EXPORT void Texture_UpdateTextureBufferIndex(TextureStruct& texture, uint32 bufferIndex);
DLL_EXPORT void Texture_GetTexturePropertiesBuffer(TextureStruct& texture, std::vector<VkDescriptorImageInfo>& textureDescriptorList);
DLL_EXPORT void Texture_DestroyTexture(VkDevice device, TextureStruct& texture);



DLL_EXPORT void Texture_CreateTextureImage2(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, TextureStruct& texture, const Pixel& clearColor);
DLL_EXPORT void Texture_CreateTextureImage2(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, TextureStruct& texture, const String& filePath);
DLL_EXPORT void Texture_CreateTextureImage2(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, TextureStruct& texture, VkImageCreateInfo& createImageInfo);

DLL_EXPORT void Texture_CreateImageTexture(VkDevice device,
    VkPhysicalDevice physicalDevice,
    VkCommandPool commandPool,
    VkQueue graphicsQueue,
    int* width,
    int* height,
    int* depth,
    VkFormat textureByteFormat,
    uint mipmapLevels,
    VkImage* textureImage,
    VkDeviceMemory* textureMemory,
    VkImageLayout* textureImageLayout,
    enum ColorChannelUsed* colorChannelUsed,
    enum TextureUsageEnum textureUsage,
    const Pixel& clearColor,
    bool usingMipMap);

DLL_EXPORT void Texture_CreateImageTexture(VkDevice device,
    VkPhysicalDevice physicalDevice,
    VkCommandPool commandPool,
    VkQueue graphicsQueue,
    int* width,
    int* height,
    int* depth,
    VkFormat textureByteFormat,
    uint mipmapLevels,
    VkImage* textureImage,
    VkDeviceMemory* textureMemory,
    VkImageLayout* textureImageLayout,
    enum ColorChannelUsed* colorChannelUsed,
    enum TextureUsageEnum textureUsage,
    const String& filePath,
    bool usingMipMap);

#ifdef __cplusplus
extern "C" {
#endif
    DLL_EXPORT VkResult Texture_TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage* image, uint32 mipmapLevels, VkImageLayout* oldLayout, VkImageLayout newLayout);
    DLL_EXPORT VkResult Texture_CreateImage(VkDevice Device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, VkImageCreateInfo imageCreateInfo);
    DLL_EXPORT VkResult Texture_QuickTransitionImageLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, uint32 mipmapLevels, VkImageLayout* oldLayout, VkImageLayout* newLayout);
    DLL_EXPORT VkResult Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint32 mipmapLevels, VkImageLayout* oldLayout, VkImageLayout newLayout);
    DLL_EXPORT VkResult Texture_CopyBufferToTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkBuffer buffer, enum TextureUsageEnum textureType, uint width, uint height, uint depth);
    DLL_EXPORT VkResult Texture_GenerateMipmaps(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkFormat* textureByteFormat, uint32 mipmapLevels, int width, int height, bool usingMipMaps);
    DLL_EXPORT VkResult Texture_CreateTextureView(VkDevice device, VkImageView* view, VkImage image, VkFormat format, VkImageAspectFlags imageType, uint32 mipmapLevels);
    DLL_EXPORT VkResult Texture_CreateTextureSampler(VkDevice device, VkSamplerCreateInfo* samplerCreateInfo, VkSampler* smapler);

    DLL_EXPORT void Texture_UpdateTextureLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkImageLayout* oldImageLayout, VkImageLayout* newImageLayout, uint32 mipLevel);
    DLL_EXPORT void Texture_UpdateCmdTextureLayout(VkCommandBuffer* commandBuffer, VkImage image, VkImageLayout* oldImageLayout, VkImageLayout* newImageLayout, uint32 mipLevel);


    DLL_EXPORT VkResult Texture_CreateImage2(VkDevice device, VkPhysicalDevice physicalDevice, TextureStruct& texture, VkImageCreateInfo& imageCreateInfo);
    DLL_EXPORT VkResult Texture_CreateTextureView2(VkDevice device, TextureStruct& texture, VkImageAspectFlags imageAspectFlags);
    DLL_EXPORT VkResult Texture_CreateSpriteTextureSampler2(VkDevice device, VkSampler& smapler);
    DLL_EXPORT VkResult Texture_CreateRenderedTextureSampler2(VkDevice device, VkSampler& smapler);
    DLL_EXPORT VkResult Texture_CreateDepthTextureSampler2(VkDevice device, VkSampler& smapler);

    DLL_EXPORT VkResult Texture_TransitionImageLayout2(VkCommandBuffer commandBuffer, TextureStruct& texture, VkImageLayout newLayout);
    DLL_EXPORT VkResult Texture_QuickTransitionImageLayout2(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, TextureStruct& texture, VkImageLayout& newLayout);
    DLL_EXPORT VkResult Texture_CommandBufferTransitionImageLayout2(VkCommandBuffer commandBuffer, TextureStruct& texture, VkImageLayout newLayout);

    DLL_EXPORT void Texture_UpdateCmdTextureLayout2(VkCommandBuffer& commandBuffer, TextureStruct& texture, VkImageLayout& newImageLayout);
    DLL_EXPORT void Texture_UpdateTextureLayout2(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, TextureStruct& texture, VkImageLayout& newImageLayout);

    DLL_EXPORT VkResult Texture_CopyBufferToTexture2(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, TextureStruct& texture, VkBuffer buffer);
    DLL_EXPORT VkResult Texture_GenerateMipmaps2(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, TextureStruct& texture);

#ifdef __cplusplus
}
#endif