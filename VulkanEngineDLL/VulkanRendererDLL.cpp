#include "VulkanRendererDLL.h"
#include <vector>
#include <iostream>

VkInstance DLL_Renderer_CreateVulkanInstance()
{
    return Renderer_CreateVulkanInstance();
}

VkDebugUtilsMessengerEXT DLL_Renderer_SetupDebugMessenger(VkInstance instance)
{
    return Renderer_SetupDebugMessenger(instance);
}

VkResult DLL_Renderer_GetDeviceQueue(VkDevice device, uint32 graphicsFamily, uint32 presentFamily, VkQueue& graphicsQueue, VkQueue& presentQueue)
{
    return Renderer_GetDeviceQueue(device, graphicsFamily, presentFamily, graphicsQueue, presentQueue);
}

VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, VkFence* inFlightFences, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores,  uint32_t swapChainImageCount)
{
    if (!device || 
        !inFlightFences || 
        !acquireImageSemaphores || 
        !presentImageSemaphores) 
    {
        return VK_ERROR_INITIALIZATION_FAILED;
    }

    Vector<VkFence> inFlightFenceList;
    Vector<VkSemaphore> acquireImageSemaphoreList;
    Vector<VkSemaphore> presentImageSemaphoreList;

    inFlightFenceList.reserve(swapChainImageCount);
    acquireImageSemaphoreList.reserve(swapChainImageCount);
    presentImageSemaphoreList.reserve(swapChainImageCount);

    for (uint32_t x = 0; x < swapChainImageCount; x++) 
    {
        inFlightFenceList.push_back(inFlightFences[x]);
        acquireImageSemaphoreList.push_back(acquireImageSemaphores[x]);
        presentImageSemaphoreList.push_back(presentImageSemaphores[x]);
    }

    VULKAN_RESULT(Renderer_SetUpSemaphores(device, inFlightFenceList, acquireImageSemaphoreList, presentImageSemaphoreList));
    return VK_SUCCESS;
}

VkPhysicalDevice DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily)
{
    return Renderer_SetUpPhysicalDevice(instance, surface, graphicsFamily, presentFamily);
}

VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32 graphicsFamily, uint32 presentFamily)
{
    return Renderer_SetUpDevice(physicalDevice, graphicsFamily, presentFamily);
}

VkPresentModeKHR* DLL_Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& swapChainImageCount)
{
    std::vector<VkPresentModeKHR> presentModes = Renderer_GetSurfacePresentModes(physicalDevice, surface);
    swapChainImageCount = static_cast<int>(presentModes.size());

    VkPresentModeKHR* result = new VkPresentModeKHR[swapChainImageCount];
    std::memcpy(result, presentModes.data(), swapChainImageCount * sizeof(VkPresentModeKHR));

    return result;
}

uint DLL_Tools_GetMemoryType(VkPhysicalDevice physicalDevice, uint32_t typeFilter, VkMemoryPropertyFlags properties)
{
    return Renderer_GetMemoryType(physicalDevice, typeFilter, properties);
}

void DLL_Tools_DeleteAllocatedPtr(void* ptr)
{
    delete[] ptr;
}
