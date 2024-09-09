#include "RenderedColorTexture.h"
#include "TextureFunctions.h"

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
    
	VULKAN_RESULT(TextureFunctions::CreateTextureImage(this));
	VULKAN_RESULT(TextureFunctions::CreateTextureView(this));
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
		.magFilter = VK_FILTER_NEAREST,
		.minFilter = VK_FILTER_NEAREST,
		.mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR,
		.addressModeU = VK_SAMPLER_ADDRESS_MODE_REPEAT,
		.addressModeV = VK_SAMPLER_ADDRESS_MODE_REPEAT,
		.addressModeW = VK_SAMPLER_ADDRESS_MODE_REPEAT,
		.mipLodBias = 0,
		.anisotropyEnable = VK_TRUE,
		.maxAnisotropy = 16.0f,
		.compareEnable = VK_FALSE,
		.compareOp = VK_COMPARE_OP_ALWAYS,
		.minLod = 0,
		.maxLod = static_cast<float>(MipMapLevels),
		.borderColor = VK_BORDER_COLOR_INT_OPAQUE_BLACK,
		.unnormalizedCoordinates = VK_FALSE,
	};
	VULKAN_RESULT(TextureFunctions::CreateTextureSampler(&TextureImageSamplerInfo, &Sampler));
}

