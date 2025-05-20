#pragma once
extern "C"
{
    #include "CVulkanRenderer.h"
}

#include "pixel.h"
#include "Typedef.h"
#include "VkGuid.h"

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
    VkGuid TextureId;
    int Width = 1;
    int Height = 1;
    int Depth = 1;
    uint32 MipMapLevels = 1;
    uint32 TextureBufferIndex = 0;

    VkImage Image = VK_NULL_HANDLE;
    VkDeviceMemory Memory = VK_NULL_HANDLE;
    VkImageView View = VK_NULL_HANDLE;
    VkSampler Sampler = VK_NULL_HANDLE;
    VkDescriptorSet ImGuiDescriptorSet = VK_NULL_HANDLE;

    TextureUsageEnum TextureUsage = TextureUsageEnum::kUse_Undefined;
    TextureTypeEnum TextureType = TextureTypeEnum::kType_UndefinedTexture;
    VkFormat TextureByteFormat = VK_FORMAT_UNDEFINED;
    VkImageLayout TextureImageLayout = VK_IMAGE_LAYOUT_UNDEFINED;
    VkSampleCountFlagBits SampleCount = VK_SAMPLE_COUNT_1_BIT;
    ColorChannelUsed ColorChannels = ColorChannelUsed::ChannelRGBA;
};

DLL_EXPORT TextureStruct Texture_CreateTexture(VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo);

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
    DLL_EXPORT void Texture_UploadTexture(VkDevice device,
        VkPhysicalDevice physicalDevice,
        VkCommandPool commandPool,
        VkQueue graphicsQueue,
        int* width,
        int* height,
        int* depth,
        VkFormat* textureByteFormat,
        uint* mipmapLevels,
        void* textureData,
        VkImage* textureImage,
        VkDeviceMemory* textureMemory,
        VkImageLayout* textureImageLayout,
        enum ColorChannelUsed* colorChannelUsed,
        const char* filePath);

#ifdef __cplusplus
}
#endif