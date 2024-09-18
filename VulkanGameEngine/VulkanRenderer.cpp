#include "VulkanRenderer.h"

VkResult VulkanRenderer::StartFrame()
{
	return Renderer_StartFrame(renderer.Device, 
                               renderer.SwapChain.Swapchain, 
                               renderer.InFlightFences, 
                               renderer.AcquireImageSemaphores, 
                               &renderer.ImageIndex, 
                               &renderer.CommandIndex,
                               &renderer.RebuildRendererFlag);
}

VkResult VulkanRenderer::EndFrame(List<VkCommandBuffer> commandBufferSubmitList)
{
    return Renderer_EndFrame(renderer.SwapChain.Swapchain,
                             renderer.AcquireImageSemaphores, 
                             renderer.PresentImageSemaphores, 
                             renderer.InFlightFences,
                             renderer.SwapChain.GraphicsQueue, 
                             renderer.SwapChain.PresentQueue,
                             renderer.CommandIndex,
                             renderer.ImageIndex,
                             commandBufferSubmitList.data(), 
                             commandBufferSubmitList.size(),
                             &renderer.RebuildRendererFlag);
}

VkResult SubmitDraw(List<VkCommandBuffer> commandBufferSubmitList)
{
    return Renderer_SubmitDraw(renderer.AcquireImageSemaphores,
                               renderer.PresentImageSemaphores,
                               renderer.InFlightFences,
                               renderer.SwapChain.GraphicsQueue,
                               renderer.SwapChain.PresentQueue,
                               renderer.CommandIndex,
                               renderer.ImageIndex,
                               commandBufferSubmitList.data(),
                               commandBufferSubmitList.size());
}

VkCommandBuffer VulkanRenderer::BeginCommandBuffer()
{
    return Renderer_BeginSingleUseCommandBuffer(renderer.Device, renderer.CommandPool);
}

VkResult VulkanRenderer::BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo)
{
    return Renderer_BeginCommandBuffer(pCommandBufferList, commandBufferBeginInfo);
}

VkResult VulkanRenderer::EndCommandBuffer(VkCommandBuffer* pCommandBufferList)
{
	return Renderer_EndCommandBuffer(pCommandBufferList);
}

VkResult VulkanRenderer::EndCommandBuffer(VkCommandBuffer commandBuffer)
{
	return Renderer_EndSingleUseCommandBuffer(renderer.Device, renderer.CommandPool, renderer.SwapChain.GraphicsQueue, commandBuffer);
}
