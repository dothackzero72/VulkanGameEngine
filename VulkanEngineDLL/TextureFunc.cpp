#include "TextureFunc.h"
#include "CBuffer.h"

#define STB_IMAGE_IMPLEMENTATION
#define STB_IMAGE_WRITE_IMPLEMENTATION
#include <stb/stb_image.h>

TextureStruct Texture_CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue queue, const String& jsonPath)
{
	nlohmann::json json = Json::ReadJson(jsonPath);
	String textureFilePath = json["TextureFilePath"];
	VkImageAspectFlags imageType = json["ImageType"];
	bool useMipMaps = json["UseMipMaps"];

	TextureStruct texture;
	texture.textureId = VkGuid(json["TextureId"].get<String>().c_str());
	texture.textureByteFormat = json["TextureByteFormat"];
	texture.mipMapLevels = useMipMaps ? static_cast<uint32>(std::floor(std::log2(std::max(texture.width, texture.height)))) + 1 : 1;
	texture.textureType = json["TextureType"];

	VkSamplerCreateInfo textureImageSamplerInfo =
	{
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
		.maxLod = (float)1,
		.borderColor = VK_BORDER_COLOR_INT_OPAQUE_BLACK,
		.unnormalizedCoordinates = VK_FALSE,
	};

	Texture_CreateTextureImage2(device, physicalDevice, commandPool, queue, texture, textureFilePath);
	Texture_CreateTextureView2(device, texture, imageType);
	Texture_CreateTextureSampler2(device, textureImageSamplerInfo, texture.textureSampler);
	return texture;
}

TextureStruct Texture_CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo)
{
	TextureStruct texture;
	texture.width = static_cast<int>(createImageInfo.extent.width);
	texture.height = static_cast<int>(createImageInfo.extent.height);
	texture.depth = static_cast<int>(createImageInfo.extent.depth);
	texture.textureByteFormat = createImageInfo.format;
	texture.textureImageLayout = createImageInfo.initialLayout;
	texture.sampleCount = createImageInfo.samples;
	texture.mipMapLevels = 1;

	Texture_CreateTextureImage2(device, physicalDevice, commandPool, graphicsQueue, texture, createImageInfo);
	Texture_CreateTextureView2(device, texture, imageType);
	Texture_CreateTextureSampler2(device, samplerCreateInfo, texture.textureSampler);
	return texture;
}

TextureStruct Texture_CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, const String& texturePath, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps)
{
	TextureStruct texture;
	texture.width = static_cast<int>(createImageInfo.extent.width);
	texture.height = static_cast<int>(createImageInfo.extent.height);
	texture.depth = static_cast<int>(createImageInfo.extent.depth);
	texture.textureByteFormat = createImageInfo.format;
	texture.textureImageLayout = createImageInfo.initialLayout;
	texture.sampleCount = createImageInfo.samples;
	texture.mipMapLevels = useMipMaps ? static_cast<uint32>(std::floor(std::log2(std::max(texture.width, texture.height)))) + 1 : 1;

	Texture_CreateTextureImage2(device, physicalDevice, commandPool, graphicsQueue, texture, texturePath);
	Texture_CreateTextureView2(device, texture, imageType);
	Texture_CreateTextureSampler2(device, samplerCreateInfo, texture.textureSampler);
	return texture;
}

TextureStruct Texture_CreateTexture(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, Pixel& clearColor, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps)
{
	TextureStruct texture;
	texture.width = static_cast<int>(createImageInfo.extent.width);
	texture.height = static_cast<int>(createImageInfo.extent.height);
	texture.depth = static_cast<int>(createImageInfo.extent.depth);
	texture.textureByteFormat = createImageInfo.format;
	texture.textureImageLayout = createImageInfo.initialLayout;
	texture.sampleCount = createImageInfo.samples;
	texture.mipMapLevels = useMipMaps ? static_cast<uint32>(std::floor(std::log2(std::max(texture.width, texture.height)))) + 1 : 1;

	Texture_CreateTextureImage2(device, physicalDevice, commandPool, graphicsQueue, texture, clearColor);
	Texture_CreateTextureView2(device, texture, imageType);
	Texture_CreateTextureSampler2(device, samplerCreateInfo, texture.textureSampler);
	return texture;
}

void Texture_UpdateTextureBufferIndex(TextureStruct& texture, uint32 bufferIndex)
{
	texture.textureBufferIndex = bufferIndex;
}

