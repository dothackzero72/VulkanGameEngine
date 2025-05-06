#include "TextureFunc.h"
#include "CBuffer.h"

#define STB_IMAGE_IMPLEMENTATION
#define STB_IMAGE_WRITE_IMPLEMENTATION
#include <stb/stb_image.h>

#ifdef __cplusplus
extern "C" {
#endif

    void Texture_UploadTexture(VkDevice device,
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
        const char* filePath,
        bool usingMipMaps)
    {
        if (!filePath || !width || !height || !colorChannelUsed) {
            printf("Invalid parameters passed to Texture_UploadTexture\n");
            return;
        }

        VkBufferUsageFlags bufferUsage = VK_BUFFER_USAGE_TRANSFER_SRC_BIT;
        VkMemoryPropertyFlags properties = VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;

        VkBuffer buffer = VK_NULL_HANDLE;
        VkBuffer stagingBuffer = VK_NULL_HANDLE;
        VkDeviceMemory bufferMemory = VK_NULL_HANDLE;
        VkDeviceMemory stagingBufferMemory = VK_NULL_HANDLE;

        Buffer_CreateStagingBuffer(device, physicalDevice, commandPool, graphicsQueue,
            &stagingBuffer, &buffer, &stagingBufferMemory, &bufferMemory,
            textureData, (*width) * (*height) * (*colorChannelUsed), bufferUsage, properties);

        VkImageCreateInfo imageCreateInfo = {
            .sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
            .imageType = VK_IMAGE_TYPE_2D,
            .format = *textureByteFormat,
            .extent = {
                .width = static_cast<uint32_t>(*width),
                .height = static_cast<uint32_t>(*height),
                .depth = 1
            },
            .mipLevels = *mipmapLevels,
            .arrayLayers = 1,
            .samples = VK_SAMPLE_COUNT_1_BIT,
            .tiling = VK_IMAGE_TILING_OPTIMAL,
            .usage = VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
            .sharingMode = VK_SHARING_MODE_EXCLUSIVE,
            .initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
        };

        VkImageLayout newTextureLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
        VULKAN_RESULT(Texture_CreateImage(device, physicalDevice, textureImage, textureMemory, imageCreateInfo));
        VULKAN_RESULT(Texture_QuickTransitionImageLayout(device, commandPool, graphicsQueue, *textureImage, *mipmapLevels, textureImageLayout, &newTextureLayout));
        VULKAN_RESULT(Texture_CopyBufferToTexture(device, commandPool, graphicsQueue, *textureImage, buffer, kUse_2DImageTexture, *width, *height, *depth));
        VULKAN_RESULT(Texture_GenerateMipmaps(device, physicalDevice, commandPool, graphicsQueue, *textureImage, textureByteFormat, *mipmapLevels, *width, *height, usingMipMaps));

        vkDestroyBuffer(device, stagingBuffer, NULL);
        Renderer_FreeDeviceMemory(device, &stagingBufferMemory);
    }



    void Texture_UpdateTextureLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkImageLayout* oldImageLayout, VkImageLayout* newImageLayout, uint32 mipLevel)
    {
        VkImageMemoryBarrier imageMemoryBarrier = {
            .sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
            .srcAccessMask = 0,
            .dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT,
            .oldLayout = *oldImageLayout,
            .newLayout = *newImageLayout,
            .image = image,
            .subresourceRange = {
                .aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                .baseMipLevel = mipLevel,
                .levelCount = VK_REMAINING_MIP_LEVELS,
                .layerCount = 1
            }
        };

        VkCommandBuffer singleCommand = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
        vkCmdPipelineBarrier(singleCommand, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, NULL, 0, NULL, 1, &imageMemoryBarrier);
        VkResult result = Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, singleCommand);
        if (result == VK_SUCCESS) {
            *oldImageLayout = *newImageLayout;
        }
    }

    void Texture_UpdateCmdTextureLayout(VkCommandBuffer* commandBuffer, VkImage image, VkImageLayout oldImageLayout, VkImageLayout* newImageLayout, uint32 mipLevel)
    {
        VkImageMemoryBarrier imageMemoryBarrier = {
            .sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
            .srcAccessMask = 0,
            .dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT,
            .oldLayout = oldImageLayout,
            .newLayout = *newImageLayout,
            .image = image,
            .subresourceRange = {
                .aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                .baseMipLevel = mipLevel,
                .levelCount = VK_REMAINING_MIP_LEVELS,
                .layerCount = 1
            }
        };

        vkCmdPipelineBarrier(*commandBuffer, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, NULL, 0, NULL, 1, &imageMemoryBarrier);
        oldImageLayout = *newImageLayout;
    }

    VkResult Texture_TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage* image, uint32 mipmapLevels, VkImageLayout* oldLayout, VkImageLayout newLayout)
    {
        VkPipelineStageFlags sourceStage = VK_PIPELINE_STAGE_FLAG_BITS_MAX_ENUM;
        VkPipelineStageFlags destinationStage = VK_PIPELINE_STAGE_FLAG_BITS_MAX_ENUM;
        VkImageMemoryBarrier barrier = {
            .sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
            .oldLayout = *oldLayout,
            .newLayout = newLayout,
            .srcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
            .dstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
            .image = *image,
            .subresourceRange = 
            {
                .aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                .baseMipLevel = 0,
                .levelCount = mipmapLevels,
                .baseArrayLayer = 0,
                .layerCount = VK_REMAINING_ARRAY_LAYERS,
            }
        };
        if (*oldLayout == VK_IMAGE_LAYOUT_UNDEFINED && newLayout == VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL) {
            barrier.srcAccessMask = 0;
            barrier.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
            sourceStage = VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT;
            destinationStage = VK_PIPELINE_STAGE_TRANSFER_BIT;
        }
        else if (*oldLayout == VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL && newLayout == VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL) {
            barrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
            barrier.dstAccessMask = VK_ACCESS_SHADER_READ_BIT;
            sourceStage = VK_PIPELINE_STAGE_TRANSFER_BIT;
            destinationStage = VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT;
        }

        vkCmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, 0, 0, NULL, 0, NULL, 1, &barrier);
        *oldLayout = newLayout;
        return VK_SUCCESS;
    }

    VkResult Texture_CreateImage(VkDevice device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, VkImageCreateInfo imageCreateInfo)
    {
        VULKAN_RESULT(vkCreateImage(device, &imageCreateInfo, NULL, image));

        VkMemoryRequirements memRequirements;
        vkGetImageMemoryRequirements(device, *image, &memRequirements);

        VkMemoryAllocateInfo allocInfo = {
            .sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
            .allocationSize = memRequirements.size,
            .memoryTypeIndex = Renderer_GetMemoryType(physicalDevice, memRequirements.memoryTypeBits, VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT)
        };
        VULKAN_RESULT(vkAllocateMemory(device, &allocInfo, NULL, memory));
        return vkBindImageMemory(device, *image, *memory, 0);
    }

    VkResult Texture_QuickTransitionImageLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, uint32 mipmapLevels, VkImageLayout* oldLayout, VkImageLayout* newLayout)
    {
        VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
        Texture_TransitionImageLayout(commandBuffer, &image, mipmapLevels, oldLayout, *newLayout);
        VkResult result = Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, commandBuffer);
        if (result == VK_SUCCESS) {
            *oldLayout = *newLayout;
        }
        return result;
    }

    VkResult Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint32 mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout)
    {
        Texture_TransitionImageLayout(commandBuffer, &image, mipmapLevels, &oldLayout, newLayout);
        return VK_SUCCESS;
    }

    VkResult Texture_CopyBufferToTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkBuffer buffer, enum TextureUsageEnum textureType, int width, int height, int depth)
    {
        VkBufferImageCopy BufferImage = {
            .bufferOffset = 0,
            .bufferRowLength = 0,
            .bufferImageHeight = 0,
            .imageSubresource = {
                .aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                .mipLevel = 0,
                .baseArrayLayer = 0,
                .layerCount = (textureType == kUse_CubeMapTexture ? static_cast<uint>(6) : static_cast<uint>(1))
            },
            .imageOffset = {.x = 0, .y = 0, .z = 0 },
            .imageExtent = {.width = (uint32_t)width, .height = (uint32_t)height, .depth = (uint32_t)depth }
        };
        VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
        vkCmdCopyBufferToImage(commandBuffer, buffer, image, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &BufferImage);
        return Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, commandBuffer);
    }

    VkResult Texture_GenerateMipmaps(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkFormat* textureByteFormat, uint32 mipmapLevels, int width, int height, bool usingMipMaps)
    {
        if (!usingMipMaps) {
            return VK_SUCCESS;
        }

        int32_t mipWidth = width;
        int32_t mipHeight = height;

        VkFormatProperties formatProperties;
        vkGetPhysicalDeviceFormatProperties(physicalDevice, *textureByteFormat, &formatProperties);
        if (!(formatProperties.optimalTilingFeatures & VK_FORMAT_FEATURE_SAMPLED_IMAGE_FILTER_LINEAR_BIT)) {
            RENDERER_ERROR("Texture image format does not support linear blitting.");
        }

        VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
        VkImageMemoryBarrier ImageMemoryBarrier = {
            .sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
            .srcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
            .dstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
            .image = image,
            .subresourceRange
            {
                .aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                .levelCount = 1,
                .baseArrayLayer = 0,
                .layerCount = 1,
            }
        };
        for (uint32_t x = 1; x < mipmapLevels; x++) {
            ImageMemoryBarrier.subresourceRange.baseMipLevel = x - 1;
            ImageMemoryBarrier.oldLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
            ImageMemoryBarrier.newLayout = VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL;
            ImageMemoryBarrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
            ImageMemoryBarrier.dstAccessMask = VK_ACCESS_TRANSFER_READ_BIT;
            vkCmdPipelineBarrier(commandBuffer, VK_PIPELINE_STAGE_TRANSFER_BIT, VK_PIPELINE_STAGE_TRANSFER_BIT, 0, 0, NULL, 0, NULL, 1, &ImageMemoryBarrier);

            VkImageBlit ImageBlit = {
                .srcSubresource
                {
                    .aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                    .mipLevel = x - 1,
                    .baseArrayLayer = 0,
                    .layerCount = 1,
                },
                .srcOffsets
                {
                    VkOffset3D
                    {
                        .x = 0,
                        .y = 0,
                        .z = 0
                    },
                    VkOffset3D
                    {
                        .x = mipWidth,
                        .y = mipHeight,
                        .z = 1
                    }
                },
                .dstSubresource
                {
                    .aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                    .mipLevel = x,
                    .baseArrayLayer = 0,
                    .layerCount = 1,
                },
                .dstOffsets
                {
                    VkOffset3D
                    {
                        .x = 0,
                        .y = 0,
                        .z = 0
                    },
                    VkOffset3D
                    {
                        .x = mipWidth > 1 ? mipWidth / 2 : 1,
                        .y = mipHeight > 1 ? mipHeight / 2 : 1,
                        .z = 1
                    }
                }
            };

            vkCmdBlitImage(commandBuffer, image, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL, image, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &ImageBlit, VK_FILTER_LINEAR);

            ImageMemoryBarrier.oldLayout = VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL;
            ImageMemoryBarrier.newLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
            ImageMemoryBarrier.srcAccessMask = VK_ACCESS_TRANSFER_READ_BIT;
            ImageMemoryBarrier.dstAccessMask = VK_ACCESS_SHADER_READ_BIT;

            vkCmdPipelineBarrier(commandBuffer, VK_PIPELINE_STAGE_TRANSFER_BIT, VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT, 0, 0, NULL, 0, NULL, 1, &ImageMemoryBarrier);

            if (mipWidth > 1) mipWidth /= 2;
            if (mipHeight > 1) mipHeight /= 2;
        }

        ImageMemoryBarrier.subresourceRange.baseMipLevel = mipmapLevels - 1;
        ImageMemoryBarrier.oldLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
        ImageMemoryBarrier.newLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
        ImageMemoryBarrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
        ImageMemoryBarrier.dstAccessMask = VK_ACCESS_SHADER_READ_BIT;

        vkCmdPipelineBarrier(commandBuffer, VK_PIPELINE_STAGE_TRANSFER_BIT, VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT, 0, 0, NULL, 0, NULL, 1, &ImageMemoryBarrier);
        return Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, commandBuffer);
    }

    VkResult Texture_CreateTextureView(VkDevice device, VkImageView* view, VkImage image, VkFormat format, VkImageAspectFlags imageType, uint32 mipmapLevels)
    {
        VkImageSubresourceRange imageSubresourceRange = {
            .aspectMask = imageType,
            .baseMipLevel = 0,
            .levelCount = mipmapLevels,
            .baseArrayLayer = 0,
            .layerCount = 1,
        };

        VkImageViewCreateInfo TextureImageViewInfo = {
            .sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
            .image = image,
            .viewType = VK_IMAGE_VIEW_TYPE_2D,
            .format = format,
            .subresourceRange = imageSubresourceRange,
        };
        return vkCreateImageView(device, &TextureImageViewInfo, NULL, view);
    }

    VkResult Texture_CreateTextureSampler(VkDevice device, VkSamplerCreateInfo* samplerCreateInfo, VkSampler* smapler)
    {
        return vkCreateSampler(device, samplerCreateInfo, NULL, smapler);
    }

