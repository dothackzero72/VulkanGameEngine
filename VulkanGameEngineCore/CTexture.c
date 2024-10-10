#include "CTexture.h"
#include "CVulkanRenderer.h"

VkResult Texture_TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage* image, uint32 mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout)
{
	VkPipelineStageFlags sourceStage = VK_PIPELINE_STAGE_FLAG_BITS_MAX_ENUM;
	VkPipelineStageFlags destinationStage = VK_PIPELINE_STAGE_FLAG_BITS_MAX_ENUM;
	VkImageMemoryBarrier barrier =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
		.oldLayout = oldLayout,
		.newLayout = newLayout,
		.srcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.dstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.image = *image,
		.subresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
		.subresourceRange.baseMipLevel = 0,
		.subresourceRange.levelCount = mipmapLevels,
		.subresourceRange.baseArrayLayer = 0,
		.subresourceRange.layerCount = VK_REMAINING_ARRAY_LAYERS,
	};
	if (oldLayout == VK_IMAGE_LAYOUT_UNDEFINED &&
		newLayout == VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL)
	{
		barrier.srcAccessMask = 0;
		barrier.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;

		sourceStage = VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT;
		destinationStage = VK_PIPELINE_STAGE_TRANSFER_BIT;
	}
	else if (oldLayout == VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL &&
			 newLayout == VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL)
	{
		barrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
		barrier.dstAccessMask = VK_ACCESS_SHADER_READ_BIT;

		sourceStage = VK_PIPELINE_STAGE_TRANSFER_BIT;
		destinationStage = VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT;
	}

	vkCmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, 0, 0, NULL, 0, NULL, 1, &barrier);
	return VK_SUCCESS;
}

VkResult Texture_NewTextureImage(VkDevice device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, int width, int height, uint32 mipmapLevels, VkFormat textureByteFormat)
{
	VkImageCreateInfo ImageCreateInfo =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
		.imageType = VK_IMAGE_TYPE_2D,
		.format = textureByteFormat,
		.extent =
		{
			.width = width,
			.height = height,
			.depth = 1
		},
		.mipLevels = mipmapLevels,
		.arrayLayers = 1,
		.samples = VK_SAMPLE_COUNT_1_BIT,
		.tiling = VK_IMAGE_TILING_LINEAR,
		.usage = VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
		.sharingMode = VK_SHARING_MODE_EXCLUSIVE,
		.initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
	};
	VULKAN_RESULT(vkCreateImage(cRenderer.Device, &ImageCreateInfo, NULL, image));

	VkMemoryRequirements memRequirements;
	vkGetImageMemoryRequirements(cRenderer.Device, *image, &memRequirements);

	VkMemoryAllocateInfo allocInfo =
	{
		.sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
		.allocationSize = memRequirements.size,
		.memoryTypeIndex = Renderer_GetMemoryType(physicalDevice, memRequirements.memoryTypeBits, VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT | VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT)
	};
	VULKAN_RESULT(vkAllocateMemory(device, &allocInfo, NULL, memory));
	return vkBindImageMemory(cRenderer.Device, *image, *memory, 0);
}

VkResult Texture_CreateTextureImage(VkDevice device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, int width, int height, uint32 mipmapLevels, VkFormat textureByteFormat)
{
	VkImageCreateInfo ImageCreateInfo =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
		.imageType = VK_IMAGE_TYPE_2D,
		.format = textureByteFormat,
		.extent = 
		{
			.width = width,
			.height = height,
			.depth = 1
		},
		.mipLevels = mipmapLevels,
		.arrayLayers = 1,
		.samples = VK_SAMPLE_COUNT_1_BIT,
		.tiling = VK_IMAGE_TILING_OPTIMAL,
		.usage = VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
		.sharingMode = VK_SHARING_MODE_EXCLUSIVE,
		.initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
	};
	VULKAN_RESULT(vkCreateImage(cRenderer.Device, &ImageCreateInfo, NULL, image));

	VkMemoryRequirements memRequirements;
	vkGetImageMemoryRequirements(cRenderer.Device, *image, &memRequirements);

	VkMemoryAllocateInfo allocInfo =
	{
		.sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
		.allocationSize = memRequirements.size,
		.memoryTypeIndex = Renderer_GetMemoryType(physicalDevice, memRequirements.memoryTypeBits, VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT)
	};
	VULKAN_RESULT(vkAllocateMemory(device, &allocInfo, NULL, memory));
	return vkBindImageMemory(cRenderer.Device, *image, *memory, 0);
}

VkResult Texture_QuickTransitionImageLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, uint32 mipmapLevels, VkImageLayout* oldLayout, VkImageLayout* newLayout)
{
	VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
	Texture_TransitionImageLayout(commandBuffer, &image, mipmapLevels, *oldLayout, *newLayout);
	VkResult result = Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, commandBuffer);
	if (result == VK_SUCCESS)
	{
		*oldLayout = *newLayout;
	}
	return result;
}

