#include "VulkanSwapChain.h"
#include "VulkanWindow.h"
#include "VulkanRenderer.h"

VkResult Vulkan_SetUpSwapChain()
{
	int width = 0;
	int height = 0;
	uint32 surfaceFormatCount = 0;
	uint32 presentModeCount = 0;
	VkSurfaceFormatKHR* compatibleSwapChainFormatList = NULL;
	VkPresentModeKHR* compatiblePresentModesList = NULL;
	VkSurfaceCapabilitiesKHR surfaceCapabilities = { 0 };

	VULKAN_RESULT(SwapChain_GetSurfaceCapabilities(renderer.PhysicalDevice, renderer.Surface, &surfaceCapabilities));
	VULKAN_RESULT(SwapChain_GetPhysicalDeviceFormats(renderer.PhysicalDevice, renderer.Surface, &compatibleSwapChainFormatList, &surfaceFormatCount));
	VULKAN_RESULT(SwapChain_GetQueueFamilies(renderer.PhysicalDevice, renderer.Surface, &renderer.SwapChain.GraphicsFamily, &renderer.SwapChain.PresentFamily));
	VULKAN_RESULT(SwapChain_GetPhysicalDevicePresentModes(renderer.PhysicalDevice, renderer.Surface, &compatiblePresentModesList, &presentModeCount));
	VkSurfaceFormatKHR swapChainImageFormat = SwapChain_FindSwapSurfaceFormat(compatibleSwapChainFormatList, surfaceFormatCount);
	VkPresentModeKHR swapChainPresentMode = SwapChain_FindSwapPresentMode(compatiblePresentModesList, presentModeCount);
	vulkanWindow->GetFrameBufferSize(vulkanWindow, &width, &height);
	renderer.SwapChain.Swapchain = SwapChain_SetUpSwapChain(renderer.Device, renderer.Surface, surfaceCapabilities, swapChainImageFormat, swapChainPresentMode, renderer.SwapChain.GraphicsFamily, renderer.SwapChain.PresentFamily, width, height, &renderer.SwapChain.SwapChainImageCount);
	renderer.SwapChain.SwapChainImages = SwapChain_SetUpSwapChainImages(renderer.Device, renderer.SwapChain.Swapchain, renderer.SwapChain.SwapChainImageCount);
	renderer.SwapChain.SwapChainImageViews = SwapChain_SetUpSwapChainImageViews(renderer.Device, renderer.SwapChain.SwapChainImages, compatibleSwapChainFormatList, renderer.SwapChain.SwapChainImageCount);

	renderer.SwapChain.SwapChainResolution.width = width;
	renderer.SwapChain.SwapChainResolution.height = height;
	return VK_SUCCESS;
}

VkResult Vulkan_RebuildSwapChain()
{
	return Vulkan_SetUpSwapChain();
}

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
		Renderer_DestroyRenderer();
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
		VkResult result = vkCreateImageView(device, &SwapChainViewInfo, NULL, &swapChainImageViews[x]);
	}
	return swapChainImageViews;
}

void Vulkan_DestroyImageView()
{
	for (uint32 x = 0; x < renderer.SwapChain.SwapChainImageCount; x++)
	{
		if (renderer.Surface != VK_NULL_HANDLE)
		{
			vkDestroyImageView(renderer.Device, renderer.SwapChain.SwapChainImageViews[x], NULL);
			renderer.SwapChain.SwapChainImageViews[x] = VK_NULL_HANDLE;
		}
	}
}

void Vulkan_DestroySwapChain()
{
	vkDestroySwapchainKHR(renderer.Device, renderer.SwapChain.Swapchain, NULL);
	renderer.SwapChain.Swapchain = VK_NULL_HANDLE;
}
