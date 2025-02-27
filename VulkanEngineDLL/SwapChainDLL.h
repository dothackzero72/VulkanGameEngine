#pragma once
#include "VulkanRendererDLL.h"
#include "DLL.h"

extern "C"
{
	DLL_EXPORT VkSurfaceCapabilitiesKHR DLL_SwapChain_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint& width, uint& height);
	DLL_EXPORT VkResult DLL_SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& graphicsFamily, uint32& presentFamily);
	DLL_EXPORT VkSurfaceFormatKHR DLL_SwapChain_FindSwapSurfaceFormat(VkSurfaceFormatKHR* availableFormats, uint count);
	DLL_EXPORT VkPresentModeKHR DLL_SwapChain_FindSwapPresentMode(VkPresentModeKHR* availablePresentModes, uint count);
	DLL_EXPORT VkSwapchainKHR DLL_SwapChain_SetUpSwapChain(VkDevice device, VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily, uint32 width, uint32 height, uint32& swapChainImageCount);
	DLL_EXPORT VkImage* DLL_SwapChain_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain, uint32 swapChainImageCount);
	DLL_EXPORT VkImageView* DLL_SwapChain_SetUpSwapChainImageViews(VkDevice device, VkImage* swapChainImageList, VkSurfaceFormatKHR swapChainImageFormat, size_t swapChainImageCount);
	DLL_EXPORT VkSurfaceFormatKHR* DLL_SwapChain_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& count);
}

