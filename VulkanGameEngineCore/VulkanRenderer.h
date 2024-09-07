#pragma once
#include "DLLMain.h"
#include <windows.h>
#include <stdbool.h>

#include "Macro.h"
#include "CTypedef.h"
#include "VulkanRendererStruct.h"
#include "VulkanSwapChain.h"
#include "VulkanError.h"

static const int MAX_FRAMES_IN_FLIGHT = 3;

typedef struct rendererState
{
	VkInstance Instance;
	VkDevice Device;
	VkPhysicalDevice PhysicalDevice;
	VkSurfaceKHR Surface;
	VkCommandPool CommandPool;
	uint32 ImageIndex;
	uint32 CommandIndex;
	VkDebugUtilsMessengerEXT DebugMessenger;
	SwapChainState SwapChain;
	VkPhysicalDeviceFeatures PhysicalDeviceFeatures;

	VkFence* InFlightFences;
	VkSemaphore* AcquireImageSemaphores;
	VkSemaphore* PresentImageSemaphores;
	bool RebuildRendererFlag;
}RendererState;
extern RendererState Renderer;

DLL_EXPORT void Renderer_Windows_Renderer(uint32* pExtensionCount, VkExtensionProperties** extensionProperties);
DLL_EXPORT VkInstance Renderer_CreateVulkanInstance(VkInstanceCreateInfo instanceInfo);
DLL_EXPORT VkResult Renderer_RendererSetUp();
DLL_EXPORT VkResult Renderer_RebuildSwapChain();
DLL_EXPORT VkResult Renderer_CreateCommandBuffers(VkCommandBuffer* pCommandBufferList);
DLL_EXPORT VkResult Renderer_CreateFrameBuffer(VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo);
DLL_EXPORT VkResult Renderer_CreateRenderPass(Renderer_RenderPassCreateInfoStruct* pRenderPassCreateInfo);
DLL_EXPORT VkResult Renderer_CreateDescriptorPool(VkDescriptorPool* descriptorPool, VkDescriptorPoolCreateInfo* descriptorPoolCreateInfo);
DLL_EXPORT VkResult Renderer_CreateDescriptorSetLayout(VkDescriptorSetLayout* descriptorSetLayout, VkDescriptorSetLayoutCreateInfo* descriptorSetLayoutCreateInfo);
DLL_EXPORT VkResult Renderer_CreatePipelineLayout(VkPipelineLayout* pipelineLayout, VkPipelineLayoutCreateInfo* pipelineLayoutCreateInfo);
DLL_EXPORT VkResult Renderer_AllocateDescriptorSets(VkDescriptorSet* descriptorSet, VkDescriptorSetAllocateInfo* descriptorSetAllocateInfo);
DLL_EXPORT VkResult Renderer_AllocateCommandBuffers(VkCommandBuffer* commandBuffer, VkCommandBufferAllocateInfo* ImGuiCommandBuffers);
DLL_EXPORT VkResult Renderer_CreateGraphicsPipelines(VkPipeline* graphicPipeline, VkGraphicsPipelineCreateInfo* createGraphicPipelines, uint32 createGraphicPipelinesCount);
DLL_EXPORT VkResult Renderer_CreateCommandPool(VkCommandPool* commandPool, VkCommandPoolCreateInfo* commandPoolInfo);
DLL_EXPORT VkResult Renderer_StartFrame();
DLL_EXPORT VkResult Renderer_EndFrame(VkCommandBuffer* pCommandBufferSubmitList, uint32 commandBufferCount);
DLL_EXPORT VkResult Renderer_BeginCommandBuffer(VkCommandBuffer* pCommandBuffer, VkCommandBufferBeginInfo* commandBufferBeginInfo);
DLL_EXPORT VkResult Renderer_EndCommandBuffer(VkCommandBuffer* pCommandBuffer);
DLL_EXPORT VkResult Renderer_SubmitDraw(VkCommandBuffer* pCommandBufferSubmitList);

DLL_EXPORT uint32 Renderer_GetMemoryType(uint32 typeFilter, VkMemoryPropertyFlags properties);
DLL_EXPORT VkCommandBuffer Renderer_BeginSingleUseCommandBuffer();
DLL_EXPORT VkResult Renderer_EndSingleUseCommandBuffer(VkCommandBuffer commandBuffer);

DLL_EXPORT void Renderer_UpdateDescriptorSet(VkWriteDescriptorSet* writeDescriptorSet, uint32 count);
DLL_EXPORT void Renderer_DestroyRenderer();
DLL_EXPORT void Renderer_DestroyFences();
DLL_EXPORT void Renderer_DestroyCommandPool();
DLL_EXPORT void Renderer_DestroyDevice();
DLL_EXPORT void Renderer_DestroySurface();
DLL_EXPORT void Renderer_DestroyDebugger();
DLL_EXPORT void Renderer_DestroyInstance();
DLL_EXPORT void Renderer_DestroyRenderPass(VkRenderPass* renderPass);
DLL_EXPORT void Renderer_DestroyFrameBuffers(VkFramebuffer* frameBufferList);
DLL_EXPORT void Renderer_DestroyDescriptorPool(VkDescriptorPool* descriptorPool);
DLL_EXPORT void Renderer_DestroyDescriptorSetLayout(VkDescriptorSetLayout* descriptorSetLayout);
DLL_EXPORT void Renderer_DestroyCommandBuffers(VkCommandPool* commandPool, VkCommandBuffer* commandBufferList);
DLL_EXPORT void Renderer_DestroyCommnadPool(VkCommandPool* commandPool);
DLL_EXPORT void Renderer_DestroyBuffer(VkBuffer* buffer);
DLL_EXPORT void Renderer_DestroyImageView(VkImageView* imageView);
DLL_EXPORT void Renderer_DestroyImage(VkImage* image);
DLL_EXPORT void Renderer_DestroySampler(VkSampler* sampler);
DLL_EXPORT void Renderer_DestroyPipeline(VkPipeline* pipeline);
DLL_EXPORT void Renderer_DestroyPipelineLayout(VkPipelineLayout* pipelineLayout);
DLL_EXPORT void Renderer_DestroyPipelineCache(VkPipelineCache* pipelineCache);
DLL_EXPORT void Renderer_FreeMemory(VkDeviceMemory* memory);

extern __declspec(dllexport) int SimpleTest();