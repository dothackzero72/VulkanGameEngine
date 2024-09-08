#pragma once
#include <vulkan/vulkan_core.h>
#include <stdbool.h>
#include "Macro.h"

#ifdef __cplusplus
extern "C" {
#endif

typedef enum Window_Type
{
    Win32 = 0,
    SDL = 1,
    GLFW = 2
} Window_Type;

typedef struct VulkanWindow 
{
    Window_Type WindowType;
    void* WindowHandle;
    uint32_t    Width;
    uint32_t    Height;
    bool        FrameBufferResized;
    bool        ShouldClose;

    struct VulkanWindow* (*CreateGraphicsWindow)(struct VulkanWindow* self, const char* WindowName, uint32_t width, uint32_t height);
    void (*PollEventHandler)(struct VulkanWindow* self);
    void (*SwapBuffer)(struct VulkanWindow* self);
    const char** (*GetInstanceExtensions)(struct VulkanWindow* self, uint32_t* outExtensionCount, bool enableValidationLayers);
    void (*CreateSurface)(struct VulkanWindow* self, VkInstance* instance, VkSurfaceKHR* surface);
    void (*GetFrameBufferSize)(struct VulkanWindow* self, int* width, int* height);
    void (*DestroyWindow)(struct VulkanWindow* self);
    bool (*WindowShouldClose)(struct VulkanWindow* self);
} VulkanWindow;
extern VulkanWindow* vulkanWindow;

VulkanWindow* Window_CreateWindow(Window_Type windowType, const char* WindowName, uint32_t width, uint32_t height);
#ifdef __cplusplus
}
#endif