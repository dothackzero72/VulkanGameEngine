//#pragma once
//#include <SDL2/SDL.h>
//#include <SDL2/SDL_vulkan.h>
//#include <SDL2/SDL_events.h>
//#include <SDL2/SDL_video.h>
//#include <glfw/include/GLFW/glfw3.h>
//#include <vulkan/vulkan_core.h>
//#include <stdio.h>
//#include <stdbool.h>
//#include "VulkanWindow.h"
//
//#ifdef __cplusplus
//extern "C" {
//#endif
//
//typedef struct SDLWindow
//{
//    VulkanWindow* base;
//    SDL_Window* sdlWindowHandle;
//} SDLWindow;
//
//void Window_SDL_CreateGraphicsWindow(struct VulkanWindow* self, const char* WindowName, uint32_t width, uint32_t height);
//void Window_SDL_PollEventHandler(struct VulkanWindow* self);
//void Window_SDL_SwapBuffer(struct VulkanWindow* self);
//const char** Window_SDL_GetInstanceExtensions(struct VulkanWindow* self, uint32_t* outExtensionCount, bool enableValidationLayers);
//void Window_SDL_CreateSurface(struct VulkanWindow* self, VkInstance* instance, VkSurfaceKHR* surface);
//void Window_SDL_GetFrameBufferSize(struct VulkanWindow* self, int* width, int* height);
//void Window_SDL_DestroyWindow(struct VulkanWindow* self);
//bool Window_SDL_WindowShouldClose(struct VulkanWindow* self);
//
//
//#ifdef __cplusplus
//}
//#endif 