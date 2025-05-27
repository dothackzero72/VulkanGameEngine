#include "Texture.h"
#include "VulkanBufferFuncs.h"

#define STB_IMAGE_IMPLEMENTATION
#define STB_IMAGE_WRITE_IMPLEMENTATION
#include <stb/stb_image.h>

Texture Texture_LoadTexture(const RendererState& rendererState, const TextureJsonLoader& textureLoader)
{
	Texture texture;
	texture.textureId = textureLoader.TextureId;
	texture.textureByteFormat = textureLoader.TextureByteFormat;
	texture.mipMapLevels = textureLoader.UseMipMaps ? static_cast<uint32>(std::floor(std::log2(std::max(texture.width, texture.height)))) + 1 : 1;
	texture.textureType = textureLoader.TextureType;

	Texture_CreateTextureImage(rendererState, texture, textureLoader.TextureFilePath);
	Texture_CreateTextureView(rendererState, texture, textureLoader.ImageType);
	Texture_CreateSpriteTextureSampler(rendererState, texture.textureSampler);
	return texture;
}

Texture Texture_CreateTexture(const RendererState& rendererState, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo)
{
	Texture texture;
	texture.width = static_cast<int>(createImageInfo.extent.width);
	texture.height = static_cast<int>(createImageInfo.extent.height);
	texture.depth = static_cast<int>(createImageInfo.extent.depth);
	texture.textureByteFormat = createImageInfo.format;
	texture.textureImageLayout = createImageInfo.initialLayout;
	texture.sampleCount = createImageInfo.samples;
	texture.mipMapLevels = 1;

	Texture_CreateTextureImage(rendererState, texture, createImageInfo);
	Texture_CreateTextureView(rendererState, texture, imageType);
	Texture_CreateRenderedTextureSampler(rendererState, texture.textureSampler);
	return texture;
}

Texture Texture_CreateTexture(const RendererState& rendererState, const String& texturePath, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps)
{
	Texture texture;
	texture.width = static_cast<int>(createImageInfo.extent.width);
	texture.height = static_cast<int>(createImageInfo.extent.height);
	texture.depth = static_cast<int>(createImageInfo.extent.depth);
	texture.textureByteFormat = createImageInfo.format;
	texture.textureImageLayout = createImageInfo.initialLayout;
	texture.sampleCount = createImageInfo.samples;
	texture.mipMapLevels = useMipMaps ? static_cast<uint32>(std::floor(std::log2(std::max(texture.width, texture.height)))) + 1 : 1;

	Texture_CreateTextureImage(rendererState, texture, texturePath);
	Texture_CreateTextureView(rendererState, texture, imageType);
	Texture_CreateRenderedTextureSampler(rendererState, texture.textureSampler);
	return texture;
}

Texture Texture_CreateTexture(const RendererState& rendererState, Pixel& clearColor, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps)
{
	Texture texture;
	texture.width = static_cast<int>(createImageInfo.extent.width);
	texture.height = static_cast<int>(createImageInfo.extent.height);
	texture.depth = static_cast<int>(createImageInfo.extent.depth);
	texture.textureByteFormat = createImageInfo.format;
	texture.textureImageLayout = createImageInfo.initialLayout;
	texture.sampleCount = createImageInfo.samples;
	texture.mipMapLevels = useMipMaps ? static_cast<uint32>(std::floor(std::log2(std::max(texture.width, texture.height)))) + 1 : 1;

	Texture_CreateTextureImage(rendererState, texture, clearColor);
	Texture_CreateTextureView(rendererState, texture, imageType);
	Texture_CreateRenderedTextureSampler(rendererState, texture.textureSampler);
	return texture;
}

void Texture_UpdateTextureBufferIndex(Texture& texture, uint32 bufferIndex)
{
	texture.textureBufferIndex = bufferIndex;
}

void Texture_UpdateTextureSize(const RendererState& rendererState, Texture& texture, VkImageAspectFlags imageType, vec2& TextureResolution)
{
	texture.width = TextureResolution.x;
	texture.height = TextureResolution.y;

	Texture_DestroyTexture(rendererState, texture);
	Texture_UpdateImage(rendererState, texture);
	Texture_CreateTextureView(rendererState, texture, imageType);
	Texture_CreateRenderedTextureSampler(rendererState, texture.textureSampler);

	//ImGuiDescriptorSet = ImGui_ImplVulkan_AddTexture(Sampler, View, VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);
}

