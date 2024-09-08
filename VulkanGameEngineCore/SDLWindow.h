#ifndef SDLWINDOW_H
#define SDLWINDOW_H

#include <SDL2/SDL.h>
#include <SDL2/SDL_vulkan.h>
#include <SDL2/SDL_syswm.h>
#include <SDL2/SDL_events.h>
#include <vulkan/vulkan_core.h>
#include <stdio.h>
#include <stdbool.h>
#include "VulkanWindow.h"

typedef struct SDLWindow
{
    VulkanWindow* base;
    SDL_Window* sdlWindowHandle;
} SDLWindow;

DLL_EXPORT SDLWindow* Window_GetSDLWindowPointer(VulkanWindow* self);
DLL_EXPORT VulkanWindow* Window_SDL_CreateGraphicsWindow(struct VulkanWindow* self, const char* WindowName, uint32_t width, uint32_t height);
DLL_EXPORT void Window_SDL_PollEventHandler(struct VulkanWindow* self);
DLL_EXPORT void Window_SDL_SwapBuffer(struct VulkanWindow* self);
DLL_EXPORT void Window_SDL_GetInstanceExtensions(struct VulkanWindow* self, uint32_t* pExtensionCount, VkExtensionProperties** extensionProperties);
DLL_EXPORT void Window_SDL_CreateSurface(struct VulkanWindow* self, VkInstance* instance, VkSurfaceKHR* surface);
DLL_EXPORT void Window_SDL_GetFrameBufferSize(struct VulkanWindow* self, int* width, int* height);
DLL_EXPORT void Window_SDL_DestroyWindow(struct VulkanWindow* self);
DLL_EXPORT bool Window_SDL_WindowShouldClose(struct VulkanWindow* self);
#endif 