#include "CTexture.h"
#include "VulkanRenderer.h"

void Texture_CreateTextureImage(struct TextureInfo* textureInfo)
{
	VkImageCreateInfo ImageCreateInfo =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
		.imageType = VK_IMAGE_TYPE_2D,
		.format = *textureInfo->TextureByteFormat,
		.extent = 
		{
			.width = *textureInfo->Width,
			.height = *textureInfo->Height,
			.depth = 1
		},
		.mipLevels = *textureInfo->MipMapLevels,
		.arrayLayers = 1,
		.samples = VK_SAMPLE_COUNT_1_BIT,
		.tiling = VK_IMAGE_TILING_OPTIMAL,
		.usage = VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
		.sharingMode = VK_SHARING_MODE_EXCLUSIVE,
		.initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
	};
	VULKAN_RESULT(vkCreateImage(Renderer.Device, &ImageCreateInfo, NULL, textureInfo->Image));

	VkMemoryRequirements memRequirements;
	vkGetImageMemoryRequirements(Renderer.Device, *textureInfo->Image, &memRequirements);

	VkMemoryAllocateInfo allocInfo =
	{
		.sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
		.allocationSize = memRequirements.size,
		.memoryTypeIndex = Renderer_GetMemoryType(memRequirements.memoryTypeBits, VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT)
	};
	VULKAN_RESULT(vkAllocateMemory(Renderer.Device, &allocInfo, NULL, textureInfo->Memory));
	VULKAN_RESULT(vkBindImageMemory(Renderer.Device, *textureInfo->Image, *textureInfo->Memory, 0));
}

void Texture_QuickTransitionImageLayout(struct TextureInfo* textureInfo, VkImageLayout newImageLayout)
{
	VkPipelineStageFlags sourceStage = VK_PIPELINE_STAGE_FLAG_BITS_MAX_ENUM;
	VkPipelineStageFlags destinationStage = VK_PIPELINE_STAGE_FLAG_BITS_MAX_ENUM;
	VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer();

	VkImageMemoryBarrier barrier =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
		.oldLayout = *textureInfo->TextureImageLayout,
		.newLayout = newImageLayout,
		.srcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.dstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.image = *textureInfo->Image,
		.subresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
		.subresourceRange.baseMipLevel = 0,
		.subresourceRange.levelCount = *textureInfo->MipMapLevels,
		.subresourceRange.baseArrayLayer = 0,
		.subresourceRange.layerCount = VK_REMAINING_ARRAY_LAYERS,
	};
	if (*textureInfo->TextureImageLayout == VK_IMAGE_LAYOUT_UNDEFINED &&
		newImageLayout == VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL)
	{
		barrier.srcAccessMask = 0;
		barrier.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;

		sourceStage = VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT;
		destinationStage = VK_PIPELINE_STAGE_TRANSFER_BIT;
	}
	else if (*textureInfo->TextureImageLayout == VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL && 
			 newImageLayout == VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL)
	{
		barrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
		barrier.dstAccessMask = VK_ACCESS_SHADER_READ_BIT;

		sourceStage = VK_PIPELINE_STAGE_TRANSFER_BIT;
		destinationStage = VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT;
	}

	vkCmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, 0, 0, NULL, 0, NULL, 1, &barrier);
	VULKAN_RESULT(Renderer_EndSingleUseCommandBuffer(commandBuffer));
}

