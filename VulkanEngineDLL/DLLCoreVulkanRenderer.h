#pragma once
#include <CoreVulkanRenderer.h>
#include <vector>

#include "DLLCVulkanRenderer.h"

struct VkExtensionProperties_C 
{
	char extensionName[256];
	uint32_t specVersion;
};

// Function declarations
extern "C" {
	DLL_EXPORT VkExtensionProperties_C* DLL_Renderer_GetDeviceExtensions(VkPhysicalDevice physicalDevice, int* count);
	DLL_EXPORT VkSurfaceFormatKHR* DLL_Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, int* count);
	DLL_EXPORT VkPresentModeKHR* DLL_Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, int* count);
	DLL_EXPORT VkInstance DLL_Renderer_CreateVulkanInstance();
	DLL_EXPORT VkPhysicalDevice DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily);
	DLL_EXPORT VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32 graphicsFamily, uint32 presentFamily);
}

	//DLL_EXPORT bool DLL_Renderer_GetRayTracingSupport();
	//DLL_EXPORT VkInstance DLL_Renderer_CreateVulkanInstance();
	//DLL_EXPORT VkDebugUtilsMessengerEXT DLL_Renderer_SetupDebugMessenger(VkInstance instance);
	//DLL_EXPORT VkPhysicalDeviceFeatures DLL_Renderer_GetPhysicalDeviceFeatures(VkPhysicalDevice physicalDevice);
	//DLL_EXPORT std::vector<VkPhysicalDevice> DLL_Renderer_GetPhysicalDeviceList(VkInstance& instance);
	//DLL_EXPORT VkPhysicalDevice DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily);
	//DLL_EXPORT VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, std::vector<VkFence>& inFlightFences, std::vector<VkSemaphore>& acquireImageSemaphores, std::vector<VkSemaphore>& presentImageSemaphores);
	
	//DLL_EXPORT VkCommandPool DLL_Renderer_SetUpCommandPool(VkDevice device, uint32 graphicsFamily);
	//DLL_EXPORT VkResult DLL_Renderer_GetDeviceQueue(VkDevice device, uint32 graphicsFamily, uint32 presentFamily, VkQueue& graphicsQueue, VkQueue& presentQueue);

	//DLL_EXPORT VkResult DLL_SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& graphicsFamily, uint32& presentFamily);
	//DLL_EXPORT VkSurfaceCapabilitiesKHR DLL_SwapChain_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
	//DLL_EXPORT std::vector<VkSurfaceFormatKHR> DLL_SwapChain_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
	//DLL_EXPORT std::vector<VkPresentModeKHR> DLL_SwapChain_GetPhysicalDevicePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
	//DLL_EXPORT VkSwapchainKHR DLL_SwapChain_SetUpSwapChain(VkDevice device, VkSurfaceKHR surface, VkSurfaceCapabilitiesKHR surfaceCapabilities, VkSurfaceFormatKHR SwapChainImageFormat, VkPresentModeKHR SwapChainPresentMode, uint32 graphicsFamily, uint32 presentFamily, uint32 width, uint32 height, uint32* swapChainImageCount);
	//DLL_EXPORT std::vector<VkImage> DLL_SwapChain_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain);

	//DLL_EXPORT std::vector<VkImageView> DLL_SwapChain_SetUpSwapChainImageViews(VkDevice device, std::vector<VkImage> swapChainImageList, VkSurfaceFormatKHR& swapChainImageFormat);
	//DLL_EXPORT VkSurfaceFormatKHR DLL_SwapChain_FindSwapSurfaceFormat(std::vector<VkSurfaceFormatKHR>& availableFormats);
	//DLL_EXPORT VkPresentModeKHR DLL_SwapChain_FindSwapPresentMode(std::vector<VkPresentModeKHR>& availablePresentModes);

