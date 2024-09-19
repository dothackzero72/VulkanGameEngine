#pragma once
extern "C"
{
	#include <CTexture.h>
}
#include <cmath>
#include <algorithm>
#include <string>
#include "typedef.h"
#include "DynamicVulkanBuffer.h"
#include <Imgui/imgui.h>
#include <Imgui/backends/imgui_impl_vulkan.h>

enum ColorChannelUsed
{
	ChannelR = 1,
	ChannelRG,
	ChannelRGB,
	ChannelRGBA
};

class TextureFunctions;
class Texture
{
	private:
		uint64 TextureBufferIndex;

		VkResult CopyBufferToTexture(VkBuffer buffer);
		VkResult GenerateMipmaps();

	protected:
		int Width;
		int Height;
		int Depth;
		ColorChannelUsed ColorChannels;
		uint32 MipMapLevels;

		TextureUsageEnum TextureUsage;
		TextureTypeEnum TextureType;
		VkFormat TextureByteFormat;
		VkImageLayout TextureImageLayout;
		VkSampleCountFlagBits SampleCount;

		VkResult CreateTextureImage();
		virtual void CreateImageTexture(const std::string& FilePath);
		virtual void CreateTextureSampler();
		VkResult CreateTextureView();
		VkResult CreateTextureSampler(VkSamplerCreateInfo samplerCreateInfo);

	public:
		friend class TextureFunctions;

		VkDescriptorSet ImGuiDescriptorSet;
		VkImage Image;
		VkDeviceMemory Memory;
		VkImageView View;
		VkSampler Sampler;
		VkDescriptorImageInfo textureBuffer;

		Texture();
		Texture(const std::string& filePath, VkFormat textureByteFormat, TextureTypeEnum TextureType);
		virtual ~Texture();
		virtual void UpdateTextureSize(vec2 TextureResolution);
		virtual void Destroy();

		VkResult TransitionImageLayout(VkImageLayout newLayout);
		VkResult TransitionImageLayout(VkImageLayout oldLayout, VkImageLayout newLayout);
		VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, VkImageLayout newLayout);
		VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, VkImageLayout oldLayout, VkImageLayout newLayout);

		void ImGuiShowTexture(const ImVec2& TextureDisplaySize);
		VkDescriptorImageInfo* GetTextureBuffer();

		const VkFormat GetTextureByteFormat() { return TextureByteFormat; }
		const VkSampleCountFlagBits GetSampleCount() { return SampleCount; }
};

