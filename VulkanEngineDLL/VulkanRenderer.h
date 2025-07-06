#pragma once
#include <windows.h>
#include <stdbool.h>

#include "DLL.h"
#include "Macro.h"
#include "CTypedef.h"
#include "VulkanError.h"
#include <TypeDef.h>
#include <vulkan/vulkan_win32.h>

static const char* InstanceExtensionList[] = {
    VK_KHR_SURFACE_EXTENSION_NAME,      // Required for surface creation
    VK_KHR_WIN32_SURFACE_EXTENSION_NAME, // Required for Win32 surface creation
    VK_EXT_DEBUG_UTILS_EXTENSION_NAME   // Required for debug messenger
};
static const char* DeviceExtensionList[] = {
    VK_KHR_SWAPCHAIN_EXTENSION_NAME,   // For swapchain creation
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
    // Removed VK_VALIDATION_FEATURE_DISABLE_CORE_CHECKS_EXT
};

#ifdef __cplusplus
extern "C" {
#endif
DLL_EXPORT VkInstance Renderer_CreateVulkanInstance2();
DLL_EXPORT VkSurfaceKHR Renderer_CreateVulkanSurface2(void* windowHandle, VkInstance instance);

VKAPI_ATTR VkBool32 VKAPI_CALL Vulkan_DebugCallBack2(VkDebugUtilsMessageSeverityFlagBitsEXT MessageSeverity, VkDebugUtilsMessageTypeFlagsEXT MessageType, const VkDebugUtilsMessengerCallbackDataEXT* CallBackData, void* UserData);
VkResult Renderer_GetWin32Extensions2(uint32_t* extensionCount, std::vector<std::string>& enabledExtensions);
#ifdef __cplusplus
}
#endif