void Texture_GetTexturePropertiesBuffer(TextureStruct& texture, std::vector<VkDescriptorImageInfo>& textureDescriptorList)
{
	VkDescriptorImageInfo textureDescriptor =
	{
		.sampler = texture.textureSampler,
		.imageView = texture.textureView,
		.imageLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL
	};
	textureDescriptorList.emplace_back(textureDescriptor);
}

void Texture_DestroyTexture(VkDevice device, TextureStruct& texture)
{
	Renderer_DestroyImageView(device, &texture.textureView);
	Renderer_DestroySampler(device, &texture.textureSampler);
	Renderer_DestroyImage(device, &texture.textureImage);
	Renderer_FreeDeviceMemory(device, &texture.textureMemory);
}

//void Texture_CreateImageTexture2(VkDevice device,
//	VkPhysicalDevice physicalDevice,
//	VkCommandPool commandPool,
//	VkQueue graphicsQueue,
//	TextureStruct& texture,
//	const Pixel& clearColor,
//	bool usingMipMap)
//{
//	VkBuffer buffer = VK_NULL_HANDLE;
//	VkBuffer stagingBuffer = VK_NULL_HANDLE;
//	VkDeviceMemory stagingBufferMemory = VK_NULL_HANDLE;
//	VkDeviceMemory bufferMemory = VK_NULL_HANDLE;
//
//	VkDeviceSize bufferSize = texture.Width * texture.Height * texture.ColorChannels);
//	VkMemoryPropertyFlags bufferProperties = VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;
//	VkBufferUsageFlags bufferUsage = VK_BUFFER_USAGE_TRANSFER_SRC_BIT;
//
//	std::vector<Pixel> pixels(texture.Width * texture.Height, clearColor);
//	Buffer_CreateStagingBuffer(device, physicalDevice, commandPool, graphicsQueue, &stagingBuffer, &buffer, &stagingBufferMemory, &bufferMemory, (void*)pixels.data(), bufferSize, bufferUsage, bufferProperties);
//
//	VkImageCreateInfo imageCreateInfo = {
//		.sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
//		.imageType = VK_IMAGE_TYPE_2D,
//		.format = texture.TextureByteFormat,
//		.extent = VkExtent3D
//		{
//			.width = static_cast<uint32_t>(texture.Width), 
//			.height = static_cast<uint32_t>(texture.Height), 
//			.depth = 1 
//	},
//		.mipLevels = texture.MipMapLevels,
//		.arrayLayers = 1,
//		.samples = VK_SAMPLE_COUNT_1_BIT,
//		.tiling = VK_IMAGE_TILING_OPTIMAL,
//		.usage = VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
//		.sharingMode = VK_SHARING_MODE_EXCLUSIVE,
//		.initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
//	};
//
//	VkImageLayout newTextureLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
//	VULKAN_RESULT(Texture_CreateImage(device, physicalDevice, &texture.Image, &texture.Memory, imageCreateInfo));
//	VULKAN_RESULT(Texture_QuickTransitionImageLayout(device, commandPool, graphicsQueue, texture.Image, texture.MipMapLevels, &texture.TextureImageLayout, &newTextureLayout));
//	Texture_CopyBufferToTexture(device, commandPool, graphicsQueue, texture.Image, texture.TextureByteFormat, texture.TextureUsage, texture.Width, texture.Height, texture.Depth);
//	Texture_GenerateMipmaps(device, physicalDevice, commandPool, graphicsQueue, texture.Image, texture.TextureByteFormat, texture.mipmapLevels, *width, *height, usingMipMap);
//
//	if (stagingBuffer != VK_NULL_HANDLE) {
//		vkDestroyBuffer(device, stagingBuffer, NULL);
//		stagingBuffer = VK_NULL_HANDLE;
//	}
//
//	if (stagingBufferMemory != VK_NULL_HANDLE) {
//		Renderer_FreeDeviceMemory(device, &stagingBufferMemory);
//		stagingBufferMemory = VK_NULL_HANDLE;
//	}
//}