void Texture_GetTexturePropertiesBuffer(Texture& texture, Vector<VkDescriptorImageInfo>& textureDescriptorList)
{
	VkDescriptorImageInfo textureDescriptor =
	{
		.sampler = texture.textureSampler,
		.imageView = texture.textureView,
		.imageLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL
	};
	textureDescriptorList.emplace_back(textureDescriptor);
}

void Texture_DestroyTexture(const RendererState& rendererState, Texture& texture)
{
	Renderer_DestroyImageView(rendererState.Device, &texture.textureView);
	Renderer_DestroySampler(rendererState.Device, &texture.textureSampler);
	Renderer_DestroyImage(rendererState.Device, &texture.textureImage);
	Renderer_FreeDeviceMemory(rendererState.Device, &texture.textureMemory);
}

 Texture Texture_LoadTexture_CS(const RendererStateCS& rendererStateCS, const TextureJsonLoader& textureJsonLoader)
 {
	 RendererState renderState = Renderer_RendererStateCStoCPP(rendererStateCS);
	 return Texture_LoadTexture(renderState, textureJsonLoader);
 }

 Texture Texture_CreateTexture_CS(const RendererStateCS& rendererStateCS, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo)
 {
	 RendererState renderState = Renderer_RendererStateCStoCPP(rendererStateCS);
	 return Texture_CreateTexture(renderState, imageType, createImageInfo, samplerCreateInfo);
 }

 Texture Texture_CreateTexture_CS(const RendererStateCS& rendererStateCS, const String& texturePath, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps)
 {
	 RendererState renderState = Renderer_RendererStateCStoCPP(rendererStateCS);
	 return Texture_CreateTexture(renderState, texturePath, imageType, createImageInfo, samplerCreateInfo, useMipMaps);
 }

 Texture Texture_CreateTexture_CS(const RendererStateCS& rendererStateCS, Pixel& clearColor, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps)
 {
	 RendererState renderState = Renderer_RendererStateCStoCPP(rendererStateCS);
	 return Texture_CreateTexture(renderState, clearColor, imageType, createImageInfo, samplerCreateInfo, useMipMaps);
 }

 void Texture_UpdateTextureSize_CS(const RendererStateCS& rendererStateCS, Texture& texture, VkImageAspectFlags imageType, vec2& TextureResolution)
 {
	 RendererState renderState = Renderer_RendererStateCStoCPP(rendererStateCS);
	 return Texture_UpdateTextureSize(renderState, texture, imageType, TextureResolution);
 }

void Texture_CreateTextureImage(const RendererState& rendererState, Texture& texture, const Pixel& clearColor)
{
	VkBuffer buffer = VK_NULL_HANDLE;
	VkBuffer stagingBuffer = VK_NULL_HANDLE;
	VkDeviceMemory stagingBufferMemory = VK_NULL_HANDLE;
	VkDeviceMemory bufferMemory = VK_NULL_HANDLE;

	VkDeviceSize bufferSize = texture.width * texture.height * texture.colorChannels;
	VkMemoryPropertyFlags bufferProperties = VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;
	VkBufferUsageFlags bufferUsage = VK_BUFFER_USAGE_TRANSFER_SRC_BIT;

	std::vector<Pixel> pixels(texture.width * texture.height, clearColor);
	Buffer_CreateStagingBuffer(rendererState, &stagingBuffer, &buffer, &stagingBufferMemory, &bufferMemory, (void*)pixels.data(), bufferSize, bufferUsage, bufferProperties);

	VkImageCreateInfo imageCreateInfo = 
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
		.imageType = VK_IMAGE_TYPE_2D,
		.format = texture.textureByteFormat,
		.extent = VkExtent3D
		{
			.width = static_cast<uint32_t>(texture.width), 
			.height = static_cast<uint32_t>(texture.height),
			.depth = 1 
		},
		.mipLevels = texture.mipMapLevels,
		.arrayLayers = 1,
		.samples = VK_SAMPLE_COUNT_1_BIT,
		.tiling = VK_IMAGE_TILING_OPTIMAL,
		.usage = VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
		.sharingMode = VK_SHARING_MODE_EXCLUSIVE,
		.initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
	};

	VkImageLayout newTextureLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
	VULKAN_RESULT(Texture_CreateImage(rendererState, texture, imageCreateInfo));
	VULKAN_RESULT(Texture_QuickTransitionImageLayout(rendererState, texture, newTextureLayout));
	VULKAN_RESULT(Texture_CopyBufferToTexture(rendererState, texture, buffer));
	VULKAN_RESULT(Texture_GenerateMipmaps(rendererState, texture));

	Renderer_DestroyBuffer(rendererState.Device, &stagingBuffer);
	Renderer_FreeDeviceMemory(rendererState.Device, &stagingBufferMemory);
}

