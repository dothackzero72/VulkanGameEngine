#pragma once
#include <CoreVulkanRenderer.h>
#include <vector>

#include "DLLCVulkanRenderer.h"

struct VkExtensionProperties_C 
{
	char extensionName[256];
	uint32_t specVersion;
};

extern "C" {
	DLL_EXPORT VkExtensionProperties_C* DLL_Renderer_GetDeviceExtensions(VkPhysicalDevice physicalDevice, int* count);
	DLL_EXPORT VkSurfaceFormatKHR* DLL_Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& count);
	DLL_EXPORT VkPresentModeKHR* DLL_Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& count);
	DLL_EXPORT VkInstance DLL_Renderer_CreateVulkanInstance();
	DLL_EXPORT VkDebugUtilsMessengerEXT DLL_Renderer_SetupDebugMessenger(VkInstance instance);
	DLL_EXPORT VkPhysicalDevice DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily);
	DLL_EXPORT VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32 graphicsFamily, uint32 presentFamily);
	DLL_EXPORT VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, Vector<VkFence>& inFlightFences, Vector<VkSemaphore>& acquireImageSemaphores, Vector<VkSemaphore>& presentImageSemaphores);
	DLL_EXPORT VkCommandPool DLL_Renderer_SetUpCommandPool(VkDevice device, uint32 graphicsFamily);
	DLL_EXPORT VkResult DLL_Renderer_GetDeviceQueue(VkDevice device, uint32 graphicsFamily, uint32 presentFamily, VkQueue& graphicsQueue, VkQueue& presentQueue);
	DLL_EXPORT VkPhysicalDeviceFeatures DLL_Renderer_GetPhysicalDeviceFeatures(VkPhysicalDevice physicalDevice);
	
	DLL_EXPORT VkSurfaceCapabilitiesKHR DLL_SwapChain_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint& width, uint& height);
	DLL_EXPORT VkResult DLL_SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& graphicsFamily, uint32& presentFamily);
	DLL_EXPORT VkSurfaceFormatKHR DLL_SwapChain_FindSwapSurfaceFormat(VkSurfaceFormatKHR* availableFormats, uint count);
	DLL_EXPORT VkPresentModeKHR DLL_SwapChain_FindSwapPresentMode(VkPresentModeKHR* availablePresentModes, uint count);
	DLL_EXPORT VkSwapchainKHR DLL_SwapChain_SetUpSwapChain(VkDevice device, VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily, uint32 width, uint32 height, uint32& swapChainImageCount);
	DLL_EXPORT VkImage* DLL_SwapChain_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain, uint32 swapChainImageCount);
	DLL_EXPORT Vector<VkImageView> DLL_SwapChain_SetUpSwapChainImageViews(VkDevice device, Vector<VkImage> swapChainImageList, VkSurfaceFormatKHR& swapChainImageFormat);
	DLL_EXPORT VkSurfaceFormatKHR* DLL_SwapChain_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& count);

	DLL_EXPORT void DLL_DeleteAllocatedPtr(void* ptr);
}



