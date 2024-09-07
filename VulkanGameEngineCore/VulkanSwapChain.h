#pragma once
#include <vulkan/vulkan.h>
#include "CTypedef.h"
#include "Macro.h"

typedef struct swapChainState
{
	uint32 SwapChainImageCount;
	uint32 GraphicsFamily;
	uint32 PresentFamily;
	VkQueue GraphicsQueue;
	VkQueue PresentQueue;
	VkImage* SwapChainImages;
	VkImageView* SwapChainImageViews;
	VkExtent2D SwapChainResolution;
	VkSwapchainKHR Swapchain;
}SwapChainState;

DLL_EXPORT VkResult Vulkan_SetUpSwapChain();
DLL_EXPORT VkResult Vulkan_RebuildSwapChain();
DLL_EXPORT void SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, uint32* graphicsFamily, uint32* presentFamily);
DLL_EXPORT void Vulkan_DestroyImageView();
DLL_EXPORT void Vulkan_DestroySwapChain();
