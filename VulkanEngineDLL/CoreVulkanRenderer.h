#pragma once
#include <windows.h>
#include <stdbool.h>

extern "C"
{
#include "CVulkanRenderer.h"
}
#include "Macro.h"
#include "Typedef.h"
#include "VulkanError.h"

static const char* DeviceExtensionList[] =
{
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

static Vector<VkValidationFeatureEnableEXT> enabledList = { VK_VALIDATION_FEATURE_ENABLE_DEBUG_PRINTF_EXT,
															VK_VALIDATION_FEATURE_ENABLE_GPU_ASSISTED_RESERVE_BINDING_SLOT_EXT };

static Vector<VkValidationFeatureDisableEXT> disabledList = { VK_VALIDATION_FEATURE_DISABLE_THREAD_SAFETY_EXT,
																VK_VALIDATION_FEATURE_DISABLE_API_PARAMETERS_EXT,
																VK_VALIDATION_FEATURE_DISABLE_OBJECT_LIFETIMES_EXT,
																VK_VALIDATION_FEATURE_DISABLE_CORE_CHECKS_EXT };


typedef struct swapChainState
{
	uint32			   SwapChainImageCount;
	uint32			   GraphicsFamily;
	uint32			   PresentFamily;
	VkQueue			   GraphicsQueue;
	VkQueue			   PresentQueue;
	VkFormat           Format;
	VkColorSpaceKHR    ColorSpace;
	VkPresentModeKHR   PresentMode;

	Vector<VkImage> SwapChainImages;
	Vector<VkImageView> SwapChainImageViews;
	VkExtent2D SwapChainResolution;
	VkSwapchainKHR Swapchain;
}SwapChainState;

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

	Vector<VkFence> InFlightFences;
	Vector<VkSemaphore> AcquireImageSemaphores;
	Vector<VkSemaphore> PresentImageSemaphores;
	bool RebuildRendererFlag;
}RendererState;
DLL_EXPORT extern RendererState cRenderer;

DLL_EXPORT Vector<VkExtensionProperties> Renderer_GetDeviceExtensions(VkPhysicalDevice physicalDevice);
DLL_EXPORT Vector<VkSurfaceFormatKHR> Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
DLL_EXPORT Vector<VkPresentModeKHR> Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
DLL_EXPORT bool Renderer_GetRayTracingSupport();
DLL_EXPORT void Renderer_GetRendererFeatures(VkPhysicalDeviceVulkan11Features* physicalDeviceVulkan11Features);
DLL_EXPORT VkInstance Renderer_CreateVulkanInstance();
DLL_EXPORT VkDebugUtilsMessengerEXT Renderer_SetupDebugMessenger(VkInstance instance);
DLL_EXPORT VkPhysicalDeviceFeatures Renderer_GetPhysicalDeviceFeatures(VkPhysicalDevice physicalDevice);
DLL_EXPORT Vector<VkPhysicalDevice> Renderer_GetPhysicalDeviceList(VkInstance& instance);
DLL_EXPORT VkPhysicalDevice Renderer_SetUpPhysicalDevice(VkInstance instance, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily);
DLL_EXPORT VkResult Renderer_SetUpSemaphores(VkDevice device, Vector<VkFence>& inFlightFences, Vector<VkSemaphore>& acquireImageSemaphores, Vector<VkSemaphore>& presentImageSemaphores);
DLL_EXPORT VkDevice Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32 graphicsFamily, uint32 presentFamily);
DLL_EXPORT VkCommandPool Renderer_SetUpCommandPool(VkDevice device, uint32 graphicsFamily);
DLL_EXPORT VkResult Renderer_GetDeviceQueue(VkDevice device, uint32 graphicsFamily, uint32 presentFamily, VkQueue& graphicsQueue, VkQueue& presentQueue);

DLL_EXPORT VkResult SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& graphicsFamily, uint32& presentFamily);
DLL_EXPORT VkSurfaceCapabilitiesKHR SwapChain_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
DLL_EXPORT Vector<VkSurfaceFormatKHR> SwapChain_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
DLL_EXPORT Vector<VkPresentModeKHR> SwapChain_GetPhysicalDevicePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
DLL_EXPORT VkSwapchainKHR SwapChain_SetUpSwapChain(VkDevice device, VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily, uint32 width, uint32 height, uint32& swapChainImageCount);
DLL_EXPORT Vector<VkImage> SwapChain_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain, uint32 swapChainImageCount);

DLL_EXPORT Vector<VkImageView> SwapChain_SetUpSwapChainImageViews(VkDevice device, Vector<VkImage> swapChainImageList, VkSurfaceFormatKHR swapChainImageFormat);
DLL_EXPORT VkSurfaceFormatKHR SwapChain_FindSwapSurfaceFormat(Vector<VkSurfaceFormatKHR>& availableFormats);
DLL_EXPORT VkPresentModeKHR SwapChain_FindSwapPresentMode(Vector<VkPresentModeKHR>& availablePresentModes);