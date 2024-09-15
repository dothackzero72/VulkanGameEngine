#pragma once
#include "DLL.h"
#include <VulkanRenderer.h>

DLL_EXPORT VkInstance DLL_Renderer_CreateVulkanInstance();
DLL_EXPORT VkDebugUtilsMessengerEXT DLL_Renderer_SetupDebugMessenger(VkInstance instance);
DLL_EXPORT VkResult DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, VkPhysicalDevice* physicalDevice, VkSurfaceKHR surface, VkPhysicalDeviceFeatures* physicalDeviceFeatures, uint32* graphicsFamily, uint32* presentFamily);
DLL_EXPORT VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32 graphicsFamily, uint32 presentFamily);
DLL_EXPORT VkCommandPool DLL_Renderer_SetUpCommandPool(VkDevice device, uint32 graphicsFamily);
DLL_EXPORT VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, VkFence** inFlightFences, VkSemaphore** acquireImageSemaphores, VkSemaphore** presentImageSemaphores, int maxFramesInFlight);
DLL_EXPORT VkResult DLL_Renderer_GetDeviceQueue(VkDevice device, uint32 graphicsFamily, uint32 presentFamily, VkQueue* graphicsQueue, VkQueue* presentQueue);
DLL_EXPORT VkResult DLL_Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceFormatKHR* surfaceFormat, uint32* surfaceFormatCount);
DLL_EXPORT VkResult DLL_Renderer_GetPresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkPresentModeKHR* presentMode, int32* presentModeCount);
DLL_EXPORT bool DLL_Renderer_GetRayTracingSupport();
DLL_EXPORT void DLL_Renderer_GetRendererFeatures(VkPhysicalDeviceVulkan11Features* physicalDeviceVulkan11Features);
DLL_EXPORT void DLL_Renderer_GetWin32Extensions(uint32_t* extensionCount, const char*** enabledExtensions);
DLL_EXPORT VkResult DLL_Renderer_StartFrame();
DLL_EXPORT VkResult DLL_Renderer_EndFrame(VkCommandBuffer* pCommandBufferSubmitList, uint32 commandBufferCount);
DLL_EXPORT VkResult DLL_Renderer_BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo);
DLL_EXPORT VkResult DLL_Renderer_EndCommandBuffer(VkCommandBuffer* pCommandBufferList);
DLL_EXPORT VkResult DLL_Renderer_SubmitDraw(VkCommandBuffer* pCommandBufferSubmitList);