void Texture_CreateTextureImage(const RendererState& rendererState, Texture& texture, const String& filePath)
{
	VkBuffer buffer = VK_NULL_HANDLE;
	VkBuffer stagingBuffer = VK_NULL_HANDLE;
	VkDeviceMemory stagingBufferMemory = VK_NULL_HANDLE;
	VkDeviceMemory bufferMemory = VK_NULL_HANDLE;

	byte* data = stbi_load(filePath.c_str(), &texture.width, &texture.height, (int*)&texture.colorChannels, 0);
	VkDeviceSize bufferSize = texture.width * texture.height * texture.colorChannels;
	VkBufferUsageFlags bufferUsage = VK_BUFFER_USAGE_TRANSFER_SRC_BIT | VK_BUFFER_USAGE_TRANSFER_DST_BIT;
	VkMemoryPropertyFlags bufferProperties = VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;

	Buffer_CreateStagingBuffer(rendererState, &stagingBuffer, &buffer, &stagingBufferMemory, &bufferMemory, data, bufferSize, bufferUsage, bufferProperties);

	VkImageCreateInfo imageCreateInfo =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
		.imageType = VK_IMAGE_TYPE_2D,
		.format = texture.textureByteFormat,
		.extent = VkExtent3D
		{
			.width = static_cast<uint32_t>(texture.width),
			.height = static_cast<uint32_t>(texture.height),
			.depth = 1
		},
		.mipLevels = texture.mipMapLevels,
		.arrayLayers = 1,
		.samples = VK_SAMPLE_COUNT_1_BIT,
		.tiling = VK_IMAGE_TILING_OPTIMAL,
		.usage = VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
		.sharingMode = VK_SHARING_MODE_EXCLUSIVE,
		.initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
	};

	VkImageLayout newTextureLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
	VULKAN_RESULT(Texture_CreateImage(rendererState, texture, imageCreateInfo));
	VULKAN_RESULT(Texture_QuickTransitionImageLayout(rendererState, texture, newTextureLayout));
	VULKAN_RESULT(Texture_CopyBufferToTexture(rendererState, texture, buffer));
	VULKAN_RESULT(Texture_GenerateMipmaps(rendererState, texture));

	Renderer_DestroyBuffer(rendererState.Device, &stagingBuffer);
	Renderer_FreeDeviceMemory(rendererState.Device, &stagingBufferMemory);
	stbi_image_free(data);
}

void Texture_CreateTextureImage(const RendererState& rendererState, Texture& texture, VkImageCreateInfo& createImageInfo)
{
	VULKAN_RESULT(vkCreateImage(rendererState.Device, &createImageInfo, nullptr, &texture.textureImage));

	VkMemoryRequirements memRequirements;
	vkGetImageMemoryRequirements(rendererState.Device, texture.textureImage, &memRequirements);

	VkMemoryAllocateInfo allocInfo =
	{
		.sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
		.allocationSize = memRequirements.size,
		.memoryTypeIndex = Renderer_GetMemoryType(rendererState.PhysicalDevice, memRequirements.memoryTypeBits, VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT)
	};
	VULKAN_RESULT(vkAllocateMemory(rendererState.Device, &allocInfo, nullptr, &texture.textureMemory));
	VULKAN_RESULT(vkBindImageMemory(rendererState.Device, texture.textureImage, texture.textureMemory, 0));
}