void Texture_CreateTextureImage2(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, TextureStruct& texture, const Pixel& clearColor)
{
	VkBuffer buffer = VK_NULL_HANDLE;
	VkBuffer stagingBuffer = VK_NULL_HANDLE;
	VkDeviceMemory stagingBufferMemory = VK_NULL_HANDLE;
	VkDeviceMemory bufferMemory = VK_NULL_HANDLE;

	VkDeviceSize bufferSize = texture.width * texture.height * texture.colorChannels;
	VkMemoryPropertyFlags bufferProperties = VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;
	VkBufferUsageFlags bufferUsage = VK_BUFFER_USAGE_TRANSFER_SRC_BIT;

	std::vector<Pixel> pixels(texture.width * texture.height, clearColor);
	Buffer_CreateStagingBuffer(device, physicalDevice, commandPool, graphicsQueue, &stagingBuffer, &buffer, &stagingBufferMemory, &bufferMemory, (void*)pixels.data(), bufferSize, bufferUsage, bufferProperties);

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
	VULKAN_RESULT(Texture_CreateImage2(device, physicalDevice, texture, imageCreateInfo));
	VULKAN_RESULT(Texture_QuickTransitionImageLayout2(device, commandPool, graphicsQueue, texture, newTextureLayout));
	VULKAN_RESULT(Texture_CopyBufferToTexture2(device, commandPool, graphicsQueue, texture, buffer));
	VULKAN_RESULT(Texture_GenerateMipmaps2(device, physicalDevice, commandPool, graphicsQueue, texture));

	Renderer_DestroyBuffer(device, &stagingBuffer);
	Renderer_FreeDeviceMemory(device, &stagingBufferMemory);
}

void Texture_CreateTextureImage2(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, TextureStruct& texture, const String& filePath)
{
	VkBuffer buffer = VK_NULL_HANDLE;
	VkBuffer stagingBuffer = VK_NULL_HANDLE;
	VkDeviceMemory stagingBufferMemory = VK_NULL_HANDLE;
	VkDeviceMemory bufferMemory = VK_NULL_HANDLE;

	byte* data = stbi_load(filePath.c_str(), &texture.width, &texture.height, (int*)&texture.colorChannels, 0);
	VkDeviceSize bufferSize = texture.width * texture.height * texture.colorChannels;
	VkBufferUsageFlags bufferUsage = VK_BUFFER_USAGE_TRANSFER_SRC_BIT | VK_BUFFER_USAGE_TRANSFER_DST_BIT;
	VkMemoryPropertyFlags bufferProperties = VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;

	Buffer_CreateStagingBuffer(device, physicalDevice, commandPool, graphicsQueue, &stagingBuffer, &buffer, &stagingBufferMemory, &bufferMemory, data, bufferSize, bufferUsage, bufferProperties);

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
	VULKAN_RESULT(Texture_CreateImage2(device, physicalDevice, texture, imageCreateInfo));
	VULKAN_RESULT(Texture_QuickTransitionImageLayout2(device, commandPool, graphicsQueue, texture, newTextureLayout));
	VULKAN_RESULT(Texture_CopyBufferToTexture2(device, commandPool, graphicsQueue, texture, buffer));
	VULKAN_RESULT(Texture_GenerateMipmaps2(device, physicalDevice, commandPool, graphicsQueue, texture));

	Renderer_DestroyBuffer(device, &stagingBuffer);
	Renderer_FreeDeviceMemory(device, &stagingBufferMemory);
	stbi_image_free(data);
}

void Texture_CreateTextureImage2(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, TextureStruct& texture, VkImageCreateInfo& createImageInfo)
{
	VULKAN_RESULT(vkCreateImage(device, &createImageInfo, nullptr, &texture.textureImage));

	VkMemoryRequirements memRequirements;
	vkGetImageMemoryRequirements(device, texture.textureImage, &memRequirements);

	VkMemoryAllocateInfo allocInfo =
	{
		.sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
		.allocationSize = memRequirements.size,
		.memoryTypeIndex = Renderer_GetMemoryType(physicalDevice, memRequirements.memoryTypeBits, VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT)
	};
	VULKAN_RESULT(vkAllocateMemory(device, &allocInfo, nullptr, &texture.textureMemory));
	VULKAN_RESULT(vkBindImageMemory(device, texture.textureImage, texture.textureMemory, 0));
}

