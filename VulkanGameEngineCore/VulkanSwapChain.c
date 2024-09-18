#include "VulkanSwapChain.h"
#include "VulkanWindow.h"
#include "CVulkanRenderer.h"

VkSurfaceFormatKHR SwapChain_FindSwapSurfaceFormat(VkSurfaceFormatKHR* availableFormats, uint32_t availableFormatsCount)
{
    for (uint32_t x = 0; x < availableFormatsCount; x++)
    {
        if (availableFormats[x].format == VK_FORMAT_B8G8R8A8_UNORM &&
            availableFormats[x].colorSpace == VK_COLOR_SPACE_SRGB_NONLINEAR_KHR)
        {
            return availableFormats[x];
        }
    }
    fprintf(stderr, "Couldn't find a usable swap surface format.\n");
    return (VkSurfaceFormatKHR) { VK_FORMAT_UNDEFINED, VK_COLOR_SPACE_MAX_ENUM_KHR };
}

VkPresentModeKHR SwapChain_FindSwapPresentMode(VkPresentModeKHR* availablePresentModes, uint32_t availablePresentModesCount)
{
	for (uint32_t x = 0; x < availablePresentModesCount; x++)
	{
		if (availablePresentModes[x] == VK_PRESENT_MODE_MAILBOX_KHR)
		{
			return availablePresentModes[x];
		}
	}
	return VK_PRESENT_MODE_FIFO_KHR;
}

VkResult SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32* graphicsFamily, uint32* presentFamily)
{
	uint32 queueFamilyCount = 0;
	vkGetPhysicalDeviceQueueFamilyProperties(physicalDevice, &queueFamilyCount, NULL);

	VkQueueFamilyProperties* queueFamilies = malloc(sizeof(VkQueueFamilyProperties) * queueFamilyCount);
	vkGetPhysicalDeviceQueueFamilyProperties(physicalDevice, &queueFamilyCount, queueFamilies);
	if (queueFamilies)
	{
		for (uint32 x = 0; x <= queueFamilyCount; x++)
		{
			VkBool32 presentSupport = false;
			vkGetPhysicalDeviceSurfaceSupportKHR(physicalDevice, x, surface, &presentSupport);

			if (queueFamilies->queueFlags & VK_QUEUE_GRAPHICS_BIT)
			{
				*presentFamily = x;
				*graphicsFamily = x;
				break;
			}
		}
		free(queueFamilies);
	}
	else
	{
		free(queueFamilies);
	}
	return VK_SUCCESS;
}

VkResult SwapChain_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceCapabilitiesKHR* surfaceCapabilities)
{
	return vkGetPhysicalDeviceSurfaceCapabilitiesKHR(physicalDevice, surface, surfaceCapabilities);
}

VkResult SwapChain_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceFormatKHR** compatibleSwapChainFormatList, uint32* surfaceFormatCount)
{
	VULKAN_RESULT(vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, surfaceFormatCount, NULL));
	if (*surfaceFormatCount != 0)
	{
		*compatibleSwapChainFormatList = malloc(sizeof(VkSurfaceFormatKHR) * (*surfaceFormatCount));
		VULKAN_RESULT(vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, surfaceFormatCount, *compatibleSwapChainFormatList));
	}
	return VK_SUCCESS;
}

VkResult SwapChain_GetPhysicalDevicePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkPresentModeKHR** compatiblePresentModesList, uint32* presentModeCount)
{
	VULKAN_RESULT(vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, presentModeCount, NULL));
	if (*presentModeCount != 0)
	{
		*compatiblePresentModesList = malloc(sizeof(VkSurfaceFormatKHR) * (*presentModeCount));
		VULKAN_RESULT(vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, presentModeCount, *compatiblePresentModesList));
	}
}

VkSwapchainKHR SwapChain_SetUpSwapChain(VkDevice device, VkSurfaceKHR surface, VkSurfaceCapabilitiesKHR surfaceCapabilities, VkSurfaceFormatKHR SwapChainImageFormat, VkPresentModeKHR SwapChainPresentMode, uint32 graphicsFamily, uint32 presentFamily, uint32 width, uint32 height, uint32* swapChainImageCount)
{
	VkSwapchainKHR swapChain = VK_NULL_HANDLE;

	VkExtent2D extent = 
	{ 
		width, 
		height 
	};

	 *swapChainImageCount = surfaceCapabilities.minImageCount + 1;
	if (surfaceCapabilities.maxImageCount > 0 &&
		*swapChainImageCount > surfaceCapabilities.maxImageCount)
	{
		*swapChainImageCount = surfaceCapabilities.maxImageCount;
	}

	VkSwapchainCreateInfoKHR SwapChainCreateInfo =
	{
		.sType = VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR,
		.surface = surface,
		.minImageCount = *swapChainImageCount,
		.imageFormat = SwapChainImageFormat.format,
		.imageColorSpace = SwapChainImageFormat.colorSpace,
		.imageExtent = extent,
		.imageArrayLayers = 1,
		.imageUsage = VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT | VK_IMAGE_USAGE_TRANSFER_SRC_BIT,
		.preTransform = surfaceCapabilities.currentTransform,
		.compositeAlpha = VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR,
		.presentMode = SwapChainPresentMode,
		.clipped = VK_TRUE
	};

	if (graphicsFamily != presentFamily)
	{
		uint32 queueFamilyIndices[] = { graphicsFamily, presentFamily };

		SwapChainCreateInfo.imageSharingMode = VK_SHARING_MODE_CONCURRENT;
		SwapChainCreateInfo.queueFamilyIndexCount = 2;
		SwapChainCreateInfo.pQueueFamilyIndices = queueFamilyIndices;
	}
	else
	{
		SwapChainCreateInfo.imageSharingMode = VK_SHARING_MODE_EXCLUSIVE;
	}
	VULKAN_RESULT(vkCreateSwapchainKHR(device, &SwapChainCreateInfo, NULL, &swapChain));
	return swapChain;
}

VkImage* SwapChain_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain, uint32 swapChainImageCount)
{
	VkImage* swapChainImages = VK_NULL_HANDLE;
	VULKAN_RESULT(vkGetSwapchainImagesKHR(device, swapChain, &swapChainImageCount, NULL));
	swapChainImages = malloc(sizeof(VkImage) * swapChainImageCount);
	VULKAN_RESULT(vkGetSwapchainImagesKHR(device, swapChain, &swapChainImageCount, swapChainImages));
	return swapChainImages;
}

VkImageView* SwapChain_SetUpSwapChainImageViews(VkDevice device, VkImage* swapChainImages, VkSurfaceFormatKHR* swapChainImageFormat, uint32_t swapChainImageCount)
{
	VkImageView* swapChainImageViews = malloc(sizeof(VkImageView) * swapChainImageCount);
	for (uint32_t x = 0; x < swapChainImageCount; x++)
	{
		VkImageViewCreateInfo SwapChainViewInfo =
		{
			.sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
			.image = swapChainImages[x],
			.viewType = VK_IMAGE_VIEW_TYPE_2D,
			.format = swapChainImageFormat->format,
			.subresourceRange =
			{
				.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
				.baseMipLevel = 0,
				.levelCount = 1,
				.baseArrayLayer = 0,
				.layerCount = 1
			}
		};
		VULKAN_RESULT(vkCreateImageView(device, &SwapChainViewInfo, NULL, &swapChainImageViews[x]));
	}
	return swapChainImageViews;
}
