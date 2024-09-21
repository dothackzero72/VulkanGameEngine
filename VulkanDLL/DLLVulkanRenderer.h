#pragma once
#include "DLL.h"
#include <CVulkanRenderer.h>


DLL_EXPORT VkInstance DLL_Renderer_CreateVulkanInstance();

DLL_EXPORT VkDebugUtilsMessengerEXT DLL_Renderer_SetupDebugMessenger(VkInstance instance);
DLL_EXPORT VkResult DLL_Renderer_SetUpPhysicalDevice(VkInstance instance, VkPhysicalDevice* physicalDevice, VkSurfaceKHR surface, VkPhysicalDeviceFeatures* physicalDeviceFeatures, uint32_t* graphicsFamily, uint32_t* presentFamily);
DLL_EXPORT VkDevice DLL_Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32_t graphicsFamily, uint32_t presentFamily);
DLL_EXPORT VkCommandPool DLL_Renderer_SetUpCommandPool(VkDevice device, uint32_t graphicsFamily);
DLL_EXPORT VkResult DLL_Renderer_SetUpSemaphores(VkDevice device, VkFence** inFlightFences, VkSemaphore** acquireImageSemaphores, VkSemaphore** presentImageSemaphores, int maxFramesInFlight);
DLL_EXPORT VkResult DLL_Renderer_GetDeviceQueue(VkDevice device, uint32_t graphicsFamily, uint32_t presentFamily, VkQueue* graphicsQueue, VkQueue* presentQueue);
DLL_EXPORT VkResult DLL_Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkPresentModeKHR** presentModes, uint32_t* presentModeCount);
DLL_EXPORT bool DLL_Renderer_GetRayTracingSupport();
DLL_EXPORT void DLL_Renderer_GetRendererFeatures(VkPhysicalDeviceVulkan11Features* physicalDeviceVulkan11Features);
DLL_EXPORT VkResult DLL_Renderer_GetWin32Extensions(uint32_t* extensionCount, char*** enabledExtensions);
DLL_EXPORT VkResult DLL_Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceFormatKHR** surfaceFormats, uint32_t* surfaceFormatCount);

DLL_EXPORT VkResult DLL_Renderer_CreateRenderPass(VkDevice device, RenderPassCreateInfoStruct* renderPassCreateInfo);
DLL_EXPORT VkResult DLL_Renderer_CreateCommandBuffers(VkDevice device, VkCommandPool commandPool, VkCommandBuffer* commandBufferList, uint32_t commandBufferCount);
DLL_EXPORT VkResult DLL_Renderer_CreateFrameBuffer(VkDevice device, VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo);
DLL_EXPORT VkResult DLL_Renderer_CreateDescriptorPool(VkDevice device, VkDescriptorPool* descriptorPool, VkDescriptorPoolCreateInfo* descriptorPoolCreateInfo);
DLL_EXPORT VkResult DLL_Renderer_CreateDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayout, VkDescriptorSetLayoutCreateInfo* descriptorSetLayoutCreateInfo);
DLL_EXPORT VkResult DLL_Renderer_CreatePipelineLayout(VkDevice device, VkPipelineLayout* pipelineLayout, VkPipelineLayoutCreateInfo* pipelineLayoutCreateInfo);
DLL_EXPORT VkResult DLL_Renderer_AllocateDescriptorSets(VkDevice device, VkDescriptorSet* descriptorSet, VkDescriptorSetAllocateInfo* descriptorSetAllocateInfo);
DLL_EXPORT VkResult DLL_Renderer_AllocateCommandBuffers(VkDevice device, VkCommandBuffer* commandBuffer, VkCommandBufferAllocateInfo* commandBufferAllocateInfo);
DLL_EXPORT VkResult DLL_Renderer_CreateGraphicsPipelines(VkDevice device, VkPipeline* graphicPipeline, VkGraphicsPipelineCreateInfo* createGraphicPipelines, uint32_t createGraphicPipelinesCount);
DLL_EXPORT VkResult DLL_Renderer_CreateCommandPool(VkDevice device, VkCommandPool* commandPool, VkCommandPoolCreateInfo* commandPoolInfo);
DLL_EXPORT void DLL_Renderer_UpdateDescriptorSet(VkDevice device, VkWriteDescriptorSet* writeDescriptorSet, uint32_t count);

