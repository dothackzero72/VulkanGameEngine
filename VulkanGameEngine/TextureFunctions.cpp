#include "TextureFunctions.h"

VkResult TextureFunctions::CreateTextureImage(Texture& texture)
{
	return Texture_CreateTextureImage(renderer.Device, renderer.PhysicalDevice, &texture.Image, &texture.Memory, texture.Width, texture.Height, texture.MipMapLevels, texture.TextureByteFormat);
}

VkResult TextureFunctions::CreateTextureImage(Texture* texture)
{
	return Texture_CreateTextureImage(renderer.Device, renderer.PhysicalDevice, &texture->Image, &texture->Memory, texture->Width, texture->Height, texture->MipMapLevels, texture->TextureByteFormat);
}

VkResult TextureFunctions::CreateTextureImage(std::shared_ptr<Texture> texture)
{
	return Texture_CreateTextureImage(renderer.Device, renderer.PhysicalDevice, &texture->Image, &texture->Memory, texture->Width, texture->Height, texture->MipMapLevels, texture->TextureByteFormat);
}

VkResult TextureFunctions::CreateTextureImage(VkImage& image, VkDeviceMemory memory, vec2 textureSize, uint32 mipmapLevels, VkFormat textureByteFormat)
{
	return Texture_CreateTextureImage(renderer.Device, renderer.PhysicalDevice, &image, &memory, static_cast<int>(textureSize.x), static_cast<int>(textureSize.y), mipmapLevels, textureByteFormat);
}

VkResult TextureFunctions::CreateTextureImage(VkImage& image, VkDeviceMemory memory, ivec2 textureSize, uint32 mipmapLevels, VkFormat textureByteFormat)
{
	return Texture_CreateTextureImage(renderer.Device, renderer.PhysicalDevice, &image, &memory, textureSize.x, textureSize.y, mipmapLevels, textureByteFormat);
}

VkResult TextureFunctions::CreateTextureImage(VkImage& image, VkDeviceMemory memory, int width, int height, uint32 mipmapLevels, VkFormat textureByteFormat)
{
	return Texture_CreateTextureImage(renderer.Device, renderer.PhysicalDevice, &image, &memory, width, height, mipmapLevels, textureByteFormat);
}

VkResult TextureFunctions::CreateTextureView(Texture& texture)
{
	return Texture_CreateTextureView(renderer.Device, &texture.View, texture.Image, texture.TextureByteFormat, texture.MipMapLevels);
}

VkResult TextureFunctions::CreateTextureView(Texture* texture)
{
	return Texture_CreateTextureView(renderer.Device, &texture->View, texture->Image, texture->TextureByteFormat, texture->MipMapLevels);
}

VkResult TextureFunctions::CreateTextureView(std::shared_ptr<Texture> texture)
{
	return Texture_CreateTextureView(renderer.Device, &texture->View, texture->Image, texture->TextureByteFormat, texture->MipMapLevels);
}

VkResult TextureFunctions::CreateTextureView(VkImageView& view, VkImage image, VkFormat format, uint32 mipmapLevels)
{
	return Texture_CreateTextureView(renderer.Device, &view, image, format, mipmapLevels);
}

VkResult TextureFunctions::CreateTextureSampler(VkSamplerCreateInfo* samplerCreateInfo, VkSampler* smapler)
{
	return Texture_CreateTextureSampler(renderer.Device, samplerCreateInfo, smapler);
}

VkResult TextureFunctions::TransitionImageLayout(Texture texture, VkImageLayout newLayout)
{
	return Texture_QuickTransitionImageLayout(renderer.Device, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, texture.Image, texture.MipMapLevels, &texture.TextureImageLayout, &newLayout);
}

VkResult TextureFunctions::TransitionImageLayout(Texture* texture, VkImageLayout newLayout)
{
	return Texture_QuickTransitionImageLayout(renderer.Device, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, texture->Image, texture->MipMapLevels, &texture->TextureImageLayout, &newLayout);
}