void Texture_CreateImageTexture(VkDevice device,
    VkPhysicalDevice physicalDevice,
    VkCommandPool commandPool,
    VkQueue graphicsQueue,
    int* width,
    int* height,
    int* depth,
    VkFormat textureByteFormat,
    uint mipmapLevels,
    VkImage* textureImage,
    VkDeviceMemory* textureMemory,
    VkImageLayout* textureImageLayout,
    enum ColorChannelUsed* colorChannelUsed,
    enum TextureUsageEnum textureUsage,
    const Pixel& clearColor,
    bool usingMipMap)
{
    VkBuffer buffer = VK_NULL_HANDLE;
    VkBuffer stagingBuffer = VK_NULL_HANDLE;
    VkDeviceMemory stagingBufferMemory = VK_NULL_HANDLE;
    VkDeviceMemory bufferMemory = VK_NULL_HANDLE;

    VkDeviceSize bufferSize = (*width) * (*height) * (*colorChannelUsed);
    VkMemoryPropertyFlags bufferProperties = VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;
    VkBufferUsageFlags bufferUsage = VK_BUFFER_USAGE_TRANSFER_SRC_BIT;

    std::vector<Pixel> pixels((*width) * (*height), clearColor);
    Buffer_CreateStagingBuffer(device, physicalDevice, commandPool, graphicsQueue, &stagingBuffer, &buffer, &stagingBufferMemory, &bufferMemory, (void*)pixels.data(), bufferSize, bufferUsage, bufferProperties);

    VkImageCreateInfo imageCreateInfo = {
        .sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
        .imageType = VK_IMAGE_TYPE_2D,
        .format = textureByteFormat,
        .extent = {.width = static_cast<uint32_t>(*width), .height = static_cast<uint32_t>(*height), .depth = 1 },
        .mipLevels = mipmapLevels,
        .arrayLayers = 1,
        .samples = VK_SAMPLE_COUNT_1_BIT,
        .tiling = VK_IMAGE_TILING_OPTIMAL,
        .usage = VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
        .sharingMode = VK_SHARING_MODE_EXCLUSIVE,
        .initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
    };

    VkImageLayout newTextureLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
    VULKAN_RESULT(Texture_CreateImage(device, physicalDevice, textureImage, textureMemory, imageCreateInfo));
    VULKAN_RESULT(Texture_QuickTransitionImageLayout(device, commandPool, graphicsQueue, *textureImage, mipmapLevels, textureImageLayout, &newTextureLayout));
    VULKAN_RESULT(Texture_CopyBufferToTexture(device, commandPool, graphicsQueue, *textureImage, buffer, textureUsage, *width, *height, *depth));
    VULKAN_RESULT(Texture_GenerateMipmaps(device, physicalDevice, commandPool, graphicsQueue, *textureImage, &textureByteFormat, mipmapLevels, *width, *height, usingMipMap));

    if (stagingBuffer != VK_NULL_HANDLE) {
        vkDestroyBuffer(device, stagingBuffer, NULL);
        stagingBuffer = VK_NULL_HANDLE;
    }

    if (stagingBufferMemory != VK_NULL_HANDLE) {
        Renderer_FreeDeviceMemory(device, &stagingBufferMemory);
        stagingBufferMemory = VK_NULL_HANDLE;
    }
}

