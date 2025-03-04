#include "RenderedColorTexture.h"
#include <stb/stb_image.h>
#include <CVulkanRenderer.h>

RenderedColorTexture::RenderedColorTexture() : Texture()
{
}

RenderedColorTexture::RenderedColorTexture(const String& filePath, VkFormat textureByteFormat, TextureTypeEnum textureType) : Texture()
{
	//MipMapLevels = static_cast<uint32>(std::floor(std::log2(std::max(Width, Height)))) + 1;
	TextureType = textureType;
	TextureByteFormat = textureByteFormat;

	CreateImageTexture(filePath);
	CreateTextureView();
	CreateTextureSampler();
	ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

RenderedColorTexture::RenderedColorTexture(glm::ivec2& textureResolution, VkFormat format) : Texture()
{
    Width = textureResolution.x;
    Height = textureResolution.y;
    TextureUsage = TextureUsageEnum::kUse_2DRenderedTexture;
    TextureType = TextureTypeEnum::kType_RenderedColorTexture;
    TextureByteFormat = format;
    

	VkImageCreateInfo ImageCreateInfo =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
		.imageType = VK_IMAGE_TYPE_2D,
		.format = TextureByteFormat,
		.extent =
		{
			.width = static_cast<uint>(Width),
			.height = static_cast<uint>(Height),
			.depth = 1
		},
		.mipLevels = MipMapLevels,
		.arrayLayers = 1,
		.samples = VK_SAMPLE_COUNT_1_BIT,
		.tiling = VK_IMAGE_TILING_OPTIMAL,
		.usage = VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
		.sharingMode = VK_SHARING_MODE_EXCLUSIVE,
		.initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
	};

	VULKAN_RESULT(CreateImage(ImageCreateInfo));
	VULKAN_RESULT(CreateTextureView());
    CreateTextureSampler();

    ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

RenderedColorTexture::~RenderedColorTexture()
{
}

void RenderedColorTexture::CreateImageTexture(const String& FilePath)
{
	int* width = &Width;
	int* height = &Height;
	int colorChannels = 0;
	byte* data = stbi_load(FilePath.c_str(), width, height, &colorChannels, 0);
	VulkanBuffer<byte> stagingBuffer((void*)data, Width * Height * colorChannels, VK_BUFFER_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, false);

	VkImageCreateInfo ImageCreateInfo =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
		.imageType = VK_IMAGE_TYPE_2D,
		.format = TextureByteFormat,
		.extent =
		{
			.width = static_cast<uint>(Width),
			.height = static_cast<uint>(Height),
			.depth = 1
		},
		.mipLevels = MipMapLevels,
		.arrayLayers = 1,
		.samples = VK_SAMPLE_COUNT_1_BIT,
		.tiling = VK_IMAGE_TILING_OPTIMAL,
		.usage = VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
		.sharingMode = VK_SHARING_MODE_EXCLUSIVE,
		.initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
	};

	VULKAN_RESULT(CreateImage(ImageCreateInfo));
	VULKAN_RESULT(TransitionImageLayout(VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL));
	VULKAN_RESULT(CopyBufferToTexture(stagingBuffer.Buffer));
	stagingBuffer.DestroyBuffer();

	ColorChannels = (ColorChannelUsed)colorChannels;
	stbi_image_free(data);
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
	VULKAN_RESULT(Texture::CreateTextureSampler(TextureImageSamplerInfo));
}