VkResult Texture_UpdateImage(const RendererState& rendererState, Texture& texture)
{
	VkImageCreateInfo imageCreateInfo = 
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
		.imageType = VK_IMAGE_TYPE_2D,
		.format = texture.textureByteFormat,
		.extent = 
		{
			.width = static_cast<uint32_t>(texture.width),
			.height = static_cast<uint32_t>(texture.height),
			.depth = 1
		},
		.mipLevels = texture.mipMapLevels,
		.arrayLayers = 1,
		.samples = VK_SAMPLE_COUNT_1_BIT,
		.tiling = VK_IMAGE_TILING_OPTIMAL,
		.usage = VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
		.sharingMode = VK_SHARING_MODE_EXCLUSIVE,
		.initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
	};
	VULKAN_RESULT(vkCreateImage(rendererState.Device, &imageCreateInfo, NULL, &texture.textureImage));

	VkMemoryRequirements memRequirements;
	vkGetImageMemoryRequirements(rendererState.Device, texture.textureImage, &memRequirements);

	VkMemoryAllocateInfo allocInfo =
	{
		.sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
		.allocationSize = memRequirements.size,
		.memoryTypeIndex = Renderer_GetMemoryType(rendererState.PhysicalDevice, memRequirements.memoryTypeBits, VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT)
	};
	VULKAN_RESULT(vkAllocateMemory(rendererState.Device, &allocInfo, NULL, &texture.textureMemory));
	return vkBindImageMemory(rendererState.Device, texture.textureImage, texture.textureMemory, 0);
}

VkResult Texture_CreateImage(const RendererState& rendererState, Texture& texture, VkImageCreateInfo& imageCreateInfo)
{
	VULKAN_RESULT(vkCreateImage(rendererState.Device, &imageCreateInfo, NULL, &texture.textureImage));

	VkMemoryRequirements memRequirements;
	vkGetImageMemoryRequirements(rendererState.Device, texture.textureImage, &memRequirements);

	VkMemoryAllocateInfo allocInfo =
	{
		.sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
		.allocationSize = memRequirements.size,
		.memoryTypeIndex = Renderer_GetMemoryType(rendererState.PhysicalDevice, memRequirements.memoryTypeBits, VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT)
	};
	VULKAN_RESULT(vkAllocateMemory(rendererState.Device, &allocInfo, NULL, &texture.textureMemory));
	return vkBindImageMemory(rendererState.Device, texture.textureImage, texture.textureMemory, 0);
}

