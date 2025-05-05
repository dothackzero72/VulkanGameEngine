#pragma once
#include <windows.h>
#include <stdbool.h>

#include "Macro.h"
#include "CTypedef.h"
#include "VulkanError.h"

#ifdef __cplusplus
extern "C" {
#endif

static const int MAX_FRAMES_IN_FLIGHT = 3;
typedef void (*TextCallback)(const char*);
typedef void (*RichTextBoxCallback)(const char*);

typedef struct
{
	VkRenderPass* pRenderPass;
	const VkAttachmentDescription* pAttachmentList;
	const VkSubpassDescription* pSubpassDescriptionList;
	const VkSubpassDependency* pSubpassDependencyList;
	uint32						AttachmentCount;
	uint32						SubpassCount;
	uint32						DependencyCount;
	uint32						Width;
	uint32						Height;
}RenderPassCreateInfoStruct;

VkResult CreateDebugUtilsMessengerEXT(VkInstance instance, const VkDebugUtilsMessengerCreateInfoEXT* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDebugUtilsMessengerEXT* pDebugMessenger);
void DestroyDebugUtilsMessengerEXT(VkInstance instance, const VkAllocationCallbacks* pAllocator);
VKAPI_ATTR VkBool32 VKAPI_CALL Vulkan_DebugCallBack(VkDebugUtilsMessageSeverityFlagBitsEXT MessageSeverity, VkDebugUtilsMessageTypeFlagsEXT MessageType, const VkDebugUtilsMessengerCallbackDataEXT* CallBackData, void* UserData);

VkResult Renderer_GetWin32Extensions(uint32_t* extensionCount, char*** enabledExtensions);

VkResult Renderer_CreateCommandBuffers(VkDevice device, VkCommandPool commandPool, VkCommandBuffer* commandBufferList, uint32 commandBufferCount);
VkResult Renderer_CreateFrameBuffer(VkDevice device, VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo);
VkResult Renderer_CreateRenderPass(VkDevice device, RenderPassCreateInfoStruct* renderPassCreateInfo);
VkResult Renderer_CreateDescriptorPool(VkDevice device, VkDescriptorPool* descriptorPool, VkDescriptorPoolCreateInfo* descriptorPoolCreateInfo);
VkResult Renderer_CreateDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayout, VkDescriptorSetLayoutCreateInfo* descriptorSetLayoutCreateInfo);
VkResult Renderer_CreatePipelineLayout(VkDevice device, VkPipelineLayout* pipelineLayout, VkPipelineLayoutCreateInfo* pipelineLayoutCreateInfo);
VkResult Renderer_AllocateDescriptorSets(VkDevice device, VkDescriptorSet* descriptorSet, VkDescriptorSetAllocateInfo* descriptorSetAllocateInfo);
VkResult Renderer_AllocateCommandBuffers(VkDevice device, VkCommandBuffer* commandBuffer, VkCommandBufferAllocateInfo* commandBufferAllocateInfo);
VkResult Renderer_CreateGraphicsPipelines(VkDevice device, VkPipeline* graphicPipeline, VkGraphicsPipelineCreateInfo* createGraphicPipelines, uint32 createGraphicPipelinesCount);
VkResult Renderer_CreateCommandPool(VkDevice device, VkCommandPool* commandPool, VkCommandPoolCreateInfo* commandPoolInfo);
VkResult Renderer_CreateSemaphores(VkDevice device, VkFence* inFlightFences, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, int maxFramesInFlight);
void Renderer_UpdateDescriptorSet(VkDevice device, VkWriteDescriptorSet* writeDescriptorSet, uint32 count);

VkResult Renderer_StartFrame(VkDevice device, VkSwapchainKHR swapChain, VkFence* fenceList, VkSemaphore* acquireImageSemaphoreList, uint32_t* pImageIndex, uint32_t* pCommandIndex, bool* pRebuildRendererFlag);
VkResult Renderer_EndFrame(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue, VkQueue presentQueue, uint32_t commandIndex, uint32_t imageIndex, VkCommandBuffer* pCommandBufferSubmitList, uint32_t commandBufferCount, bool* rebuildRendererFlag);

uint32 Renderer_GetMemoryType(VkPhysicalDevice physicalDevice, uint32_t typeFilter, VkMemoryPropertyFlags properties);

VkResult Renderer_BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo);
VkResult Renderer_EndCommandBuffer(VkCommandBuffer* pCommandBufferList);
VkCommandBuffer Renderer_BeginSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool);
VkResult Renderer_EndSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkCommandBuffer commandBuffer);

void Renderer_DestroyFences(VkDevice device, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, VkFence* fences, size_t semaphoreCount);
void Renderer_DestroyCommandPool(VkDevice device, VkCommandPool* commandPool);
void Renderer_DestroyDevice(VkDevice device);
void Renderer_DestroySurface(VkInstance instance, VkSurfaceKHR* surface);
void Renderer_DestroyDebugger(VkInstance* instance, VkDebugUtilsMessengerEXT debugUtilsMessengerEXT);
void Renderer_DestroyInstance(VkInstance* instance);
void Renderer_DestroyRenderPass(VkDevice device, VkRenderPass* renderPass);
void Renderer_DestroyFrameBuffers(VkDevice device, VkFramebuffer* frameBufferList, uint32 count);
void Renderer_DestroyDescriptorPool(VkDevice device, VkDescriptorPool* descriptorPool);
void Renderer_DestroyDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayout);
void Renderer_DestroyCommandBuffers(VkDevice device, VkCommandPool* commandPool, VkCommandBuffer* commandBufferList, uint32 count);
void Renderer_DestroyBuffer(VkDevice device, VkBuffer* buffer);
void Renderer_FreeDeviceMemory(VkDevice device, VkDeviceMemory* memory);
void Renderer_DestroySwapChainImageView(VkDevice device, VkSurfaceKHR surface, VkImageView* pSwapChainImageViewList, uint32 count);
void Renderer_DestroySwapChain(VkDevice device, VkSwapchainKHR* swapChain);
void Renderer_DestroyImageView(VkDevice device, VkImageView* imageView);
void Renderer_DestroyImage(VkDevice device, VkImage* image);
void Renderer_DestroySampler(VkDevice device, VkSampler* sampler);
void Renderer_DestroyPipeline(VkDevice device, VkPipeline* pipeline);
void Renderer_DestroyPipelineLayout(VkDevice device, VkPipelineLayout* pipelineLayout);
void Renderer_DestroyPipelineCache(VkDevice device, VkPipelineCache* pipelineCache);

#ifdef __cplusplus
}
#endif