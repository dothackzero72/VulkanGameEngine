#pragma once
#include "Texture.h"
#include "RenderedColorTexture.h"
#include "DynamicVulkanBuffer.h"

class TextureFunctions
{
private:
	static VkResult CreateTextureImage(Texture& texture);
	static VkResult CreateTextureImage(Texture* texture);
	static VkResult CreateTextureImage(std::shared_ptr<Texture> texture);
	static VkResult CreateTextureImage(VkImage& image, VkDeviceMemory memory, vec2 textureSize, uint32 mipmapLevels, VkFormat textureByteFormat);
	static VkResult CreateTextureImage(VkImage& image, VkDeviceMemory memory, ivec2 textureSize, uint32 mipmapLevels, VkFormat textureByteFormat);
	static VkResult CreateTextureImage(VkImage& image, VkDeviceMemory memory, int width, int height, uint32 mipmapLevels, VkFormat textureByteFormat);

	static VkResult CreateTextureView(Texture& texture);
	static VkResult CreateTextureView(Texture* texture);
	static VkResult CreateTextureView(std::shared_ptr<Texture> texture);
	static VkResult CreateTextureView(VkImageView& view, VkImage image, VkFormat format, uint32 mipmapLevels);

	static VkResult CreateTextureSampler(VkSamplerCreateInfo* samplerCreateInfo, VkSampler* smapler);

public:
	friend class Texture;
	friend class RenderedColorTexture;

	static VkResult TransitionImageLayout(Texture texture, VkImageLayout newLayout);
	static VkResult TransitionImageLayout(Texture* texture, VkImageLayout newLayout);
	static VkResult TransitionImageLayout(std::shared_ptr<Texture> texture, VkImageLayout newLayout);
	static VkResult TransitionImageLayout(Texture texture, VkImageLayout oldLayout, VkImageLayout newLayout);
	static VkResult TransitionImageLayout(Texture* texture, VkImageLayout oldLayout, VkImageLayout newLayout);
	static VkResult TransitionImageLayout(std::shared_ptr<Texture> texture, VkImageLayout oldLayout, VkImageLayout newLayout);
	static VkResult TransitionImageLayout(VkImage image, uint32 MipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout);

	static VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, Texture texture, VkImageLayout newLayout);
	static VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, Texture* texture, VkImageLayout newLayout);
	static VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, std::shared_ptr<Texture> texture, VkImageLayout newLayout);
	static VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, Texture texture, VkImageLayout oldLayout, VkImageLayout newLayout);
	static VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, Texture* texture, VkImageLayout oldLayout, VkImageLayout newLayout);
	static VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, std::shared_ptr<Texture> texture, VkImageLayout oldLayout, VkImageLayout newLayout);
	static VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout oldLayout, VkImageLayout newLayout);
	static VkResult TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint32 MipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout);
	
	template <class T> static VkResult CopyBufferToTexture(Texture& texture, VulkanBuffer<T>& buffer);
	template <class T> static VkResult CopyBufferToTexture(Texture* texture, VulkanBuffer<T>& buffer);
	template <class T> static VkResult CopyBufferToTexture(std::shared_ptr<Texture> texture, VulkanBuffer<T>& buffer);
	template <class T> static VkResult CopyBufferToTexture(Texture& texture, DynamicVulkanBuffer<T>& buffer);
	template <class T> static VkResult CopyBufferToTexture(Texture* texture, DynamicVulkanBuffer<T>& buffer);
	template <class T> static VkResult CopyBufferToTexture(std::shared_ptr<Texture> texture, DynamicVulkanBuffer<T>& buffer);
	static VkResult CopyBufferToTexture(Texture& texture, VkBuffer& buffer);
	static VkResult CopyBufferToTexture(Texture* texture, VkBuffer& buffer);
	static VkResult CopyBufferToTexture(std::shared_ptr<Texture> texture, VkBuffer& buffer);
	static VkResult CopyBufferToTexture(VkImage image, VkBuffer buffer, enum TextureUsageEnum textureType, vec3 textureSize);
	static VkResult CopyBufferToTexture(VkImage image, VkBuffer buffer, enum TextureUsageEnum textureType, ivec3 textureSize);
	static VkResult CopyBufferToTexture(VkImage image, VkBuffer buffer, enum TextureUsageEnum textureType, int width, int height, int depth);

	static VkResult GenerateMipmaps(Texture& texture);
	static VkResult GenerateMipmaps(Texture* texture);
	static VkResult GenerateMipmaps(std::shared_ptr<Texture> texture);
	static VkResult GenerateMipmaps(VkImage image, VkFormat& textureByteFormat, uint32 mipmapLevels, int width, int height);
	static VkResult GenerateMipmaps(VkImage image, VkFormat& textureByteFormat, uint32 mipmapLevels, ivec2 textureSize);
	static VkResult GenerateMipmaps(VkImage image, VkFormat& textureByteFormat, uint32 mipmapLevels, vec2 textureSize);
};

