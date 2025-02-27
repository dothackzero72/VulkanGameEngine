#pragma once
#include "DLL.h"
#include <CoreVulkanRenderer.h>
#include <vector>

struct VkExtensionProperties_C 
{
	char extensionName[256];
	uint32_t specVersion;
};

extern "C" 
{
	DLL_EXPORT VkInstance DLL_Renderer_CreateVulkanInstance();
	DLL_EXPORT VkDebugUtilsMessengerEXT DLL_Renderer_SetupDebugMessenger(VkInstance instance);
	DLL_EXPORT VkResult DLL_Renderer_GetDeviceQueue(VkDevice device, uint32 graphicsFamily, uint32 presentFamily, VkQueue& graphicsQueue, VkQueue& presentQueue);
	DLL_EXPORT VkResult DLL_Renderer_CreateSemaphores(VkDevice device, VkFence* inFlightFences, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, int maxFramesInFlight);
	DLL_EXPORT VkPhysicalDevice DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily);
	DLL_EXPORT VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32 graphicsFamily, uint32 presentFamily);
	DLL_EXPORT VkPresentModeKHR* DLL_Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& count);
	DLL_EXPORT VkPresentModeKHR* DLL_Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& count);

	DLL_EXPORT void DLL_DeleteAllocatedPtr(void* ptr);
}


