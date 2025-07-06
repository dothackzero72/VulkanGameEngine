#pragma once
#include <windows.h>
#include <stdbool.h>
#include <vulkan/vulkan_core.h>
#include <vulkan/vulkan_win32.h>
#include "DLL.h"
#include "Macro.h"
#include "Typedef.h"
#include "VulkanError.h"

static const char* InstanceExtensionList[] = {
	VK_KHR_SURFACE_EXTENSION_NAME,
	VK_KHR_WIN32_SURFACE_EXTENSION_NAME,
	VK_EXT_DEBUG_UTILS_EXTENSION_NAME
};
static const char* DeviceExtensionList[] = {
	VK_KHR_SWAPCHAIN_EXTENSION_NAME,
	VK_KHR_MAINTENANCE3_EXTENSION_NAME,
	VK_KHR_BUFFER_DEVICE_ADDRESS_EXTENSION_NAME,
	VK_EXT_DESCRIPTOR_INDEXING_EXTENSION_NAME,
	VK_KHR_SPIRV_1_4_EXTENSION_NAME,
	VK_KHR_SHADER_FLOAT_CONTROLS_EXTENSION_NAME,
	VK_KHR_SHADER_NON_SEMANTIC_INFO_EXTENSION_NAME,
	VK_EXT_ROBUSTNESS_2_EXTENSION_NAME
};

static const char* ValidationLayers[] = { "VK_LAYER_KHRONOS_validation" };

static Vector<VkValidationFeatureEnableEXT> enabledList = {
	VK_VALIDATION_FEATURE_ENABLE_DEBUG_PRINTF_EXT,
	VK_VALIDATION_FEATURE_ENABLE_GPU_ASSISTED_RESERVE_BINDING_SLOT_EXT
};

static Vector<VkValidationFeatureDisableEXT> disabledList = {
	VK_VALIDATION_FEATURE_DISABLE_THREAD_SAFETY_EXT,
	VK_VALIDATION_FEATURE_DISABLE_API_PARAMETERS_EXT,
	VK_VALIDATION_FEATURE_DISABLE_OBJECT_LIFETIMES_EXT
};

struct GPUIncludes
{
	VkDescriptorBufferInfo* VertexProperties;
	VkDescriptorBufferInfo* IndexProperties;
	VkDescriptorBufferInfo* TransformProperties;
	VkDescriptorBufferInfo* MeshProperties;
	VkDescriptorBufferInfo* LevelLayerMeshProperties;
	VkDescriptorImageInfo*  TexturePropertiesList;
	VkDescriptorBufferInfo* MaterialProperties;
	size_t VertexPropertiesCount;
	size_t IndexPropertiesCount;
	size_t TransformPropertiesCount;
	size_t MeshPropertiesCount;
	size_t LevelLayerMeshPropertiesCount;
	size_t TexturePropertiesListCount;
	size_t MaterialPropertiesCount;
};

struct GraphicsRenderer
{
	VkInstance         Instance;
	VkDevice           Device;
	VkPhysicalDevice   PhysicalDevice;
	VkSurfaceKHR       Surface;
	VkCommandPool      CommandPool;
	VkDebugUtilsMessengerEXT DebugMessenger;

	VkFence* InFlightFences;
	VkSemaphore* AcquireImageSemaphores;
	VkSemaphore* PresentImageSemaphores;
	VkImage* SwapChainImages;
	VkImageView* SwapChainImageViews;
	VkExtent2D         SwapChainResolution;
	VkSwapchainKHR     Swapchain;

	size_t			   SwapChainImageCount;
	size_t			   ImageIndex;
	size_t			   CommandIndex;
	uint32			   GraphicsFamily;
	uint32			   PresentFamily;

	VkQueue			   GraphicsQueue;
	VkQueue			   PresentQueue;
	VkFormat           Format;
	VkColorSpaceKHR    ColorSpace;
	VkPresentModeKHR   PresentMode;

	bool               RebuildRendererFlag;
};

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

