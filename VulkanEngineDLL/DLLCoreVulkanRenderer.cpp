#include "DLLCoreVulkanRenderer.h"
#include <vector>

VkExtensionProperties_C* DLL_Renderer_GetDeviceExtensions(VkPhysicalDevice physicalDevice, int* count) 
{
    std::vector<VkExtensionProperties> extensions = Renderer_GetDeviceExtensions(physicalDevice); 
    *count = static_cast<int>(extensions.size());

    VkExtensionProperties_C* result = new VkExtensionProperties_C[*count];
    for (size_t x = 0; x < extensions.size(); ++x) 
    {
        std::strncpy(result[x].extensionName, extensions[x].extensionName, sizeof(result[x].extensionName));
        result[x].specVersion = extensions[x].specVersion;
    }

    return result;
}

VkSurfaceFormatKHR* DLL_Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, int* count) 
{
    std::vector<VkSurfaceFormatKHR> formats = Renderer_GetSurfaceFormats(physicalDevice, surface);
    *count = static_cast<int>(formats.size());

    VkSurfaceFormatKHR* result = new VkSurfaceFormatKHR[*count];
    std::memcpy(result, formats.data(), *count * sizeof(VkSurfaceFormatKHR));

    return result;
}

VkPresentModeKHR* DLL_Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, int* count)
{
    std::vector<VkPresentModeKHR> presentModes = Renderer_GetSurfacePresentModes(physicalDevice, surface);
    *count = static_cast<int>(presentModes.size());

    VkPresentModeKHR* result = new VkPresentModeKHR[*count];
    std::memcpy(result, presentModes.data(), *count * sizeof(VkPresentModeKHR));

    return result;
}


Vector<VkExtensionProperties> DLL_Renderer_GetDeviceExtensions(VkPhysicalDevice physicalDevice)
{
	return Renderer_GetDeviceExtensions(physicalDevice);
}

Vector<VkSurfaceFormatKHR> DLL_Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
{
	return Renderer_GetSurfaceFormats(physicalDevice, surface);
}

Vector<VkPresentModeKHR> DLL_Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
{
	return Renderer_GetSurfacePresentModes(physicalDevice, surface);
}

bool DLL_Renderer_GetRayTracingSupport()
{
	return Renderer_GetRayTracingSupport();
}

VkInstance DLL_Renderer_CreateVulkanInstance()
{
	return Renderer_CreateVulkanInstance();
}

VkDebugUtilsMessengerEXT DLL_Renderer_SetupDebugMessenger(VkInstance instance)
{
	return Renderer_SetupDebugMessenger(instance);
}

VkPhysicalDeviceFeatures DLL_Renderer_GetPhysicalDeviceFeatures(VkPhysicalDevice physicalDevice)
{
	return Renderer_GetPhysicalDeviceFeatures(physicalDevice);
}

std::vector<VkPhysicalDevice> DLL_Renderer_GetPhysicalDeviceList(VkInstance& instance)
{
	return Renderer_GetPhysicalDeviceList(instance);
}

VkPhysicalDevice DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily)
{
	return Renderer_SetUpPhysicalDevice(instance, surface, graphicsFamily, presentFamily);
}

VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, std::vector<VkFence>& inFlightFences, std::vector<VkSemaphore>& acquireImageSemaphores, std::vector<VkSemaphore>& presentImageSemaphores)
{
	return Renderer_SetUpSemaphores(device, inFlightFences, acquireImageSemaphores, presentImageSemaphores);
}

VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32 graphicsFamily, uint32 presentFamily)
{
	return Renderer_SetUpDevice(physicalDevice, graphicsFamily, presentFamily);
}

VkCommandPool DLL_Renderer_SetUpCommandPool(VkDevice device, uint32 graphicsFamily)
{
	return Renderer_SetUpCommandPool(device, graphicsFamily);
}

VkResult DLL_Renderer_GetDeviceQueue(VkDevice device, uint32 graphicsFamily, uint32 presentFamily, VkQueue& graphicsQueue, VkQueue& presentQueue)
{
	return Renderer_GetDeviceQueue(device, graphicsFamily, presentFamily, graphicsQueue, presentQueue);
}

VkResult DLL_SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& graphicsFamily, uint32& presentFamily)
{
	return SwapChain_GetQueueFamilies(physicalDevice, surface, graphicsFamily, presentFamily);
}

VkSurfaceCapabilitiesKHR DLL_SwapChain_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
{
	return SwapChain_GetSurfaceCapabilities(physicalDevice, surface);
}

std::vector<VkSurfaceFormatKHR> DLL_SwapChain_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
{
	return SwapChain_GetPhysicalDeviceFormats(physicalDevice, surface);
}

std::vector<VkPresentModeKHR> DLL_SwapChain_GetPhysicalDevicePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
{
	return SwapChain_GetPhysicalDevicePresentModes(physicalDevice, surface);
}

VkSwapchainKHR DLL_SwapChain_SetUpSwapChain(VkDevice device, VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily, uint32 width, uint32 height, uint32* swapChainImageCount)
{
	return SwapChain_SetUpSwapChain(device, physicalDevice, surface, graphicsFamily, presentFamily, width, height, swapChainImageCount);
}

std::vector<VkImage> DLL_SwapChain_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain)
{
	return SwapChain_SetUpSwapChainImages(device, swapChain);
}

std::vector<VkImageView> DLL_SwapChain_SetUpSwapChainImageViews(VkDevice device, std::vector<VkImage> swapChainImageList, VkSurfaceFormatKHR& swapChainImageFormat)
{
	return SwapChain_SetUpSwapChainImageViews(device, swapChainImageList, swapChainImageFormat);
}

VkSurfaceFormatKHR DLL_SwapChain_FindSwapSurfaceFormat(std::vector<VkSurfaceFormatKHR>& availableFormats)
{
	return SwapChain_FindSwapSurfaceFormat(availableFormats);
}

VkPresentModeKHR DLL_SwapChain_FindSwapPresentMode(std::vector<VkPresentModeKHR>& availablePresentModes)
{
	return SwapChain_FindSwapPresentMode(availablePresentModes);
}

