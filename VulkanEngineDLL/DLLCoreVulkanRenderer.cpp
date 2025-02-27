#include "DLLCoreVulkanRenderer.h"
#include <vector>
#include <iostream>

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

VkSurfaceFormatKHR* DLL_Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, int& count) 
{
    std::vector<VkSurfaceFormatKHR> formats = Renderer_GetSurfaceFormats(physicalDevice, surface);
    count = static_cast<int>(formats.size());

    VkSurfaceFormatKHR* result = new VkSurfaceFormatKHR[count];
    std::memcpy(result, formats.data(), count * sizeof(VkSurfaceFormatKHR));

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

 VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, Vector<VkFence>& inFlightFences, Vector<VkSemaphore>& acquireImageSemaphores, Vector<VkSemaphore>& presentImageSemaphores)
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

////SwapChain
VkSurfaceCapabilitiesKHR DLL_SwapChain_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint& width, uint& height)
{
    VkSurfaceCapabilitiesKHR surfaceCapabilities = SwapChain_GetSurfaceCapabilities(physicalDevice, surface);
    width = surfaceCapabilities.maxImageExtent.width;
    height = surfaceCapabilities.maxImageExtent.height;
    return SwapChain_GetSurfaceCapabilities(physicalDevice, surface);
}

VkSurfaceFormatKHR* DLL_SwapChain_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& count)
{
    Vector<VkSurfaceFormatKHR> surfaces = SwapChain_GetPhysicalDeviceFormats(physicalDevice, surface);
    count = static_cast<int>(surfaces.size());

    VkSurfaceFormatKHR* result = new VkSurfaceFormatKHR[count];
    for (uint32_t x = 0; x < count; x++) 
    {
        result[x] = surfaces[x];
    }
    return result;
}

VkPresentModeKHR* DLL_Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& count)
{
    std::vector<VkPresentModeKHR> presentModes = Renderer_GetSurfacePresentModes(physicalDevice, surface);
    count = static_cast<int>(presentModes.size());

    VkPresentModeKHR* result = new VkPresentModeKHR[count];
    std::memcpy(result, presentModes.data(), count * sizeof(VkPresentModeKHR));

    return result;
}

VkResult DLL_SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& graphicsFamily, uint32& presentFamily)
{
    return SwapChain_GetQueueFamilies(physicalDevice, surface, graphicsFamily, presentFamily);
}

VkSurfaceFormatKHR DLL_SwapChain_FindSwapSurfaceFormat(VkSurfaceFormatKHR* availableFormats, uint count)
{
    Vector<VkSurfaceFormatKHR> availableFormatList;
    for (int x = 0; x < count; x++)
    {
        availableFormatList.emplace_back(availableFormats[x]);
    }
    return SwapChain_FindSwapSurfaceFormat(availableFormatList);
}

VkPresentModeKHR DLL_SwapChain_FindSwapPresentMode(VkPresentModeKHR* availablePresentModes, uint count)
{
    Vector<VkPresentModeKHR> availablePresentModeList;
    for (int x = 0; x < count; x++)
    {
        availablePresentModeList.emplace_back(availablePresentModes[x]);
    }
    return SwapChain_FindSwapPresentMode(availablePresentModeList);
}

VkSwapchainKHR DLL_SwapChain_SetUpSwapChain(VkDevice device, VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily, uint32 width, uint32 height, uint32& swapChainImageCount)
{
    return SwapChain_SetUpSwapChain(device, physicalDevice, surface, graphicsFamily, presentFamily, width, height, swapChainImageCount);
}

VkImage* DLL_SwapChain_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain, uint32 swapChainImageCount)
{
    Vector<VkImage> swapChainImageList = SwapChain_SetUpSwapChainImages(device, swapChain, swapChainImageCount);
    VkImage* result = new VkImage[swapChainImageCount];
    std::memcpy(result, swapChainImageList.data(), swapChainImageCount * sizeof(VkImage));
    return result;
}

Vector<VkImageView> DLL_SwapChain_SetUpSwapChainImageViews(VkDevice device, Vector<VkImage> swapChainImageList, VkSurfaceFormatKHR& swapChainImageFormat, uint32& count)
{
    Vector<VkImageView> views = SwapChain_SetUpSwapChainImageViews(device, swapChainImageList, swapChainImageFormat);
    count = views.size();

    return views;
}

void DLL_DeleteAllocatedPtr(void* ptr)
{
    delete[] ptr;
}
