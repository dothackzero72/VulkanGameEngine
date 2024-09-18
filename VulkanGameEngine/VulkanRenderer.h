#pragma once
extern "C"
{
#include <CVulkanRenderer.h>
#include <CBuffer.h>
}
#include "Typedef.h"

class VulkanRenderer
{
private:
public:

	static VkResult StartFrame();
	static VkResult EndFrame(List<VkCommandBuffer> commandBufferSubmitList);
	static VkCommandBuffer BeginCommandBuffer();
	static VkResult BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo);
	static VkResult EndCommandBuffer(VkCommandBuffer* pCommandBufferList);
	static VkResult EndCommandBuffer(VkCommandBuffer commandBuffer);
};

