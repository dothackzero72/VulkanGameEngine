#include "DepthTexture.h"

DepthTexture::DepthTexture()
{
}

DepthTexture::DepthTexture(ivec2 textureResolution)
{
    Width = textureResolution.x;
    Height = textureResolution.y;
    Depth = 1;
    TextureImageLayout = VK_IMAGE_LAYOUT_UNDEFINED;
    SampleCount = VK_SAMPLE_COUNT_1_BIT;
    TextureByteFormat = VK_FORMAT_D32_SFLOAT;

    CreateTextureImage();
    CreateTextureView();
    CreateTextureSampler();
}

DepthTexture::DepthTexture(VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo) : Texture()
{
    Width = (int)createImageInfo.extent.width;
    Height = (int)createImageInfo.extent.height;
    Depth = (int)createImageInfo.extent.depth;
    TextureByteFormat = createImageInfo.format;
    TextureImageLayout = createImageInfo.initialLayout;
    SampleCount = createImageInfo.samples;

    CreateTextureImage(createImageInfo);
    CreateTextureView();
    CreateTextureSampler(samplerCreateInfo);
    ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}


DepthTexture::~DepthTexture()
{
}

VkResult DepthTexture::CreateTextureView()
{
    VkImageViewCreateInfo textureImageViewInfo =
    {
        .sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
        .image = Image,
        .viewType = VK_IMAGE_VIEW_TYPE_2D,
        .format = TextureByteFormat,
        .subresourceRange = VkImageSubresourceRange
         {
            .aspectMask = VK_IMAGE_ASPECT_DEPTH_BIT,
            .baseMipLevel = 0,
            .levelCount = MipMapLevels,
            .baseArrayLayer = 0,
            .layerCount = 1,
         }
    };

    return vkCreateImageView(cRenderer.Device, &textureImageViewInfo, nullptr, &View);
}

void DepthTexture::RecreateRendererTexture(vec2 textureResolution)
{
    Width = (int)textureResolution.x;
    Height = (int)textureResolution.y;

    CreateTextureImage();
    CreateTextureView();
    CreateTextureSampler();
}