#include "VulkanBufferSystem.h"

int VulkanBufferSystem::NextBufferId = 0;
VulkanBufferSystem bufferSystem = VulkanBufferSystem();

void VulkanBufferSystem::DestroyBuffer(const RendererState& renderer, int vulkanBufferId)
{
	VulkanBuffer_DestroyBuffer(renderer, VulkanBuffer[vulkanBufferId]);
}

void VulkanBufferSystem::DestroyAllBuffers()
{
	for (auto& buffer : VulkanBuffer)
	{
		VulkanBuffer_DestroyBuffer(cRenderer, buffer.second);
	}
}
