#include "SwapChainDLL.h"

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

VkImageView* DLL_SwapChain_SetUpSwapChainImageViews(VkDevice device, VkImage* swapChainImageList, VkSurfaceFormatKHR swapChainImageFormat, size_t swapChainImageCount)
{
    std::vector<VkImage> swapChainImagePtr(swapChainImageList, swapChainImageList + swapChainImageCount);
    auto swapChainViewList = SwapChain_SetUpSwapChainImageViews(device, swapChainImagePtr, swapChainImageFormat);

    VkImageView* result = new VkImageView[swapChainImageCount];
    std::memcpy(result, swapChainViewList.data(), swapChainImageCount * sizeof(VkImageView));
    return result;
}