void Texture_CreateImageTexture(VkDevice device,
    VkPhysicalDevice physicalDevice,
    VkCommandPool commandPool,
    VkQueue graphicsQueue,
    int* width,
    int* height,
    int* depth,
    VkFormat textureByteFormat,
    uint mipmapLevels,
    VkImage* textureImage,
    VkDeviceMemory* textureMemory,
    VkImageLayout* textureImageLayout,
    enum ColorChannelUsed* colorChannelUsed,
    enum TextureUsageEnum textureUsage,
    const String& filePath,
    bool usingMipMap)
{
    VkBuffer buffer = VK_NULL_HANDLE;
    VkBuffer stagingBuffer = VK_NULL_HANDLE;
    VkDeviceMemory stagingBufferMemory = VK_NULL_HANDLE;
    VkDeviceMemory bufferMemory = VK_NULL_HANDLE;

    byte* data = stbi_load(filePath.c_str(), width, height, (int*)colorChannelUsed, 0);

    VkDeviceSize bufferSize = (*width) * (*height) * (*colorChannelUsed);
    VkBufferUsageFlags bufferUsage = VK_BUFFER_USAGE_TRANSFER_SRC_BIT | VK_BUFFER_USAGE_TRANSFER_DST_BIT;
    VkMemoryPropertyFlags bufferProperties = VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;

    Buffer_CreateStagingBuffer(device, physicalDevice, commandPool, graphicsQueue, &stagingBuffer, &buffer, &stagingBufferMemory, &bufferMemory, data, bufferSize, bufferUsage, bufferProperties);

    VkImageCreateInfo imageCreateInfo = {
        .sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
        .imageType = VK_IMAGE_TYPE_2D,
        .format = textureByteFormat,
        .extent = {.width = static_cast<uint32_t>(*width), .height = static_cast<uint32_t>(*height), .depth = 1 },
        .mipLevels = mipmapLevels,
        .arrayLayers = 1,
        .samples = VK_SAMPLE_COUNT_1_BIT,
        .tiling = VK_IMAGE_TILING_OPTIMAL,
        .usage = VK_IMAGE_USAGE_TRANSFER_SRC_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
        .sharingMode = VK_SHARING_MODE_EXCLUSIVE,
        .initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
    };

    VkImageLayout newTextureLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
    VULKAN_RESULT(Texture_CreateImage(device, physicalDevice, textureImage, textureMemory, imageCreateInfo));
    VULKAN_RESULT(Texture_QuickTransitionImageLayout(device, commandPool, graphicsQueue, *textureImage, mipmapLevels, textureImageLayout, &newTextureLayout));
    VULKAN_RESULT(Texture_CopyBufferToTexture(device, commandPool, graphicsQueue, *textureImage, buffer, textureUsage, *width, *height, *depth));
    VULKAN_RESULT(Texture_GenerateMipmaps(device, physicalDevice, commandPool, graphicsQueue, *textureImage, &textureByteFormat, mipmapLevels, *width, *height, usingMipMap));

    if (stagingBuffer != VK_NULL_HANDLE) {
        vkDestroyBuffer(device, stagingBuffer, NULL);
        stagingBuffer = VK_NULL_HANDLE;
    }

    if (stagingBufferMemory != VK_NULL_HANDLE) {
        Renderer_FreeDeviceMemory(device, &stagingBufferMemory);
        stagingBufferMemory = VK_NULL_HANDLE;
    }
    stbi_image_free(data);
}

//void Texture_SaveTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, const char* filename, SharedPtr<Texture> texture, ExportTextureFormat textureFormat, uint32 channels)
//{
//    // Implementation as provided in the original code
//}

void Texture_UpdateTextureLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkImageLayout* oldImageLayout, VkImageLayout* newImageLayout, uint32 mipLevel)
{
	VkImageMemoryBarrier imageMemoryBarrier =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
		.srcAccessMask = 0,
		.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT,
		.oldLayout = *oldImageLayout,
		.newLayout = *newImageLayout,
		.image = image,
		.subresourceRange =
		{
			.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
			.baseMipLevel = mipLevel,
			.levelCount = VK_REMAINING_MIP_LEVELS,
			.layerCount = 1
		}
	};

	auto singleCommand = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
	vkCmdPipelineBarrier(singleCommand, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, NULL, 0, NULL, 1, &imageMemoryBarrier);
	VkResult result = Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, singleCommand);
	if (result == VK_SUCCESS)
	{
		*oldImageLayout = *newImageLayout;
	}
}

void Texture_UpdateCmdTextureLayout(VkCommandBuffer* commandBuffer, VkImage image, VkImageLayout* oldImageLayout, VkImageLayout* newImageLayout, uint32 mipLevel)
{
	VkImageMemoryBarrier imageMemoryBarrier =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
		.srcAccessMask = 0,
		.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT,
		.oldLayout = *oldImageLayout,
		.newLayout = *newImageLayout,
		.image = image,
		.subresourceRange =
		{
			.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
			.baseMipLevel = mipLevel,
			.levelCount = VK_REMAINING_MIP_LEVELS,
			.layerCount = 1
		}
	};

	vkCmdPipelineBarrier(*commandBuffer, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, NULL, 0, NULL, 1, &imageMemoryBarrier);
	*oldImageLayout = *newImageLayout;
}