#ifdef __cplusplus
extern "C" {
#endif
	DLL_EXPORT GraphicsRenderer Renderer_RendererSetUp(void* windowHandle);
	DLL_EXPORT VkCommandBuffer Renderer_BeginSingleTimeCommands(VkDevice device, VkCommandPool commandPool);
	DLL_EXPORT VkResult Renderer_EndSingleTimeCommands(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkCommandBuffer commandBuffer);
	DLL_EXPORT void Renderer_DestroyRenderer(GraphicsRenderer& renderer);

	DLL_EXPORT VkResult CreateDebugUtilsMessengerEXT(VkInstance instance, const VkDebugUtilsMessengerCreateInfoEXT* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDebugUtilsMessengerEXT* pDebugMessenger);
	DLL_EXPORT void DestroyDebugUtilsMessengerEXT(VkInstance instance, const VkAllocationCallbacks* pAllocator);
	VKAPI_ATTR VkBool32 VKAPI_CALL Vulkan_DebugCallBack(VkDebugUtilsMessageSeverityFlagBitsEXT MessageSeverity, VkDebugUtilsMessageTypeFlagsEXT MessageType, const VkDebugUtilsMessengerCallbackDataEXT* CallBackData, void* UserData);


	DLL_EXPORT VkResult Renderer_CreateRenderPass(VkDevice device, RenderPassCreateInfoStruct* renderPassCreateInfo);
	DLL_EXPORT VkResult Renderer_CreateDescriptorPool(VkDevice device, VkDescriptorPool* descriptorPool, VkDescriptorPoolCreateInfo* descriptorPoolCreateInfo);
	DLL_EXPORT VkResult Renderer_CreateDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayout, VkDescriptorSetLayoutCreateInfo* descriptorSetLayoutCreateInfo);
	DLL_EXPORT VkResult Renderer_CreatePipelineLayout(VkDevice device, VkPipelineLayout* pipelineLayout, VkPipelineLayoutCreateInfo* pipelineLayoutCreateInfo);
	DLL_EXPORT VkResult Renderer_AllocateDescriptorSets(VkDevice device, VkDescriptorSet* descriptorSet, VkDescriptorSetAllocateInfo* descriptorSetAllocateInfo);
	DLL_EXPORT VkResult Renderer_AllocateCommandBuffers(VkDevice device, VkCommandBuffer* commandBuffer, VkCommandBufferAllocateInfo* commandBufferAllocateInfo);
	DLL_EXPORT VkResult Renderer_CreateGraphicsPipelines(VkDevice device, VkPipeline* graphicPipeline, VkGraphicsPipelineCreateInfo* createGraphicPipelines, uint32 createGraphicPipelinesCount);
	DLL_EXPORT VkResult Renderer_CreateCommandPool(VkDevice device, VkCommandPool* commandPool, VkCommandPoolCreateInfo* commandPoolInfo);
	DLL_EXPORT VkResult Renderer_CreateSemaphores(VkDevice device, VkFence* inFlightFences, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, int maxFramesInFlight);
	DLL_EXPORT void Renderer_UpdateDescriptorSet(VkDevice device, VkWriteDescriptorSet* writeDescriptorSet, uint32 count);

	DLL_EXPORT VkResult Renderer_StartFrame(VkDevice device, VkSwapchainKHR swapChain, VkFence* fenceList, VkSemaphore* acquireImageSemaphoreList, size_t* pImageIndex, size_t* pCommandIndex, bool* pRebuildRendererFlag);
	DLL_EXPORT VkResult Renderer_EndFrame(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue, VkQueue presentQueue, size_t commandIndex, uint32 imageIndex, VkCommandBuffer* pCommandBufferSubmitList, uint32_t commandBufferCount, bool* rebuildRendererFlag);

	DLL_EXPORT uint32 Renderer_GetMemoryType(VkPhysicalDevice physicalDevice, uint32_t typeFilter, VkMemoryPropertyFlags properties);

	DLL_EXPORT VkResult Renderer_BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo);
	DLL_EXPORT VkResult Renderer_EndCommandBuffer(VkCommandBuffer* pCommandBufferList);
	DLL_EXPORT VkCommandBuffer Renderer_BeginSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool);
	DLL_EXPORT VkResult Renderer_EndSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkCommandBuffer commandBuffer);

	void Renderer_DestroyFences(VkDevice device, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, VkFence* fences, size_t semaphoreCount);
	void Renderer_DestroyCommandPool(VkDevice device, VkCommandPool* commandPool);
	void Renderer_DestroyDevice(VkDevice device);
	void Renderer_DestroySurface(VkInstance instance, VkSurfaceKHR* surface);
	void Renderer_DestroyDebugger(VkInstance* instance, VkDebugUtilsMessengerEXT debugUtilsMessengerEXT);
	void Renderer_DestroyInstance(VkInstance* instance);
	DLL_EXPORT void Renderer_DestroyRenderPass(VkDevice device, VkRenderPass* renderPass);
	DLL_EXPORT void Renderer_DestroyFrameBuffers(VkDevice device, VkFramebuffer* frameBufferList, uint32 count);
	void Renderer_DestroyDescriptorPool(VkDevice device, VkDescriptorPool* descriptorPool);
	void Renderer_DestroyDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayout);
	DLL_EXPORT void Renderer_DestroyCommandBuffers(VkDevice device, VkCommandPool* commandPool, VkCommandBuffer* commandBufferList, uint32 count);
	DLL_EXPORT void Renderer_DestroyBuffer(VkDevice device, VkBuffer* buffer);
	void Renderer_FreeDeviceMemory(VkDevice device, VkDeviceMemory* memory);
	void Renderer_DestroySwapChainImageView(VkDevice device, VkSurfaceKHR surface, VkImageView* pSwapChainImageViewList, uint32 count);
	void Renderer_DestroySwapChain(VkDevice device, VkSwapchainKHR* swapChain);
	void Renderer_DestroyImageView(VkDevice device, VkImageView* imageView);
	void Renderer_DestroyImage(VkDevice device, VkImage* image);
	void Renderer_DestroySampler(VkDevice device, VkSampler* sampler);
	void Renderer_DestroyPipeline(VkDevice device, VkPipeline* pipeline);
	void Renderer_DestroyPipelineLayout(VkDevice device, VkPipelineLayout* pipelineLayout);
	void Renderer_DestroyPipelineCache(VkDevice device, VkPipelineCache* pipelineCache);

	VkResult Renderer_GetWin32Extensions(uint32_t* extensionCount, std::vector<std::string>& enabledExtensions);
	VkSurfaceKHR Renderer_CreateVulkanSurface(void* windowHandle, VkInstance instance);
	bool Renderer_GetRayTracingSupport();
	void Renderer_GetRendererFeatures(VkPhysicalDeviceVulkan11Features * physicalDeviceVulkan11Features);
	VkInstance Renderer_CreateVulkanInstance();
	VkDebugUtilsMessengerEXT Renderer_SetupDebugMessenger(VkInstance instance);
	VkPhysicalDeviceFeatures Renderer_GetPhysicalDeviceFeatures(VkPhysicalDevice physicalDevice);
	VkPhysicalDevice Renderer_SetUpPhysicalDevice(VkInstance instance, VkSurfaceKHR surface, uint32 & graphicsFamily, uint32 & presentFamily);
	VkResult Renderer_SetUpSemaphores(VkDevice device, VkFence* inFlightFences, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, size_t swapChainImageCount);
	VkDevice Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32 graphicsFamily, uint32 presentFamily);
	VkCommandPool Renderer_SetUpCommandPool(VkDevice device, uint32 graphicsFamily);
	VkResult Renderer_GetDeviceQueue(VkDevice device, uint32 graphicsFamily, uint32 presentFamily, VkQueue & graphicsQueue, VkQueue & presentQueue);

	VkResult SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32 & graphicsFamily, uint32 & presentFamily);
	VkSwapchainKHR SwapChain_SetUpSwapChain(VkDevice device, VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily, uint32 width, uint32 height, size_t& swapChainImageCount);
	VkImage* SwapChain_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain, uint32 swapChainImageCount);

	VkImageView* SwapChain_SetUpSwapChainImageViews(VkDevice device, VkImage* swapChainImageList, size_t swapChainImageCount, VkSurfaceFormatKHR swapChainImageFormat);
	VkSurfaceFormatKHR SwapChain_FindSwapSurfaceFormat(Vector<VkSurfaceFormatKHR>&availableFormats);
	VkPresentModeKHR SwapChain_FindSwapPresentMode(Vector<VkPresentModeKHR>&availablePresentModes);

	VkResult Renderer_SetUpSwapChain(void* windowHandle, GraphicsRenderer& renderState);
#ifdef __cplusplus
}
#endif

Vector<VkExtensionProperties> Renderer_GetDeviceExtensions(VkPhysicalDevice physicalDevice);
Vector<VkSurfaceFormatKHR> Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
Vector<VkPresentModeKHR> Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
Vector<VkPhysicalDevice> Renderer_GetPhysicalDeviceList(VkInstance& instance);
VkSurfaceCapabilitiesKHR SwapChain_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
Vector<VkSurfaceFormatKHR> SwapChain_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
Vector<VkPresentModeKHR> SwapChain_GetPhysicalDevicePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);