VkResult Texture_CreateTextureView(const RendererState& rendererState, Texture& texture, VkImageAspectFlags imageAspectFlags)
{
	VkImageViewCreateInfo TextureImageViewInfo =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
		.image = texture.textureImage,
		.viewType = VK_IMAGE_VIEW_TYPE_2D,
		.format = texture.textureByteFormat,
		.subresourceRange =
		{
			.aspectMask = imageAspectFlags,
			.baseMipLevel = 0,
			.levelCount = texture.mipMapLevels,
			.baseArrayLayer = 0,
			.layerCount = 1,
		}
	};
	return vkCreateImageView(rendererState.Device, &TextureImageViewInfo, NULL, &texture.textureView);
}

 VkResult Texture_CreateSpriteTextureSampler(const RendererState& rendererState, VkSampler& smapler)
{
	 VkSamplerCreateInfo spriteSamplerInfo = {
	.sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
	.magFilter = VK_FILTER_NEAREST,           
	.minFilter = VK_FILTER_NEAREST,         
	.mipmapMode = VK_SAMPLER_MIPMAP_MODE_NEAREST, 
	.addressModeU = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE,
	.addressModeV = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE, 
	.addressModeW = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE, 
	.mipLodBias = 0,                          
	.anisotropyEnable = VK_FALSE,          
	.maxAnisotropy = 1.0f,                   
	.compareEnable = VK_FALSE,             
	.compareOp = VK_COMPARE_OP_ALWAYS,        
	.minLod = 0,                             
	.maxLod = 0,                 
	.borderColor = VK_BORDER_COLOR_INT_OPAQUE_BLACK, 
	.unnormalizedCoordinates = VK_FALSE, 
	 };
	 return vkCreateSampler(rendererState.Device, &spriteSamplerInfo, NULL, &smapler);
}

 VkResult Texture_CreateRenderedTextureSampler(const RendererState& rendererState, VkSampler& smapler)
{
	 VkSamplerCreateInfo renderPassSamplerInfo = {
	.sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
	.magFilter = VK_FILTER_LINEAR,          
	.minFilter = VK_FILTER_LINEAR,          
	.mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR, 
	.addressModeU = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE,
	.addressModeV = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE, 
	.addressModeW = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE,
	.mipLodBias = 0,                         
	.anisotropyEnable = VK_FALSE,            
	.maxAnisotropy = 1.0f,                  
	.compareEnable = VK_FALSE,              
	.compareOp = VK_COMPARE_OP_ALWAYS,       
	.minLod = 0,                           
	.maxLod = 0,                          
	.borderColor = VK_BORDER_COLOR_INT_OPAQUE_BLACK,
	.unnormalizedCoordinates = VK_FALSE,    
	 };
	 return vkCreateSampler(rendererState.Device, &renderPassSamplerInfo, NULL, &smapler);
}

 VkResult Texture_CreateDepthTextureSampler(const RendererState& rendererState, VkSampler& smapler)
{
	 VkSamplerCreateInfo depthMapSamplerInfo = {
	.sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
	.magFilter = VK_FILTER_LINEAR,           
	.minFilter = VK_FILTER_LINEAR,         
	.mipmapMode = VK_SAMPLER_MIPMAP_MODE_NEAREST,
	.addressModeU = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE, 
	.addressModeV = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE,
	.addressModeW = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE,
	.mipLodBias = 0,                     
	.anisotropyEnable = VK_FALSE,            
	.maxAnisotropy = 1.0f,                   
	.compareEnable = VK_TRUE,                
	.compareOp = VK_COMPARE_OP_LESS,         
	.minLod = 0,                           
	.maxLod = 0,                            
	.borderColor = VK_BORDER_COLOR_INT_OPAQUE_BLACK,
	.unnormalizedCoordinates = VK_FALSE,     
	 };
	 return vkCreateSampler(rendererState.Device, &depthMapSamplerInfo, NULL, &smapler);
}

VkResult Texture_TransitionImageLayout(const RendererState& rendererState, VkCommandBuffer& commandBuffer, Texture& texture, VkImageLayout newLayout)
{
	VkPipelineStageFlags sourceStage = VK_PIPELINE_STAGE_FLAG_BITS_MAX_ENUM;
	VkPipelineStageFlags destinationStage = VK_PIPELINE_STAGE_FLAG_BITS_MAX_ENUM;
	VkImageMemoryBarrier barrier =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
		.oldLayout = texture.textureImageLayout,
		.newLayout = newLayout,
		.srcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.dstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.image = texture.textureImage,
		.subresourceRange = VkImageSubresourceRange
		{
			.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
			.baseMipLevel = 0,
			.levelCount = texture.mipMapLevels,
			.baseArrayLayer = 0,
			.layerCount = VK_REMAINING_ARRAY_LAYERS,
		}
	};
	if (texture.textureImageLayout == VK_IMAGE_LAYOUT_UNDEFINED &&
		newLayout == VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL)
	{
		barrier.srcAccessMask = 0;
		barrier.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;

		sourceStage = VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT;
		destinationStage = VK_PIPELINE_STAGE_TRANSFER_BIT;
	}
	else if (texture.textureImageLayout == VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL &&
		newLayout == VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL)
	{
		barrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
		barrier.dstAccessMask = VK_ACCESS_SHADER_READ_BIT;

		sourceStage = VK_PIPELINE_STAGE_TRANSFER_BIT;
		destinationStage = VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT;
	}

	vkCmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, 0, 0, NULL, 0, NULL, 1, &barrier);
	texture.textureImageLayout = newLayout;
	return VK_SUCCESS;
}