DLL_EXPORT VkResult DLL_Renderer_StartFrame(VkDevice device, VkSwapchainKHR swapChain, VkFence* fenceList, VkSemaphore* acquireImageSemaphoreList, uint32_t* pImageIndex, uint32_t* pCommandIndex, bool* pRebuildRendererFlag);
DLL_EXPORT VkResult DLL_Renderer_EndFrame(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue, VkQueue presentQueue, uint32_t commandIndex, uint32_t imageIndex, VkCommandBuffer* pCommandBufferSubmitList, uint32_t commandBufferCount, bool* rebuildRendererFlag);
DLL_EXPORT VkResult DLL_Renderer_SubmitDraw(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue, VkQueue presentQueue, uint32_t commandIndex, uint32_t imageIndex, VkCommandBuffer* pCommandBufferSubmitList, uint32_t commandBufferCount, bool* rebuildRendererFlag);
DLL_EXPORT uint32_t DLL_Renderer_GetMemoryType(VkPhysicalDevice physicalDevice, uint32_t typeFilter, VkMemoryPropertyFlags properties);

DLL_EXPORT VkResult DLL_Renderer_BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo);
DLL_EXPORT VkResult DLL_Renderer_EndCommandBuffer(VkCommandBuffer* pCommandBufferList);
DLL_EXPORT VkCommandBuffer DLL_Renderer_BeginSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool);
DLL_EXPORT VkResult DLL_Renderer_EndSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkCommandBuffer commandBuffer);

DLL_EXPORT void DLL_Renderer_DestroyFences(VkDevice device, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, VkFence* fences, size_t semaphoreCount);
DLL_EXPORT void DLL_Renderer_DestroyCommandPool(VkDevice device, VkCommandPool* commandPool);
DLL_EXPORT void DLL_Renderer_DestroyDevice(VkDevice device);
DLL_EXPORT void DLL_Renderer_DestroySurface(VkInstance instance, VkSurfaceKHR* surface);
DLL_EXPORT void DLL_Renderer_DestroyDebugger(VkInstance* instance);
DLL_EXPORT void DLL_Renderer_DestroyInstance(VkInstance* instance);
DLL_EXPORT void DLL_Renderer_DestroyRenderPass(VkDevice device, VkRenderPass* renderPass);
DLL_EXPORT void DLL_Renderer_DestroyDescriptorPool(VkDevice device, VkDescriptorPool* descriptorPool);
DLL_EXPORT void DLL_Renderer_DestroyDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayout);
DLL_EXPORT void DLL_Renderer_DestroyBuffer(VkDevice device, VkBuffer* buffer);
DLL_EXPORT void DLL_Renderer_FreeDeviceMemory(VkDevice device, VkDeviceMemory* memory);
DLL_EXPORT void DLL_Renderer_DestroySwapChain(VkDevice device, VkSwapchainKHR* swapChain);
DLL_EXPORT void DLL_Renderer_DestroyImageView(VkDevice device, VkImageView* imageView);
DLL_EXPORT void DLL_Renderer_DestroyImage(VkDevice device, VkImage* image);
DLL_EXPORT void DLL_Renderer_DestroySampler(VkDevice device, VkSampler* sampler);
DLL_EXPORT void DLL_Renderer_DestroyPipeline(VkDevice device, VkPipeline* pipeline);
DLL_EXPORT void DLL_Renderer_DestroyPipelineLayout(VkDevice device, VkPipelineLayout* pipelineLayout);
DLL_EXPORT void DLL_Renderer_DestroyPipelineCache(VkDevice device, VkPipelineCache* pipelineCache);
DLL_EXPORT int DLL_Renderer_SimpleTestLIB();