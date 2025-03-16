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

VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, VkFence* inFlightFences, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, uint32_t swapChainImageCount)
{
    if (!device || 
        !inFlightFences || 
        !acquireImageSemaphores || 
        !presentImageSemaphores)
    {
        fprintf(stderr, "Error: Null pointer in DLL_Renderer_SetUpSemaphores\n");
        return VK_ERROR_INITIALIZATION_FAILED;
    }

    Vector<VkFence> inFlightFenceList(swapChainImageCount);
    Vector<VkSemaphore> acquireImageSemaphoreList(swapChainImageCount);
    Vector<VkSemaphore> presentImageSemaphoreList(swapChainImageCount);

    VULKAN_RESULT(Renderer_SetUpSemaphores(device, inFlightFenceList, acquireImageSemaphoreList, presentImageSemaphoreList));
    std::memcpy(inFlightFences, inFlightFenceList.data(), swapChainImageCount * sizeof(VkFence));
    std::memcpy(acquireImageSemaphores, acquireImageSemaphoreList.data(), swapChainImageCount * sizeof(VkSemaphore));
    std::memcpy(presentImageSemaphores, presentImageSemaphoreList.data(), swapChainImageCount * sizeof(VkSemaphore));

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

 VkResult DLL_Renderer_StartFrame(VkDevice device, VkSwapchainKHR swapChain, VkFence* fenceList, VkSemaphore* acquireImageSemaphoreList, uint32_t* pImageIndex, uint32_t* pCommandIndex, bool* pRebuildRendererFlag)
{
    return Renderer_StartFrame(device, swapChain, fenceList, acquireImageSemaphoreList, pImageIndex, pCommandIndex, pRebuildRendererFlag);
}

 VkResult DLL_Renderer_EndFrame(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence& fenceList, VkQueue graphicsQueue, VkQueue presentQueue, uint32_t commandIndex, uint32_t imageIndex, VkCommandBuffer* pCommandBufferSubmitList, uint32_t commandBufferCount, bool* rebuildRendererFlag)
 {
     return Renderer_EndFrame(swapChain, acquireImageSemaphoreList, presentImageSemaphoreList, &fenceList, graphicsQueue, presentQueue, commandIndex, imageIndex, pCommandBufferSubmitList, commandBufferCount, rebuildRendererFlag);
 }


uint DLL_Tools_GetMemoryType(VkPhysicalDevice physicalDevice, uint32_t typeFilter, VkMemoryPropertyFlags properties)
{
    return Renderer_GetMemoryType(physicalDevice, typeFilter, properties);
}

void DLL_Tools_DeleteAllocatedPtr(void* ptr)
{
    delete[] ptr;
}