VkResult Texture_CommandBufferTransitionImageLayout(const RendererState& rendererState, VkCommandBuffer commandBuffer, Texture& texture, VkImageLayout newLayout)
{
	Texture_TransitionImageLayout(rendererState, commandBuffer, texture, newLayout);
	return VK_SUCCESS;
}

void Texture_UpdateCmdTextureLayout(const RendererState& rendererState, VkCommandBuffer& commandBuffer, Texture& texture, VkImageLayout& newImageLayout)
{
	VkImageMemoryBarrier imageMemoryBarrier =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
		.srcAccessMask = 0,
		.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT,
		.oldLayout = texture.textureImageLayout,
		.newLayout = newImageLayout,
		.image = texture.textureImage,
		.subresourceRange =
		{
			.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
			.baseMipLevel = texture.mipMapLevels,
			.levelCount = VK_REMAINING_MIP_LEVELS,
			.layerCount = 1
		}
	};

	vkCmdPipelineBarrier(commandBuffer, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, NULL, 0, NULL, 1, &imageMemoryBarrier);
	texture.textureImageLayout = newImageLayout;
}

void Texture_UpdateTextureLayout(const RendererState& rendererState, Texture& texture, VkImageLayout& newImageLayout)
{
	VkImageMemoryBarrier imageMemoryBarrier =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
		.srcAccessMask = 0,
		.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT,
		.oldLayout = texture.textureImageLayout,
		.newLayout = newImageLayout,
		.image = texture.textureImage,
		.subresourceRange =
		{
			.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
			.baseMipLevel = texture.mipMapLevels,
			.levelCount = VK_REMAINING_MIP_LEVELS,
			.layerCount = 1
		}
	};

	auto singleCommand = Renderer_BeginSingleUseCommandBuffer(rendererState.Device, rendererState.CommandPool);
	vkCmdPipelineBarrier(singleCommand, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, NULL, 0, NULL, 1, &imageMemoryBarrier);
	VkResult result = Renderer_EndSingleUseCommandBuffer(rendererState.Device, rendererState.CommandPool, rendererState.GraphicsQueue, singleCommand);
	if (result == VK_SUCCESS)
	{
		texture.textureImageLayout = newImageLayout;
	}
}

VkResult Texture_QuickTransitionImageLayout(const RendererState& rendererState, Texture& texture, VkImageLayout& newLayout)
{
	VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(rendererState.Device, rendererState.CommandPool);
	Texture_TransitionImageLayout(rendererState, commandBuffer, texture, newLayout);
	VkResult result = Renderer_EndSingleUseCommandBuffer(rendererState.Device, rendererState.CommandPool, rendererState.GraphicsQueue, commandBuffer);
	if (result == VK_SUCCESS)
	{
		texture.textureImageLayout = newLayout;
	}
	return result;
}

VkResult Texture_CopyBufferToTexture(const RendererState& rendererState, Texture& texture, VkBuffer buffer)
{
	VkBufferImageCopy BufferImage =
	{
		.bufferOffset = 0,
		.bufferRowLength = 0,
		.bufferImageHeight = 0,
		.imageSubresource = VkImageSubresourceLayers
		{
			.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
			.mipLevel = 0,
			.baseArrayLayer = 0,
			.layerCount = static_cast<uint>((texture.textureType == kUse_CubeMapTexture ? 6 : 1))
		},
		.imageOffset = VkOffset3D
		{
			.x = 0,
			.y = 0,
			.z = 0,
		},
		.imageExtent = VkExtent3D
		{
			.width = static_cast<uint>(texture.width),
			.height = static_cast<uint>(texture.height),
			.depth = static_cast<uint>(texture.depth),
		}
	};
	VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(rendererState.Device, rendererState.CommandPool);
	vkCmdCopyBufferToImage(commandBuffer, buffer, texture.textureImage, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &BufferImage);
	return Renderer_EndSingleUseCommandBuffer(rendererState.Device, rendererState.CommandPool, rendererState.GraphicsQueue, commandBuffer);
}

