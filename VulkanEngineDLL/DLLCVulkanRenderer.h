#pragma once
#include <CVulkanRenderer.h>
#include "DLL.h"

DLL_EXPORT VkResult DLL_CreateDebugUtilsMessengerEXT(VkInstance instance, const VkDebugUtilsMessengerCreateInfoEXT* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDebugUtilsMessengerEXT* pDebugMessenger);
DLL_EXPORT void DLL_DestroyDebugUtilsMessengerEXT(VkInstance instance, const VkAllocationCallbacks* pAllocator);
DLL_EXPORT VKAPI_ATTR VkBool32 VKAPI_CALL DLL_Vulkan_DebugCallBack(VkDebugUtilsMessageSeverityFlagBitsEXT MessageSeverity, VkDebugUtilsMessageTypeFlagsEXT MessageType, const VkDebugUtilsMessengerCallbackDataEXT* CallBackData, void* UserData);

DLL_EXPORT VkResult DLL_Renderer_GetWin32Extensions(uint32_t* extensionCount, char*** enabledExtensions);

DLL_EXPORT VkResult DLL_Renderer_CreateCommandBuffers(VkDevice device, VkCommandPool commandPool, VkCommandBuffer* commandBufferList, uint32 commandBufferCount);
DLL_EXPORT VkResult DLL_Renderer_CreateFrameBuffer(VkDevice device, VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo);
DLL_EXPORT VkResult DLL_Renderer_CreateRenderPass(VkDevice device, RenderPassCreateInfoStruct* renderPassCreateInfo);
DLL_EXPORT VkResult DLL_Renderer_CreateDescriptorPool(VkDevice device, VkDescriptorPool* descriptorPool, VkDescriptorPoolCreateInfo* descriptorPoolCreateInfo);
DLL_EXPORT VkResult DLL_Renderer_CreateDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayout, VkDescriptorSetLayoutCreateInfo* descriptorSetLayoutCreateInfo);
DLL_EXPORT VkResult DLL_Renderer_CreatePipelineLayout(VkDevice device, VkPipelineLayout* pipelineLayout, VkPipelineLayoutCreateInfo* pipelineLayoutCreateInfo);
DLL_EXPORT VkResult DLL_Renderer_AllocateDescriptorSets(VkDevice device, VkDescriptorSet* descriptorSet, VkDescriptorSetAllocateInfo* descriptorSetAllocateInfo);
DLL_EXPORT VkResult DLL_Renderer_AllocateCommandBuffers(VkDevice device, VkCommandBuffer* commandBuffer, VkCommandBufferAllocateInfo* commandBufferAllocateInfo);
DLL_EXPORT VkResult DLL_Renderer_CreateGraphicsPipelines(VkDevice device, VkPipeline* graphicPipeline, VkGraphicsPipelineCreateInfo* createGraphicPipelines, uint32 createGraphicPipelinesCount);
DLL_EXPORT VkResult DLL_Renderer_CreateCommandPool(VkDevice device, VkCommandPool* commandPool, VkCommandPoolCreateInfo* commandPoolInfo);
DLL_EXPORT void DLL_Renderer_UpdateDescriptorSet(VkDevice device, VkWriteDescriptorSet* writeDescriptorSet, uint32 count);

DLL_EXPORT VkResult DLL_Renderer_StartFrame(VkDevice device, VkSwapchainKHR swapChain, VkFence* fenceList, VkSemaphore* acquireImageSemaphoreList, uint32_t* pImageIndex, uint32_t* pCommandIndex, bool* pRebuildRendererFlag);
DLL_EXPORT VkResult DLL_Renderer_EndFrame(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue, VkQueue presentQueue, uint32_t commandIndex, uint32_t imageIndex, VkCommandBuffer* pCommandBufferSubmitList, uint32_t commandBufferCount, bool* rebuildRendererFlag);

DLL_EXPORT uint32 DLL_Renderer_GetMemoryType(VkPhysicalDevice physicalDevice, uint32_t typeFilter, VkMemoryPropertyFlags properties);

DLL_EXPORT VkResult DLL_Renderer_BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo);
DLL_EXPORT VkResult DLL_Renderer_EndCommandBuffer(VkCommandBuffer* pCommandBufferList);
DLL_EXPORT VkCommandBuffer DLL_Renderer_BeginSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool);
DLL_EXPORT VkResult DLL_Renderer_EndSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkCommandBuffer commandBuffer);

DLL_EXPORT void DLL_Renderer_DestroyFences(VkDevice device, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, VkFence* fences, size_t semaphoreCount);
DLL_EXPORT void DLL_Renderer_DestroyCommandPool(VkDevice device, VkCommandPool* commandPool);
DLL_EXPORT void DLL_Renderer_DestroyDevice(VkDevice device);
DLL_EXPORT void DLL_Renderer_DestroySurface(VkInstance instance, VkSurfaceKHR* surface);
DLL_EXPORT void DLL_Renderer_DestroyDebugger(VkInstance* instance, VkDebugUtilsMessengerEXT debugUtilsMessengerEXT);
DLL_EXPORT void DLL_Renderer_DestroyInstance(VkInstance* instance);
DLL_EXPORT void DLL_Renderer_DestroyRenderPass(VkDevice device, VkRenderPass* renderPass);
DLL_EXPORT void DLL_Renderer_DestroyFrameBuffers(VkDevice device, VkFramebuffer* frameBufferList, uint32 count);
DLL_EXPORT void DLL_Renderer_DestroyDescriptorPool(VkDevice device, VkDescriptorPool* descriptorPool);
DLL_EXPORT void DLL_Renderer_DestroyDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayout);
DLL_EXPORT void DLL_Renderer_DestroyCommandBuffers(VkDevice device, VkCommandPool* commandPool, VkCommandBuffer* commandBufferList, uint32 count);
DLL_EXPORT void DLL_Renderer_DestroyBuffer(VkDevice device, VkBuffer* buffer);
DLL_EXPORT void DLL_Renderer_FreeDeviceMemory(VkDevice device, VkDeviceMemory* memory);
DLL_EXPORT void DLL_Renderer_DestroySwapChainImageView(VkDevice device, VkSurfaceKHR surface, VkImageView* pSwapChainImageViewList, uint32 count);
DLL_EXPORT void DLL_Renderer_DestroySwapChain(VkDevice device, VkSwapchainKHR* swapChain);
DLL_EXPORT void DLL_Renderer_DestroyImageView(VkDevice device, VkImageView* imageView);
DLL_EXPORT void DLL_Renderer_DestroyImage(VkDevice device, VkImage* image);
DLL_EXPORT void DLL_Renderer_DestroySampler(VkDevice device, VkSampler* sampler);
DLL_EXPORT void DLL_Renderer_DestroyPipeline(VkDevice device, VkPipeline* pipeline);
DLL_EXPORT void DLL_Renderer_DestroyPipelineLayout(VkDevice device, VkPipelineLayout* pipelineLayout);
DLL_EXPORT void DLL_Renderer_DestroyPipelineCache(VkDevice device, VkPipelineCache* pipelineCache);
