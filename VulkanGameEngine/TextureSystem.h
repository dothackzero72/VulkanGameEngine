#pragma once
#include "VulkanRenderer.h"
#include "VkGuid.h"
#include <Texture.h>
#include "RenderSystem.h"

class TextureSystem
{
private:
public:
	UnorderedMap<RenderPassGuid, Texture>						  TextureList;
	UnorderedMap<RenderPassGuid, Texture>						  DepthTextureList;
	UnorderedMap<RenderPassGuid, Vector<Texture>>				  RenderedTextureList;
	UnorderedMap<UM_RenderPipelineID, Vector<SharedPtr<Texture>>> InputTextureList;

	VkGuid  LoadTexture(const String& texturePath);
	Texture CreateTexture(VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo);
	Texture CreateTexture(const String& texturePath, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps);
    Texture CreateTexture(Pixel& clearColor, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps);

	void UpdateTextureBufferIndex(Texture& texture, uint32 bufferIndex);
	void UpdateTextureSize(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, Texture& texture, VkImageAspectFlags imageType, vec2& TextureResolution);
	void GetTexturePropertiesBuffer(Texture& texture, Vector<VkDescriptorImageInfo>& textureDescriptorList);
	void UpdateTextureLayout(Texture& texture, VkCommandBuffer& commandBuffer, VkImageLayout newImageLayout);
	void DestroyTexture(VkDevice device, Texture& texture);
};
extern TextureSystem textureSystem;
