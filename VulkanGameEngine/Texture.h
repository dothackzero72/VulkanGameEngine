#pragma once
extern "C"
{
	#include <CTexture.h>
	#include <CBuffer.h>
}
#include <cmath>
#include <algorithm>
#include <string>
#include "typedef.h"
#include "VulkanBuffer.h"
#include <Imgui/imgui.h>
#include <Imgui/imgui_impl_vulkan.h>

class Texture
{
	private:
		uint64 TextureBufferIndex;

	protected:
		int Width;
		int Height;
		int Depth;
		uint32 MipMapLevels;

		TextureUsageEnum TextureUsage;
		TextureTypeEnum TextureType;
		VkFormat TextureByteFormat;
		VkImageLayout TextureImageLayout;
		VkSampleCountFlagBits SampleCount;

		std::unique_ptr<TextureInfo> SendCTextureInfo();
		virtual void CreateImageTexture(const std::string& FilePath);
		virtual void CreateTextureView();
		virtual void CreateTextureSampler();


	public:
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
		void ImGuiShowTexture(const ImVec2& TextureDisplaySize);
		VkDescriptorImageInfo* GetTextureBuffer();

		const VkFormat GetTextureByteFormat() { return TextureByteFormat; }
		const VkSampleCountFlagBits GetSampleCount() { return SampleCount; }
};