VkResult Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint32 mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout)
{
	Texture_TransitionImageLayout(commandBuffer, &image, mipmapLevels, oldLayout, newLayout);
	return VK_SUCCESS;
}

VkResult Texture_CopyBufferToTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkBuffer buffer, enum TextureUsageEnum textureType, int width, int height, int depth)
{
	VkBufferImageCopy BufferImage = 
	{
		.bufferOffset = 0,
		.bufferRowLength = 0,
		.bufferImageHeight = 0,
		.imageSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
		.imageSubresource.mipLevel = 0,
		.imageSubresource.baseArrayLayer = 0,
		.imageOffset.x = 0,
		.imageOffset.y = 0,
		.imageOffset.z = 0,
		.imageExtent.width = width,
		.imageExtent.height = height,
		.imageExtent.depth = depth,
		.imageSubresource.layerCount = (textureType == kUse_CubeMapTexture ? 6 : 1),
	};
	VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
	vkCmdCopyBufferToImage(commandBuffer, buffer, image, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &BufferImage);
	return Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, commandBuffer);
}

VkResult Texture_GenerateMipmaps(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkFormat* textureByteFormat, uint32 mipmapLevels, int width, int height)
{
	int32_t mipWidth = width;
	int32_t mipHeight = height;

	VkFormatProperties formatProperties;
	vkGetPhysicalDeviceFormatProperties(physicalDevice, *textureByteFormat, &formatProperties);
	if (!(formatProperties.optimalTilingFeatures & VK_FORMAT_FEATURE_SAMPLED_IMAGE_FILTER_LINEAR_BIT))
	{
		RENDERER_ERROR("Texture image format does not support linear blitting.");
	}

	VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
	VkImageMemoryBarrier ImageMemoryBarrier =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
		.image = image,
		.srcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.dstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.subresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
		.subresourceRange.baseArrayLayer = 0,
		.subresourceRange.layerCount = 1,
		.subresourceRange.levelCount = 1,
	};
	for (uint32 x = 1; x < mipmapLevels; x++)
	{
		ImageMemoryBarrier.subresourceRange.baseMipLevel = x - 1;
		ImageMemoryBarrier.oldLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
		ImageMemoryBarrier.newLayout = VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL;
		ImageMemoryBarrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
		ImageMemoryBarrier.dstAccessMask = VK_ACCESS_TRANSFER_READ_BIT;
		vkCmdPipelineBarrier(commandBuffer, VK_PIPELINE_STAGE_TRANSFER_BIT, VK_PIPELINE_STAGE_TRANSFER_BIT, 0, 0, NULL, 0, NULL, 1, &ImageMemoryBarrier);

		VkImageBlit ImageBlit =
		{
			.srcOffsets[0] = { 0, 0, 0 },
			.srcOffsets[1] = { mipWidth, mipHeight, 1 },
			.srcSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
			.srcSubresource.mipLevel = x - 1,
			.srcSubresource.baseArrayLayer = 0,
			.srcSubresource.layerCount = 1,
			.dstOffsets[0] = { 0, 0, 0 },
			.dstOffsets[1] = { mipWidth > 1 ? mipWidth / 2 : 1, mipHeight > 1 ? mipHeight / 2 : 1, 1 },
			.dstSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
			.dstSubresource.mipLevel = x,
			.dstSubresource.baseArrayLayer = 0,
			.dstSubresource.layerCount = 1,
		};
		vkCmdBlitImage(commandBuffer, image, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL, image, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &ImageBlit, VK_FILTER_LINEAR);

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

	ImageMemoryBarrier.subresourceRange.baseMipLevel = mipmapLevels - 1;
	ImageMemoryBarrier.oldLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
	ImageMemoryBarrier.newLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
	ImageMemoryBarrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
	ImageMemoryBarrier.dstAccessMask = VK_ACCESS_SHADER_READ_BIT;

	vkCmdPipelineBarrier(commandBuffer, VK_PIPELINE_STAGE_TRANSFER_BIT, VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT, 0, 0, NULL, 0, NULL, 1, &ImageMemoryBarrier);
	return Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, commandBuffer);
}

VkResult Texture_CreateTextureView(VkDevice device, VkImageView* view, VkImage image, VkFormat format, uint32 mipmapLevels)
{

	VkImageSubresourceRange imageSubresourceRange =
	{
		.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
		.baseMipLevel = 0,
		.levelCount = mipmapLevels,
		.baseArrayLayer = 0,
		.layerCount = 1,
	};

	VkImageViewCreateInfo TextureImageViewInfo =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
		.image = image,
		.viewType = VK_IMAGE_VIEW_TYPE_2D,
		.format = format,
		.subresourceRange = imageSubresourceRange
	};
	return vkCreateImageView(device, &TextureImageViewInfo, NULL, view);
}

VkResult Texture_CreateTextureSampler(VkDevice device, VkSamplerCreateInfo* samplerCreateInfo, VkSampler* smapler)
{
	return vkCreateSampler(device, samplerCreateInfo, NULL, smapler);
}
