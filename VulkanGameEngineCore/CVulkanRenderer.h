#pragma once
#include "DLLMain.h"
#include <windows.h>
#include <stdbool.h>

#include "Macro.h"
#include "CTypedef.h"
#include "VulkanSwapChain.h"
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
extern RendererState renderer;

VkInstance Renderer_CreateVulkanInstance();
VkDebugUtilsMessengerEXT Renderer_SetupDebugMessenger(VkInstance instance);
VkResult Renderer_SetUpPhysicalDevice(VkInstance instance, VkPhysicalDevice* physicalDevice, VkSurfaceKHR surface, VkPhysicalDeviceFeatures* physicalDeviceFeatures, uint32* graphicsFamily, uint32* presentFamily);
VkDevice Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32 graphicsFamily, uint32 presentFamily);
VkCommandPool Renderer_SetUpCommandPool(VkDevice device, uint32 graphicsFamily);
VkResult Renderer_SetUpSemaphores(VkDevice device, VkFence** inFlightFences, VkSemaphore** acquireImageSemaphores, VkSemaphore** presentImageSemaphores, int maxFramesInFlight);
VkResult Renderer_GetDeviceQueue(VkDevice device, uint32 graphicsFamily, uint32 presentFamily, VkQueue* graphicsQueue, VkQueue* presentQueue);
 
VkResult Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceFormatKHR* surfaceFormat, uint32* surfaceFormatCount);
VkResult Renderer_GetPresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkPresentModeKHR* presentMode, int32* presentModeCount);
bool Renderer_GetRayTracingSupport();
void Renderer_GetRendererFeatures(VkPhysicalDeviceVulkan11Features* physicalDeviceVulkan11Features);
void Renderer_GetWin32Extensions(uint32_t* extensionCount, const char*** enabledExtensions);

VkResult CreateDebugUtilsMessengerEXT(VkInstance instance, const VkDebugUtilsMessengerCreateInfoEXT* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDebugUtilsMessengerEXT* pDebugMessenger);
void DestroyDebugUtilsMessengerEXT(VkInstance instance, const VkAllocationCallbacks* pAllocator);
VKAPI_ATTR VkBool32 VKAPI_CALL Vulkan_DebugCallBack(VkDebugUtilsMessageSeverityFlagBitsEXT MessageSeverity, VkDebugUtilsMessageTypeFlagsEXT MessageType, const VkDebugUtilsMessengerCallbackDataEXT* CallBackData, void* UserData);
VkExtensionProperties* Renderer_GetDeviceExtensions(VkPhysicalDevice physicalDevice, uint32* deviceExtensionCountPtr);

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
void Renderer_UpdateDescriptorSet(VkDevice device, VkWriteDescriptorSet* writeDescriptorSet, uint32 count);

VkResult Renderer_StartFrame(VkDevice device, VkSwapchainKHR swapChain, VkFence* pFence, VkSemaphore* pAcquireImageSemaphore, uint32* pImageIndex, uint32* pCommandIndex, bool* pRebuildRendererFlag);
VkResult Renderer_EndFrame(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue, VkQueue presentQueue, uint32 commandIndex, uint32 imageIndex, VkCommandBuffer* pCommandBufferSubmitList, uint32 commandBufferCount, bool* rebuildRendererFlag);
VkResult Renderer_SubmitDraw(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue, VkQueue presentQueue, uint32 commandIndex, uint32 imageIndex, VkCommandBuffer* pCommandBufferSubmitList, uint32 commandBufferCount, bool* rebuildRendererFlag);

uint32 Renderer_GetMemoryType(VkPhysicalDevice physicalDevice, uint32_t typeFilter, VkMemoryPropertyFlags properties);

VkResult Renderer_BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo);
VkResult Renderer_EndCommandBuffer(VkCommandBuffer* pCommandBufferList);
VkCommandBuffer Renderer_BeginSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool);
VkResult Renderer_EndSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkCommandBuffer commandBuffer);

bool Array_RendererExtensionPropertiesSearch(VkExtensionProperties* array, uint32 arrayCount, const char* target);

void Renderer_DestroyFences(VkDevice* device, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, VkFence* fences, size_t semaphoreCount);
void Renderer_DestroyCommandPool(VkDevice* device, VkCommandPool* commandPool);
void Renderer_DestroyDevice(VkDevice* device);
void Renderer_DestroySurface(VkInstance* instance, VkSurfaceKHR* surface);
void Renderer_DestroyDebugger(VkInstance* instance);
void Renderer_DestroyInstance(VkInstance* instance);
void Renderer_DestroyRenderPass(VkDevice device, VkRenderPass* renderPass);
void Renderer_DestroyFrameBuffers(VkDevice device, VkFramebuffer* frameBufferList, uint32 count);
void Renderer_DestroyDescriptorPool(VkDevice device, VkDescriptorPool* descriptorPool);
void Renderer_DestroyDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayout);
void Renderer_DestroyCommandBuffers(VkDevice device, VkCommandPool* commandPool, VkCommandBuffer* commandBufferList, uint32 count);
void Renderer_DestroyBuffer(VkDevice device, VkBuffer* buffer);
void Renderer_FreeMemory(VkDevice device, VkDeviceMemory* memory);
void Renderer_DestroySwapChainImageView(VkDevice device, VkImageView* pSwapChainImageViewList, uint32 count);
void Renderer_DestroySwapChain(VkDevice device, VkSwapchainKHR* swapChain);
void Renderer_DestroyImageView(VkDevice device, VkImageView* imageView);
void Renderer_DestroyImage(VkDevice device, VkImage* image);
void Renderer_DestroySampler(VkDevice device, VkSampler* sampler);
void Renderer_DestroyPipeline(VkDevice device, VkPipeline* pipeline);
void Renderer_DestroyPipelineLayout(VkDevice device, VkPipelineLayout* pipelineLayout);
void Renderer_DestroyPipelineCache(VkDevice device, VkPipelineCache* pipelineCache);
int Renderer_SimpleTestLIB();
#ifdef __cplusplus
}
#endif