VkResult TextureFunctions::TransitionImageLayout(std::shared_ptr<Texture> texture, VkImageLayout newLayout)
{
	return Texture_QuickTransitionImageLayout(renderer.Device, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, texture->Image, texture->MipMapLevels, &texture->TextureImageLayout, &newLayout);
}

VkResult TextureFunctions::TransitionImageLayout(Texture texture, VkImageLayout oldLayout, VkImageLayout newLayout)
{
	return Texture_QuickTransitionImageLayout(renderer.Device, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, texture.Image, texture.MipMapLevels, &oldLayout, &newLayout);
}

VkResult TextureFunctions::TransitionImageLayout(Texture* texture, VkImageLayout oldLayout, VkImageLayout newLayout)
{
	return Texture_QuickTransitionImageLayout(renderer.Device, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, texture->Image, texture->MipMapLevels, &oldLayout, &newLayout);
}

VkResult TextureFunctions::TransitionImageLayout(std::shared_ptr<Texture> texture, VkImageLayout oldLayout, VkImageLayout newLayout)
{
	return Texture_QuickTransitionImageLayout(renderer.Device, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, texture->Image, texture->MipMapLevels, &oldLayout, &newLayout);
}

VkResult TextureFunctions::TransitionImageLayout(VkImage image, uint32 mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout)
{
	return Texture_QuickTransitionImageLayout(renderer.Device, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, image, mipmapLevels, &oldLayout, &newLayout);
}

VkResult TextureFunctions::TransitionImageLayout(VkCommandBuffer commandBuffer, Texture texture, VkImageLayout newLayout)
{
	return Texture_CommandBufferTransitionImageLayout(commandBuffer, texture.Image, texture.MipMapLevels, texture.TextureImageLayout, newLayout);
}

VkResult TextureFunctions::TransitionImageLayout(VkCommandBuffer commandBuffer, Texture* texture, VkImageLayout newLayout)
{
	return Texture_CommandBufferTransitionImageLayout(commandBuffer, texture->Image, texture->MipMapLevels, texture->TextureImageLayout, newLayout);
}

VkResult TextureFunctions::TransitionImageLayout(VkCommandBuffer commandBuffer, std::shared_ptr<Texture> texture, VkImageLayout newLayout)
{
	return Texture_CommandBufferTransitionImageLayout(commandBuffer, texture->Image, texture->MipMapLevels, texture->TextureImageLayout, newLayout);
}

VkResult TextureFunctions::TransitionImageLayout(VkCommandBuffer commandBuffer, Texture texture, VkImageLayout oldLayout, VkImageLayout newLayout)
{
	return Texture_CommandBufferTransitionImageLayout(commandBuffer, texture.Image, texture.MipMapLevels, oldLayout, newLayout);
}

VkResult TextureFunctions::TransitionImageLayout(VkCommandBuffer commandBuffer, Texture* texture, VkImageLayout oldLayout, VkImageLayout newLayout)
{
	return Texture_CommandBufferTransitionImageLayout(commandBuffer, texture->Image, texture->MipMapLevels, oldLayout, newLayout);
}

VkResult TextureFunctions::TransitionImageLayout(VkCommandBuffer commandBuffer, std::shared_ptr<Texture> texture, VkImageLayout oldLayout, VkImageLayout newLayout)
{
	return Texture_CommandBufferTransitionImageLayout(commandBuffer, texture->Image, texture->MipMapLevels, oldLayout, newLayout);
}

VkResult TextureFunctions::TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout oldLayout, VkImageLayout newLayout)
{
	return Texture_CommandBufferTransitionImageLayout(commandBuffer, image, 0, oldLayout, newLayout);
}

VkResult TextureFunctions::TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, uint32 mipmapLevels, VkImageLayout oldLayout, VkImageLayout newLayout)
{
	return Texture_CommandBufferTransitionImageLayout(commandBuffer, image, mipmapLevels, oldLayout, newLayout);
}

