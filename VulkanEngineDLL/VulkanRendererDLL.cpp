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

VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, Vector<VkFence>& inFlightFences, Vector<VkSemaphore>& acquireImageSemaphores, Vector<VkSemaphore>& presentImageSemaphores)
{
    return Renderer_SetUpSemaphores(device, inFlightFences, acquireImageSemaphores, presentImageSemaphores);
}

VkPhysicalDevice DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily)
{
    return Renderer_SetUpPhysicalDevice(instance, surface, graphicsFamily, presentFamily);
}

VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32 graphicsFamily, uint32 presentFamily)
{
    return Renderer_SetUpDevice(physicalDevice, graphicsFamily, presentFamily);
}

VkPresentModeKHR* DLL_Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& count)
{
    std::vector<VkPresentModeKHR> presentModes = Renderer_GetSurfacePresentModes(physicalDevice, surface);
    count = static_cast<int>(presentModes.size());

    VkPresentModeKHR* result = new VkPresentModeKHR[count];
    std::memcpy(result, presentModes.data(), count * sizeof(VkPresentModeKHR));

    return result;
}

void DLL_DeleteAllocatedPtr(void* ptr)
{
    delete[] ptr;
}