#ifdef __cplusplus
}
#endif

// C++-specific function implementations
void Texture_CreateImageTexture(VkDevice device,
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
    bool usingMipMap)
{
    VkBuffer buffer = VK_NULL_HANDLE;
    VkBuffer stagingBuffer = VK_NULL_HANDLE;
    VkDeviceMemory stagingBufferMemory = VK_NULL_HANDLE;
    VkDeviceMemory bufferMemory = VK_NULL_HANDLE;

    VkDeviceSize bufferSize = (*width) * (*height) * (*colorChannelUsed);
    VkMemoryPropertyFlags bufferProperties = VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;
    VkBufferUsageFlags bufferUsage = VK_BUFFER_USAGE_TRANSFER_SRC_BIT;

    std::vector<Pixel> pixels((*width) * (*height), clearColor);
    Buffer_CreateStagingBuffer(device, physicalDevice, commandPool, graphicsQueue, &stagingBuffer, &buffer, &stagingBufferMemory, &bufferMemory, (void*)pixels.data(), bufferSize, bufferUsage, bufferProperties);

    VkImageCreateInfo imageCreateInfo = {
        .sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
        .imageType = VK_IMAGE_TYPE_2D,
        .format = textureByteFormat,
        .extent = {.width = static_cast<uint32_t>(*width), .height = static_cast<uint32_t>(*height), .depth = 1 },
        .mipLevels = mipmapLevels,
        .arrayLayers = 1,
        .samples = VK_SAMPLE_COUNT_1_BIT,
        .tiling = VK_IMAGE_TILING_OPTIMAL,
        .usage = VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
        .sharingMode = VK_SHARING_MODE_EXCLUSIVE,
        .initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
    };

    VkImageLayout newTextureLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
    VULKAN_RESULT(Texture_CreateImage(device, physicalDevice, textureImage, textureMemory, imageCreateInfo));
    VULKAN_RESULT(Texture_QuickTransitionImageLayout(device, commandPool, graphicsQueue, *textureImage, mipmapLevels, textureImageLayout, &newTextureLayout));
    VULKAN_RESULT(Texture_CopyBufferToTexture(device, commandPool, graphicsQueue, *textureImage, buffer, textureUsage, *width, *height, *depth));
    VULKAN_RESULT(Texture_GenerateMipmaps(device, physicalDevice, commandPool, graphicsQueue, *textureImage, &textureByteFormat, mipmapLevels, *width, *height, usingMipMap));

    if (stagingBuffer != VK_NULL_HANDLE) {
        vkDestroyBuffer(device, stagingBuffer, NULL);
        stagingBuffer = VK_NULL_HANDLE;
    }

    if (stagingBufferMemory != VK_NULL_HANDLE) {
        Renderer_FreeDeviceMemory(device, &stagingBufferMemory);
        stagingBufferMemory = VK_NULL_HANDLE;
    }
}

