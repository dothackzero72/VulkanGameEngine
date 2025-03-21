#include <imgui/backends/imgui_impl_vulkan.h>
#include "RenderedTexture.h"
#include <stb/stb_image_write.h>
#include <stb/stb_image.h>

RenderedTexture::RenderedTexture()
{

}

RenderedTexture::RenderedTexture(VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo)
{
	Width = (int)createImageInfo.extent.width;
	Height = (int)createImageInfo.extent.height;
	Depth = (int)createImageInfo.extent.depth;
	TextureByteFormat = createImageInfo.format;
	TextureImageLayout = createImageInfo.initialLayout;
	SampleCount = createImageInfo.samples;

	CreateImage(createImageInfo);
	Texture_CreateTextureView(cRenderer.Device, &View, Image, TextureByteFormat, imageType, MipMapLevels);
	CreateTextureSampler(samplerCreateInfo);
	ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

RenderedTexture::~RenderedTexture()
{
}

void RenderedTexture::RecreateRendererTexture(VkImageAspectFlags imageType, glm::vec2 TextureResolution)
{
	Width = TextureResolution.x;
	Height = TextureResolution.y;

	Texture::Destroy();

	VkImageCreateInfo imageCreateInfo =
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


	CreateImage(imageCreateInfo);
	Texture_CreateTextureView(cRenderer.Device, &View, Image, TextureByteFormat, imageType, MipMapLevels);
	CreateTextureSampler();

	ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}