VkResult Texture_TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage* image, uint32 mipmapLevels, VkImageLayout* oldLayout, VkImageLayout newLayout)
{
	VkPipelineStageFlags sourceStage = VK_PIPELINE_STAGE_FLAG_BITS_MAX_ENUM;
	VkPipelineStageFlags destinationStage = VK_PIPELINE_STAGE_FLAG_BITS_MAX_ENUM;
	VkImageMemoryBarrier barrier =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
		.oldLayout = *oldLayout,
		.newLayout = newLayout,
		.srcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.dstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.image = *image,
		.subresourceRange = VkImageSubresourceRange
		{
			.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
			.baseMipLevel = 0,
			.levelCount = mipmapLevels,
			.baseArrayLayer = 0,
			.layerCount = VK_REMAINING_ARRAY_LAYERS,
		}
	};
	if (*oldLayout == VK_IMAGE_LAYOUT_UNDEFINED &&
		newLayout == VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL)
	{
		barrier.srcAccessMask = 0;
		barrier.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;

		sourceStage = VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT;
		destinationStage = VK_PIPELINE_STAGE_TRANSFER_BIT;
	}
	else if (*oldLayout == VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL &&
		newLayout == VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL)
	{
		barrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
		barrier.dstAccessMask = VK_ACCESS_SHADER_READ_BIT;

		sourceStage = VK_PIPELINE_STAGE_TRANSFER_BIT;
		destinationStage = VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT;
	}

	vkCmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, 0, 0, NULL, 0, NULL, 1, &barrier);
	*oldLayout = newLayout;
	return VK_SUCCESS;
}

VkResult Texture_CreateImage(VkDevice device, VkPhysicalDevice physicalDevice, VkImage* image, VkDeviceMemory* memory, VkImageCreateInfo imageCreateInfo)
{
	VULKAN_RESULT(vkCreateImage(device, &imageCreateInfo, NULL, image));

	VkMemoryRequirements memRequirements;
	vkGetImageMemoryRequirements(device, *image, &memRequirements);

	VkMemoryAllocateInfo allocInfo =
	{
		.sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
		.allocationSize = memRequirements.size,
		.memoryTypeIndex = Renderer_GetMemoryType(physicalDevice, memRequirements.memoryTypeBits, VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT)
	};
	VULKAN_RESULT(vkAllocateMemory(device, &allocInfo, NULL, memory));
	return vkBindImageMemory(device, *image, *memory, 0);
}

VkResult Texture_QuickTransitionImageLayout(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, uint32 mipmapLevels, VkImageLayout* oldLayout, VkImageLayout* newLayout)
{
	VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
	Texture_TransitionImageLayout(commandBuffer, &image, mipmapLevels, oldLayout, *newLayout);
	VkResult result = Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, commandBuffer);
	if (result == VK_SUCCESS)
	{
		*oldLayout = *newLayout;
	}
	return result;
}

VkResult Texture_CommandBufferTransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint32 mipmapLevels, VkImageLayout* oldLayout, VkImageLayout newLayout)
{
	Texture_TransitionImageLayout(commandBuffer, &image, mipmapLevels, oldLayout, newLayout);
	return VK_SUCCESS;
}

VkResult Texture_CopyBufferToTexture(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkBuffer buffer, enum TextureUsageEnum textureType, uint width, uint height, uint depth)
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
			.layerCount = static_cast<uint>((textureType == kUse_CubeMapTexture ? 6 : 1))
		},
		.imageOffset = VkOffset3D
		{
			.x = 0,
			.y = 0,
			.z = 0,
		},
		.imageExtent = VkExtent3D
		{
			.width = width,
			.height = height,
			.depth = depth,
		}
	};
	VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
	vkCmdCopyBufferToImage(commandBuffer, buffer, image, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &BufferImage);
	return Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, commandBuffer);
}

VkResult Texture_CreateTextureSampler(VkDevice device, VkSamplerCreateInfo* samplerCreateInfo, VkSampler* smapler)
{
	return vkCreateSampler(device, samplerCreateInfo, NULL, smapler);
}