void Texture_CommandBufferTransitionImageLayout(VkCommandBuffer* commandBuffer, struct TextureInfo* textureInfo, VkImageLayout newImageLayout)
{
	VkPipelineStageFlags sourceStage = VK_PIPELINE_STAGE_FLAG_BITS_MAX_ENUM;
	VkPipelineStageFlags destinationStage = VK_PIPELINE_STAGE_FLAG_BITS_MAX_ENUM;

	VkImageMemoryBarrier barrier =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
		.oldLayout = *textureInfo->TextureImageLayout,
		.newLayout = newImageLayout,
		.srcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.dstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.image = *textureInfo->Image,
		.subresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
		.subresourceRange.baseMipLevel = 0,
		.subresourceRange.levelCount = *textureInfo->MipMapLevels,
		.subresourceRange.baseArrayLayer = 0,
		.subresourceRange.layerCount = VK_REMAINING_ARRAY_LAYERS,
	};
	if (*textureInfo->TextureImageLayout == VK_IMAGE_LAYOUT_UNDEFINED &&
		newImageLayout == VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL)
	{
		barrier.srcAccessMask = 0;
		barrier.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;

		sourceStage = VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT;
		destinationStage = VK_PIPELINE_STAGE_TRANSFER_BIT;
	}
	else if (*textureInfo->TextureImageLayout == VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL &&
		newImageLayout == VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL)
	{
		barrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
		barrier.dstAccessMask = VK_ACCESS_SHADER_READ_BIT;

		sourceStage = VK_PIPELINE_STAGE_TRANSFER_BIT;
		destinationStage = VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT;
	}

	vkCmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, 0, 0, NULL, 0, NULL, 1, &barrier);
}

void Texture_CopyBufferToTexture(struct TextureInfo* textureInfo, VkBuffer* buffer)
{
	VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer();
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
		.imageExtent.width = *textureInfo->Width,
		.imageExtent.height = *textureInfo->Height,
		.imageExtent.depth = *textureInfo->Depth,
		.imageSubresource.layerCount = 1,
	};
	if (*textureInfo->TextureType == kUse_CubeMapTexture)
	{
		BufferImage.imageSubresource.layerCount = 6;
	}
	vkCmdCopyBufferToImage(commandBuffer, *buffer, *textureInfo->Image, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &BufferImage);
	VULKAN_RESULT(Renderer_EndSingleUseCommandBuffer(commandBuffer));
}

void Texture_GenerateMipmaps(struct TextureInfo* textureInfo)
{
	if (*textureInfo->MipMapLevels > (uint32)1)
	{
		int32_t mipWidth = *textureInfo->Width;
		int32_t mipHeight = *textureInfo->Height;

		VkFormatProperties formatProperties;
		vkGetPhysicalDeviceFormatProperties(Renderer.PhysicalDevice, *textureInfo->TextureByteFormat, &formatProperties);
		if (!(formatProperties.optimalTilingFeatures & VK_FORMAT_FEATURE_SAMPLED_IMAGE_FILTER_LINEAR_BIT))
		{
			//throw std::runtime_error("Texture image format does not support linear blitting.");
		}

		VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer();
		VkImageMemoryBarrier ImageMemoryBarrier =
		{
			.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
			.image = *textureInfo->Image,
			.srcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
			.dstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
			.subresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
			.subresourceRange.baseArrayLayer = 0,
			.subresourceRange.layerCount = 1,
			.subresourceRange.levelCount = 1,
		};
		for (uint32 x = 1; x < *textureInfo->MipMapLevels; x++)
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
			vkCmdBlitImage(commandBuffer, *textureInfo->Image, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL, *textureInfo->Image, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &ImageBlit, VK_FILTER_LINEAR);

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

		ImageMemoryBarrier.subresourceRange.baseMipLevel = *textureInfo->MipMapLevels - 1;
		ImageMemoryBarrier.oldLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
		ImageMemoryBarrier.newLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
		ImageMemoryBarrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
		ImageMemoryBarrier.dstAccessMask = VK_ACCESS_SHADER_READ_BIT;

		vkCmdPipelineBarrier(commandBuffer, VK_PIPELINE_STAGE_TRANSFER_BIT, VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT, 0, 0, NULL, 0, NULL, 1, &ImageMemoryBarrier);
		VULKAN_RESULT(Renderer_EndSingleUseCommandBuffer(commandBuffer));
		
		*textureInfo->TextureImageLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
	}
}

VkResult Texture_CreateTextureView(struct TextureInfo* textureInfo, VkImageViewCreateInfo* imageViewCreateInfo)
{
	return vkCreateImageView(Renderer.Device, imageViewCreateInfo, NULL, textureInfo->View);
}

VkResult Texture_CreateTextureSampler(struct TextureInfo* textureInfo, VkSamplerCreateInfo* samplerCreateInfo)
{
	return vkCreateSampler(Renderer.Device, samplerCreateInfo, NULL, textureInfo->Sampler);
}
