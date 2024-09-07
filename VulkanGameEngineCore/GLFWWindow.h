#ifndef GLFWWINDOW_H
#define GLFWWINDOW_H

#include <GLFW/glfw3.h>
#include "VulkanWindow.h"

typedef struct GLFWWindow
{
    VulkanWindow* base;
    GLFWwindow* glfwWindowHandle;
} GLFWWindow;

DLL_EXPORT VulkanWindow* Window_GLFW_CreateGraphicsWindow(struct VulkanWindow* self, const char* WindowName, uint32_t width, uint32_t height);
DLL_EXPORT void Window_GLFW_PollEventHandler(struct VulkanWindow* self);
DLL_EXPORT void Window_GLFW_FrameBufferResizeCallBack(GLFWwindow* window, int width, int height);
DLL_EXPORT void Window_GLFW_SwapBuffer(struct VulkanWindow* self);
DLL_EXPORT void Window_GLFW_GetInstanceExtensions(struct VulkanWindow* self, uint32_t* pExtensionCount, VkExtensionProperties** extensionProperties);
DLL_EXPORT void Window_GLFW_CreateSurface(struct VulkanWindow* self, VkInstance* instance, VkSurfaceKHR* surface);
DLL_EXPORT void Window_GLFW_GetFrameBufferSize(struct VulkanWindow* self, int* width, int* height);
DLL_EXPORT void Window_GLFW_DestroyWindow(struct VulkanWindow* self);
DLL_EXPORT bool Window_GLFW_WindowShouldClose(struct VulkanWindow* self);
#endif 