VkResult TextureFunctions::CopyBufferToTexture(Texture& texture, VkBuffer& buffer)
{
	return Texture_CopyBufferToTexture(renderer.Device, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, texture.Image, buffer, texture.TextureUsage, texture.Width, texture.Height, texture.Depth);
}

VkResult TextureFunctions::CopyBufferToTexture(Texture* texture, VkBuffer& buffer)
{
	return Texture_CopyBufferToTexture(renderer.Device, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, texture->Image, buffer, texture->TextureUsage, texture->Width, texture->Height, texture->Depth);
}

VkResult TextureFunctions::CopyBufferToTexture(std::shared_ptr<Texture> texture, VkBuffer& buffer)
{
	return Texture_CopyBufferToTexture(renderer.Device, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, texture->Image, buffer, texture->TextureUsage, texture->Width, texture->Height, texture->Depth);
}

VkResult TextureFunctions::CopyBufferToTexture(VkImage image, VkBuffer buffer, TextureUsageEnum textureType, vec3 textureSize)
{
	return Texture_CopyBufferToTexture(renderer.Device, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, image, buffer, textureType, static_cast<int>(textureSize.x), static_cast<int>(textureSize.y), static_cast<int>(textureSize.z));
}

VkResult TextureFunctions::CopyBufferToTexture(VkImage image, VkBuffer buffer, TextureUsageEnum textureType, ivec3 textureSize)
{
	return Texture_CopyBufferToTexture(renderer.Device, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, image, buffer, textureType, textureSize.x, textureSize.y, textureSize.z);
}

VkResult TextureFunctions::CopyBufferToTexture(VkImage image, VkBuffer buffer, TextureUsageEnum textureType, int width, int height, int depth)
{
	return Texture_CopyBufferToTexture(renderer.Device, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, image, buffer, textureType, width, height, depth);
}

VkResult TextureFunctions::GenerateMipmaps(Texture& texture)
{
	return Texture_GenerateMipmaps(renderer.Device, renderer.PhysicalDevice, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, texture.Image, &texture.TextureByteFormat, texture.MipMapLevels, texture.Width, texture.Height);
}

VkResult TextureFunctions::GenerateMipmaps(Texture* texture)
{
	return Texture_GenerateMipmaps(renderer.Device, renderer.PhysicalDevice, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, texture->Image, &texture->TextureByteFormat, texture->MipMapLevels, texture->Width, texture->Height);
}

VkResult TextureFunctions::GenerateMipmaps(std::shared_ptr<Texture> texture)
{
	return Texture_GenerateMipmaps(renderer.Device, renderer.PhysicalDevice, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, texture->Image, &texture->TextureByteFormat, texture->MipMapLevels, texture->Width, texture->Height);
}

VkResult TextureFunctions::GenerateMipmaps(VkImage image, VkFormat& textureByteFormat, uint32 mipmapLevels, int width, int height)
{
	return Texture_GenerateMipmaps(renderer.Device, renderer.PhysicalDevice, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, image, &textureByteFormat, mipmapLevels, width, height);
}

VkResult TextureFunctions::GenerateMipmaps(VkImage image, VkFormat& textureByteFormat, uint32 mipmapLevels, ivec2 textureSize)
{
	return Texture_GenerateMipmaps(renderer.Device, renderer.PhysicalDevice, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, image, &textureByteFormat, mipmapLevels, textureSize.x, textureSize.y);
}

VkResult TextureFunctions::GenerateMipmaps(VkImage image, VkFormat& textureByteFormat, uint32 mipmapLevels, vec2 textureSize)
{
	return Texture_GenerateMipmaps(renderer.Device, renderer.PhysicalDevice, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, image, &textureByteFormat, mipmapLevels, static_cast<int>(textureSize.x), static_cast<int>(textureSize.y));
}