VkResult Texture_GenerateMipmaps(const RendererState& rendererState, Texture& texture)
{
	if (texture.mipMapLevels == 1)
	{
		return VK_SUCCESS;
	}

	int32 mipWidth = texture.width;
	int32 mipHeight = texture.height;

	VkFormatProperties formatProperties;
	vkGetPhysicalDeviceFormatProperties(rendererState.PhysicalDevice, texture.textureByteFormat, &formatProperties);
	if (!(formatProperties.optimalTilingFeatures & VK_FORMAT_FEATURE_SAMPLED_IMAGE_FILTER_LINEAR_BIT))
	{
		RENDERER_ERROR("Texture image format does not support linear blitting.");
	}

	VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(rendererState.Device, rendererState.CommandPool);
	VkImageMemoryBarrier ImageMemoryBarrier =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
		.srcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.dstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.image = texture.textureImage,
		.subresourceRange = VkImageSubresourceRange
		{
			.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
			.levelCount = 1,
			.baseArrayLayer = 0,
			.layerCount = 1,
		}
	};
	for (uint32 x = 1; x < texture.mipMapLevels; x++)
	{
		ImageMemoryBarrier.subresourceRange.baseMipLevel = x - 1;
		ImageMemoryBarrier.oldLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
		ImageMemoryBarrier.newLayout = VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL;
		ImageMemoryBarrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
		ImageMemoryBarrier.dstAccessMask = VK_ACCESS_TRANSFER_READ_BIT;
		vkCmdPipelineBarrier(commandBuffer, VK_PIPELINE_STAGE_TRANSFER_BIT, VK_PIPELINE_STAGE_TRANSFER_BIT, 0, 0, NULL, 0, NULL, 1, &ImageMemoryBarrier);

		VkImageBlit ImageBlit =
		{
			.srcSubresource = VkImageSubresourceLayers
			{
				.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
				.mipLevel = x - 1,
				.baseArrayLayer = 0,
				.layerCount = 1,
			},
			.srcOffsets =
			{
				VkOffset3D
				{
					.x = 0,
					.y = 0,
					.z = 0,
				},
				VkOffset3D
				{
					.x = mipWidth,
					.y = mipHeight,
					.z = 1,
				}
			},
			.dstSubresource = VkImageSubresourceLayers
			{
				.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
				.mipLevel = x,
				.baseArrayLayer = 0,
				.layerCount = 1,
			},
			.dstOffsets =
			{
				VkOffset3D
				{
					.x = 0,
					.y = 0,
					.z = 0,
				},
				VkOffset3D
				{
					.x = static_cast<int32_t>(mipWidth > 1 ? mipWidth / 2 : 1),
					.y = static_cast<int32_t>(mipHeight > 1 ? mipHeight / 2 : 1),
					.z = static_cast<int32_t>(1),
				}
			}
		};
		vkCmdBlitImage(commandBuffer, texture.textureImage, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL, texture.textureImage, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &ImageBlit, VK_FILTER_LINEAR);

		ImageMemoryBarrier.oldLayout = VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL;
		ImageMemoryBarrier.newLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
		ImageMemoryBarrier.srcAccessMask = VK_ACCESS_TRANSFER_READ_BIT;
		ImageMemoryBarrier.dstAccessMask = VK_ACCESS_SHADER_READ_BIT;

		vkCmdPipelineBarrier(commandBuffer, VK_PIPELINE_STAGE_TRANSFER_BIT, VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT, 0, 0, NULL, 0, NULL, 1, &ImageMemoryBarrier);

		if (mipWidth > 1)
		{
			mipWidth /= 2;
		}
		if (mipHeight > 1)
		{
			mipHeight /= 2;
		};
	}

	ImageMemoryBarrier.subresourceRange.baseMipLevel = texture.mipMapLevels - 1;
	ImageMemoryBarrier.oldLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
	ImageMemoryBarrier.newLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
	ImageMemoryBarrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
	ImageMemoryBarrier.dstAccessMask = VK_ACCESS_SHADER_READ_BIT;

	vkCmdPipelineBarrier(commandBuffer, VK_PIPELINE_STAGE_TRANSFER_BIT, VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT, 0, 0, NULL, 0, NULL, 1, &ImageMemoryBarrier);
	return Renderer_EndSingleUseCommandBuffer(rendererState.Device, rendererState.CommandPool, rendererState.GraphicsQueue, commandBuffer);
}