VkResult Texture_GenerateMipmaps(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, VkImage image, VkFormat* textureByteFormat, uint32 mipmapLevels, int width, int height, bool usingMipMaps)
{
	if (!usingMipMaps)
	{
		return VK_SUCCESS;
	}

	int32 mipWidth = width;
	int32 mipHeight = height;

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
		.srcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.dstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
		.image = image,
		.subresourceRange = VkImageSubresourceRange
		{
			.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
			.levelCount = 1,
			.baseArrayLayer = 0,
			.layerCount = 1,
		}
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

VkResult Texture_CreateTextureView(VkDevice device, VkImageView* view, VkImage image, VkFormat format, VkImageAspectFlags imageType, uint32 mipmapLevels)
{
	VkImageViewCreateInfo TextureImageViewInfo =
	{
		.sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
		.image = image,
		.viewType = VK_IMAGE_VIEW_TYPE_2D,
		.format = format,
		.subresourceRange =
		{
			.aspectMask = imageType,
			.baseMipLevel = 0,
			.levelCount = mipmapLevels,
			.baseArrayLayer = 0,
			.layerCount = 1,
		}
	};
	return vkCreateImageView(device, &TextureImageViewInfo, NULL, view);
}
//////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////


VkResult Texture_CreateImage2(VkDevice device, VkPhysicalDevice physicalDevice, TextureStruct& texture, VkImageCreateInfo& imageCreateInfo)
{
	VULKAN_RESULT(vkCreateImage(device, &imageCreateInfo, NULL, &texture.textureImage));

	VkMemoryRequirements memRequirements;
	vkGetImageMemoryRequirements(device, texture.textureImage, &memRequirements);

	VkMemoryAllocateInfo allocInfo =
	{
		.sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
		.allocationSize = memRequirements.size,
		.memoryTypeIndex = Renderer_GetMemoryType(physicalDevice, memRequirements.memoryTypeBits, VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT)
	};
	VULKAN_RESULT(vkAllocateMemory(device, &allocInfo, NULL, &texture.textureMemory));
	return vkBindImageMemory(device, texture.textureImage, texture.textureMemory, 0);
}

VkResult Texture_CreateTextureView2(VkDevice device, TextureStruct& texture, VkImageAspectFlags imageAspectFlags)
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
	return vkCreateImageView(device, &TextureImageViewInfo, NULL, &texture.textureView);
}

VkResult Texture_CreateTextureSampler2(VkDevice device, VkSamplerCreateInfo& samplerCreateInfo, VkSampler& smapler)
{
	return vkCreateSampler(device, &samplerCreateInfo, NULL, &smapler);
}

VkResult Texture_TransitionImageLayout2(VkCommandBuffer commandBuffer, TextureStruct& texture, VkImageLayout newLayout)
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

VkResult Texture_CommandBufferTransitionImageLayout2(VkCommandBuffer commandBuffer, TextureStruct& texture, VkImageLayout newLayout)
{
	Texture_TransitionImageLayout2(commandBuffer, texture, newLayout);
	return VK_SUCCESS;
}

void Texture_UpdateCmdTextureLayout2(VkCommandBuffer& commandBuffer, TextureStruct& texture, VkImageLayout& newImageLayout)
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

void Texture_UpdateTextureLayout2(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, TextureStruct& texture, VkImageLayout& newImageLayout)
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

	auto singleCommand = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
	vkCmdPipelineBarrier(singleCommand, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, VK_PIPELINE_STAGE_ALL_COMMANDS_BIT, 0, 0, NULL, 0, NULL, 1, &imageMemoryBarrier);
	VkResult result = Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, singleCommand);
	if (result == VK_SUCCESS)
	{
		texture.textureImageLayout = newImageLayout;
	}
}

VkResult Texture_QuickTransitionImageLayout2(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, TextureStruct& texture, VkImageLayout& newLayout)
{
	VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
	Texture_TransitionImageLayout2(commandBuffer, texture, newLayout);
	VkResult result = Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, commandBuffer);
	if (result == VK_SUCCESS)
	{
		texture.textureImageLayout = newLayout;
	}
	return result;
}

VkResult Texture_CopyBufferToTexture2(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, TextureStruct& texture, VkBuffer buffer)
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
	VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
	vkCmdCopyBufferToImage(commandBuffer, buffer, texture.textureImage, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &BufferImage);
	return Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, commandBuffer);
}

VkResult Texture_GenerateMipmaps2(VkDevice device, VkPhysicalDevice physicalDevice, VkCommandPool commandPool, VkQueue graphicsQueue, TextureStruct& texture)
{
	if (texture.mipMapLevels == 1)
	{
		return VK_SUCCESS;
	}

	int32 mipWidth = texture.width;
	int32 mipHeight = texture.height;

	VkFormatProperties formatProperties;
	vkGetPhysicalDeviceFormatProperties(physicalDevice, texture.textureByteFormat, &formatProperties);
	if (!(formatProperties.optimalTilingFeatures & VK_FORMAT_FEATURE_SAMPLED_IMAGE_FILTER_LINEAR_BIT))
	{
		RENDERER_ERROR("Texture image format does not support linear blitting.");
	}

	VkCommandBuffer commandBuffer = Renderer_BeginSingleUseCommandBuffer(device, commandPool);
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
	return Renderer_EndSingleUseCommandBuffer(device, commandPool, graphicsQueue, commandBuffer);
}