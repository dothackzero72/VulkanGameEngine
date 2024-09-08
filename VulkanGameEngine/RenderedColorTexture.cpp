#include "RenderedColorTexture.h"

RenderedColorTexture::RenderedColorTexture() : Texture()
{
}

RenderedColorTexture::RenderedColorTexture(glm::ivec2& textureResolution, VkFormat format) : Texture()
{
    Width = textureResolution.x;
    Height = textureResolution.y;
    TextureUsage = TextureUsageEnum::kUse_2DRenderedTexture;
    TextureType = TextureTypeEnum::kType_RenderedColorTexture;
    TextureByteFormat = format;
    
    Texture_CreateTextureImage(SendCTextureInfo().get());
    CreateTextureView();
    CreateTextureSampler();

    ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

RenderedColorTexture::~RenderedColorTexture()
{
}

void RenderedColorTexture::CreateTextureSampler()
{
    VkSamplerCreateInfo TextureImageSamplerInfo =
    {
        .sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
        .magFilter = VK_FILTER_LINEAR,                              
        .minFilter = VK_FILTER_LINEAR,                        
        .mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR,             
        .addressModeU = VK_SAMPLER_ADDRESS_MODE_REPEAT,           
        .addressModeV = VK_SAMPLER_ADDRESS_MODE_REPEAT,           
        .addressModeW = VK_SAMPLER_ADDRESS_MODE_REPEAT,           
        .mipLodBias = 0.0f,                                       
        .anisotropyEnable = VK_TRUE,                              
        .maxAnisotropy = 16.0f,                                  
        .compareEnable = VK_FALSE,                                 
        .compareOp = VK_COMPARE_OP_ALWAYS,                        
        .minLod = 0.0f,                                           
        .maxLod = static_cast<float>(MipMapLevels),            
        .borderColor = VK_BORDER_COLOR_INT_OPAQUE_BLACK,         
        .unnormalizedCoordinates = VK_FALSE,                       
    };
	VULKAN_RESULT(Texture_CreateTextureSampler(SendCTextureInfo().get(), &TextureImageSamplerInfo));
}

