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
    VK_KHR_SHADER_NON_SEMANTIC_INFO_EXTENSION_NAME
};

static const char* ValidationLayers[] = { "VK_LAYER_KHRONOS_validation" };

static VkValidationFeatureEnableEXT enabledList[] = { VK_VALIDATION_FEATURE_ENABLE_DEBUG_PRINTF_EXT,
                                                      VK_VALIDATION_FEATURE_ENABLE_GPU_ASSISTED_RESERVE_BINDING_SLOT_EXT };

static VkValidationFeatureDisableEXT disabledList[] = { VK_VALIDATION_FEATURE_DISABLE_THREAD_SAFETY_EXT,
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

	List<VkImage> SwapChainImages;
	List<VkImageView> SwapChainImageViews;
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

	List<VkFence> InFlightFences;
	List<VkSemaphore> AcquireImageSemaphores;
	List<VkSemaphore> PresentImageSemaphores;
	bool RebuildRendererFlag;
}RendererState;
extern RendererState cRenderer;



List<VkExtensionProperties> Renderer_GetDeviceExtensions(VkPhysicalDevice physicalDevice);
List<VkSurfaceFormatKHR> Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
List<VkPresentModeKHR> Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
bool Renderer_GetRayTracingSupport();
void Renderer_GetRendererFeatures(VkPhysicalDeviceVulkan11Features* physicalDeviceVulkan11Features);
VkInstance Renderer_CreateVulkanInstance();
VkDebugUtilsMessengerEXT Renderer_SetupDebugMessenger(VkInstance instance);
VkPhysicalDeviceFeatures Renderer_GetPhysicalDeviceFeatures(VkPhysicalDevice physicalDevice);
List<VkPhysicalDevice> Renderer_GetPhysicalDeviceList(VkInstance& instance);
VkPhysicalDevice Renderer_SetUpPhysicalDevice(VkInstance instance, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily);
VkResult Renderer_SetUpSemaphores(VkDevice device, List<VkFence>& inFlightFences, List<VkSemaphore>& acquireImageSemaphores, List<VkSemaphore>& presentImageSemaphores);
VkDevice Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32 graphicsFamily, uint32 presentFamily);
VkCommandPool Renderer_SetUpCommandPool(VkDevice device, uint32 graphicsFamily);
VkResult Renderer_GetDeviceQueue(VkDevice device, uint32 graphicsFamily, uint32 presentFamily, VkQueue& graphicsQueue, VkQueue& presentQueue);

VkResult SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& graphicsFamily, uint32& presentFamily);
VkSurfaceCapabilitiesKHR SwapChain_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
List<VkSurfaceFormatKHR> SwapChain_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
List<VkPresentModeKHR> SwapChain_GetPhysicalDevicePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface);
VkSwapchainKHR SwapChain_SetUpSwapChain(VkDevice device, VkSurfaceKHR surface, VkSurfaceCapabilitiesKHR surfaceCapabilities, VkSurfaceFormatKHR SwapChainImageFormat, VkPresentModeKHR SwapChainPresentMode, uint32 graphicsFamily, uint32 presentFamily, uint32 width, uint32 height, uint32* swapChainImageCount);
List<VkImage> SwapChain_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain);

List<VkImageView> SwapChain_SetUpSwapChainImageViews(VkDevice device, List<VkImage> swapChainImageList, VkSurfaceFormatKHR& swapChainImageFormat);
VkSurfaceFormatKHR SwapChain_FindSwapSurfaceFormat(List<VkSurfaceFormatKHR>& availableFormats);
VkPresentModeKHR SwapChain_FindSwapPresentMode(List<VkPresentModeKHR>& availablePresentModes);