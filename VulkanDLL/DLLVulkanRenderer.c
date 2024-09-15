#include "DLLVulkanRenderer.h"

VkInstance DLL_Renderer_CreateVulkanInstance()
{
	return Renderer_CreateVulkanInstance();
}

VkDebugUtilsMessengerEXT DLL_Renderer_SetupDebugMessenger(VkInstance instance)
{
	return Renderer_SetupDebugMessenger( instance);
}

VkResult DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, VkPhysicalDevice* physicalDevice, VkSurfaceKHR surface, VkPhysicalDeviceFeatures* physicalDeviceFeatures, uint32* graphicsFamily, uint32* presentFamily)
{
	return Renderer_SetUpPhysicalDevice(instance, physicalDevice, surface, physicalDeviceFeatures, graphicsFamily, presentFamily);
}

VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32 graphicsFamily, uint32 presentFamily)
{
	return Renderer_SetUpDevice( physicalDevice,  graphicsFamily,  presentFamily);
}

VkCommandPool DLL_Renderer_SetUpCommandPool(VkDevice device, uint32 graphicsFamily)
{
	return Renderer_SetUpCommandPool( device,  graphicsFamily);
}

VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, VkFence** inFlightFences, VkSemaphore** acquireImageSemaphores, VkSemaphore** presentImageSemaphores, int maxFramesInFlight)
{
	return Renderer_SetUpSemaphores( device, inFlightFences, acquireImageSemaphores, presentImageSemaphores,  maxFramesInFlight);
}

VkResult DLL_Renderer_GetDeviceQueue(VkDevice device, uint32 graphicsFamily, uint32 presentFamily, VkQueue* graphicsQueue, VkQueue* presentQueue)
{
	return Renderer_GetDeviceQueue(device, graphicsFamily, presentFamily, graphicsQueue, presentQueue);
}

VkResult DLL_Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceFormatKHR* surfaceFormat, uint32* surfaceFormatCount)
{
	return Renderer_GetSurfaceFormats(physicalDevice, surface, surfaceFormat, surfaceFormatCount);
}

VkResult DLL_Renderer_GetPresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkPresentModeKHR* presentMode, int32* presentModeCount)
{
	return Renderer_GetPresentModes(physicalDevice, surface, presentMode, presentModeCount);
}

bool DLL_Renderer_GetRayTracingSupport()
{
	return Renderer_GetRayTracingSupport();
}

void DLL_Renderer_GetRendererFeatures(VkPhysicalDeviceVulkan11Features* physicalDeviceVulkan11Features)
{
	return Renderer_GetRendererFeatures(physicalDeviceVulkan11Features);
}

void DLL_Renderer_GetWin32Extensions(uint32_t* extensionCount, const char*** enabledExtensions)
{
 	Renderer_GetWin32Extensions(extensionCount, enabledExtensions);
}

VkResult DLL_Renderer_StartFrame()
{
	return Renderer_StartFrame();
}

VkResult DLL_Renderer_EndFrame(VkCommandBuffer* pCommandBufferSubmitList, uint32 commandBufferCount)
{
	return Renderer_EndFrame( pCommandBufferSubmitList, commandBufferCount);
}

VkResult DLL_Renderer_BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo)
{
	return Renderer_BeginCommandBuffer(pCommandBufferList, commandBufferBeginInfo);
}

VkResult DLL_Renderer_EndCommandBuffer(VkCommandBuffer* pCommandBufferList)
{
	return Renderer_EndCommandBuffer(pCommandBufferList);
}

VkResult DLL_Renderer_SubmitDraw(VkCommandBuffer* pCommandBufferSubmitList)
{
	return Renderer_SubmitDraw(pCommandBufferSubmitList);
}