void Texture_CreateImageTexture(VkDevice device,
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
    bool usingMipMap)
{
    VkBuffer buffer = VK_NULL_HANDLE;
    VkBuffer stagingBuffer = VK_NULL_HANDLE;
    VkDeviceMemory stagingBufferMemory = VK_NULL_HANDLE;
    VkDeviceMemory bufferMemory = VK_NULL_HANDLE;

    byte* data = stbi_load(filePath.c_str(), width, height, (int*)colorChannelUsed, 0);

    VkDeviceSize bufferSize = (*width) * (*height) * (*colorChannelUsed);
    VkBufferUsageFlags bufferUsage = VK_BUFFER_USAGE_TRANSFER_SRC_BIT;
    VkMemoryPropertyFlags bufferProperties = VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;

    Buffer_CreateStagingBuffer(device, physicalDevice, commandPool, graphicsQueue, &stagingBuffer, &buffer, &stagingBufferMemory, &bufferMemory, data, bufferSize, bufferUsage, bufferProperties);

    VkImageCreateInfo imageCreateInfo = {
        .sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
        .imageType = VK_IMAGE_TYPE_2D,
        .format = textureByteFormat,
        .extent = {.width = static_cast<uint32_t>(*width), .height = static_cast<uint32_t>(*height), .depth = 1 },
        .mipLevels = mipmapLevels,
        .arrayLayers = 1,
        .samples = VK_SAMPLE_COUNT_1_BIT,
        .tiling = VK_IMAGE_TILING_OPTIMAL,
        .usage = VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
        .sharingMode = VK_SHARING_MODE_EXCLUSIVE,
        .initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
    };

    VkImageLayout newTextureLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
    VULKAN_RESULT(Texture_CreateImage(device, physicalDevice, textureImage, textureMemory, imageCreateInfo));
    VULKAN_RESULT(Texture_QuickTransitionImageLayout(device, commandPool, graphicsQueue, *textureImage, mipmapLevels, textureImageLayout, &newTextureLayout));
    VULKAN_RESULT(Texture_CopyBufferToTexture(device, commandPool, graphicsQueue, *textureImage, buffer, textureUsage, *width, *height, *depth));
    VULKAN_RESULT(Texture_GenerateMipmaps(device, physicalDevice, commandPool, graphicsQueue, *textureImage, &textureByteFormat, mipmapLevels, *width, *height, usingMipMap));

    if (stagingBuffer != VK_NULL_HANDLE) {
        vkDestroyBuffer(device, stagingBuffer, NULL);
        stagingBuffer = VK_NULL_HANDLE;
    }

    if (stagingBufferMemory != VK_NULL_HANDLE) {
        Renderer_FreeDeviceMemory(device, &stagingBufferMemory);
        stagingBufferMemory = VK_NULL_HANDLE;
    }
    stbi_image_free(data);
}

// Note: Texture_SaveTexture is commented out in the original code and requires additional definitions (e.g., SharedPtr, ExportTextureFormat).
// Uncomment and implement it if needed, ensuring all dependencies are resolved.
/*
void Texture_SaveTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, const char* filename, SharedPtr<Texture> texture, ExportTextureFormat textureFormat, uint32 channels)
{
    // Implementation as provided in the original code
}
*/