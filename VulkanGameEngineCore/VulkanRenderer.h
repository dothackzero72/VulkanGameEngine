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
}Renderer_RenderPassCreateInfoStruct;


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

VkResult Renderer_RendererSetUp();
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

VkResult Renderer_CreateCommandBuffers(VkCommandBuffer* commandBufferList);
VkResult Renderer_CreateFrameBuffer(VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo);
VkResult Renderer_CreateRenderPass(Renderer_RenderPassCreateInfoStruct* renderPassCreateInfo);
VkResult Renderer_CreateDescriptorPool(VkDescriptorPool* descriptorPool, VkDescriptorPoolCreateInfo* descriptorPoolCreateInfo);
VkResult Renderer_CreateDescriptorSetLayout(VkDescriptorSetLayout* descriptorSetLayout, VkDescriptorSetLayoutCreateInfo* descriptorSetLayoutCreateInfo);
VkResult Renderer_CreatePipelineLayout(VkPipelineLayout* pipelineLayout, VkPipelineLayoutCreateInfo* pipelineLayoutCreateInfo);
VkResult Renderer_AllocateDescriptorSets(VkDescriptorSet* descriptorSet, VkDescriptorSetAllocateInfo* descriptorSetAllocateInfo);
VkResult Renderer_AllocateCommandBuffers(VkCommandBuffer* commandBuffer, VkCommandBufferAllocateInfo* commandBufferAllocateInfo);
VkResult Renderer_CreateGraphicsPipelines(VkPipeline* graphicPipeline, VkGraphicsPipelineCreateInfo* createGraphicPipelines, uint32 createGraphicPipelinesCount);
VkResult Renderer_CreateCommandPool(VkCommandPool* commandPool, VkCommandPoolCreateInfo* commandPoolInfo);
void Renderer_UpdateDescriptorSet(VkWriteDescriptorSet* writeDescriptorSet, uint32 count);
VkResult Renderer_RebuildSwapChain();

VkResult Renderer_StartFrame();
VkResult Renderer_EndFrame(VkCommandBuffer* pCommandBufferSubmitList, uint32 commandBufferCount);
VkResult Renderer_BeginCommandBuffer(VkCommandBuffer* pCommandBuffer, VkCommandBufferBeginInfo* commandBufferBeginInfo);
VkResult Renderer_EndCommandBuffer(VkCommandBuffer* pCommandBuffer);
VkResult Renderer_SubmitDraw(VkCommandBuffer* pCommandBufferSubmitList);

uint32 Renderer_GetMemoryType(uint32 typeFilter, VkMemoryPropertyFlags properties);

VkCommandBuffer Renderer_BeginSingleUseCommandBuffer();
VkResult Renderer_EndSingleUseCommandBuffer(VkCommandBuffer commandBuffer);

bool Array_RendererExtensionPropertiesSearch(VkExtensionProperties* array, uint32 arrayCount, const char* target);

void Renderer_DestroyRenderer();
void Renderer_DestroyCommandPool();
void Renderer_DestroyFences();
void Renderer_DestroyDevice();
void Renderer_DestroySurface();
void Renderer_DestroyDebugger();
void Renderer_DestroyInstance();
void Renderer_CSDestroyFences(VkDevice* device, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, VkFence* fences, size_t semaphoreCount, int bufferCount);
void Renderer_CSDestroyCommandPool(VkDevice* device, VkCommandBuffer* commandPool);
void Renderer_CSDestroyDevice(VkDevice* device);
void Renderer_CSDestroySurface(VkInstance* instance, VkSurfaceKHR* surface);
void Renderer_CSDestroyDebugger(VkInstance* instance);
void Renderer_CSDestroyInstance(VkInstance* instance);
void Renderer_DestroyRenderPass(VkRenderPass* renderPass);
void Renderer_DestroyFrameBuffers(VkFramebuffer* frameBufferList);
void Renderer_DestroyDescriptorPool(VkDescriptorPool* descriptorPool);
void Renderer_DestroyDescriptorSetLayout(VkDescriptorSetLayout* descriptorSetLayout);
void Renderer_DestroyCommandBuffers(VkCommandPool* commandPool, VkCommandBuffer* commandBufferList);
void Renderer_DestroyCommnadPool(VkCommandPool* commandPool);
void Renderer_DestroyBuffer(VkBuffer* buffer);
void Renderer_FreeMemory(VkDeviceMemory* memory);
void Renderer_DestroyImageView(VkImageView* imageView);
void Renderer_DestroyImage(VkImage* image);
void Renderer_DestroySampler(VkSampler* sampler);
void Renderer_DestroyPipeline(VkPipeline* pipeline);
void Renderer_DestroyPipelineLayout(VkPipelineLayout* pipelineLayout);
void Renderer_DestroyPipelineCache(VkPipelineCache* pipelineCache);
int Renderer_SimpleTestLIB();
#ifdef __cplusplus
}
#endif