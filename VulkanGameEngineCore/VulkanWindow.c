#include "VulkanWindow.h"
#include "SDLWindow.h"
#include "GLFWWindow.h"
//#include "Win32Window.h"

VulkanWindow* vulkanWindow = { 0 };

VulkanWindow* Window_CreateWindow(Window_Type windowType, const char* WindowName, uint32_t width, uint32_t height)
{
    switch (windowType)
    {
    case Win32:
    {
        /*      Win32Window* window = (Win32Window*)malloc(sizeof(Win32Window));
              if (!window)
              {
                  fprintf(stderr, "Failed to allocate memory for Win32Window.\n");
                  return NULL;
              }
              window->base = (VulkanWindow*)malloc(sizeof(VulkanWindow));
              if (!window->base)
              {
                  fprintf(stderr, "Failed to allocate memory for VulkanWindow.\n");
                  free(window);
                  return NULL;
              }
              window->base->CreateGraphicsWindow = Window_Win32_CreateGraphicsWindow;
              window->base->PollEventHandler = Window_Win32_PollEventHandler;
              window->base->SwapBuffer = Window_Win32_SwapBuffer;
              window->base->GetInstanceExtensions = Window_Win32_GetInstanceExtensions;
              window->base->CreateSurface = Window_Win32_CreateSurface;
              window->base->DestroyWindow = Window_Win32_DestroyWindow;
              window->base->WindowShouldClose = Window_Win32_WindowShouldClose;

              window = window->base->CreateGraphicsWindow(window->base, WindowName, width, height);
              return window->base;*/
    }

    case SDL:
    {
        SDLWindow* window = (SDLWindow*)malloc(sizeof(SDLWindow));
        if (!window)
        {
            fprintf(stderr, "Failed to allocate memory for SDLWindow.\n");
            return NULL;
        }
        window->base = (VulkanWindow*)malloc(sizeof(VulkanWindow));
        if (!window->base)
        {
            fprintf(stderr, "Failed to allocate memory for VulkanWindow.\n");
            free(window);
            return NULL;
        }
        window->base->CreateGraphicsWindow = Window_SDL_CreateGraphicsWindow;
        window->base->PollEventHandler = Window_SDL_PollEventHandler;
        window->base->SwapBuffer = Window_SDL_SwapBuffer;
        window->base->GetInstanceExtensions = Window_SDL_GetInstanceExtensions;
        window->base->CreateSurface = Window_SDL_CreateSurface;
        window->base->GetFrameBufferSize = Window_SDL_GetFrameBufferSize;
        window->base->DestroyWindow = Window_SDL_DestroyWindow;
        window->base->WindowShouldClose = Window_SDL_WindowShouldClose;

        window = window->base->CreateGraphicsWindow(window->base, WindowName, width, height);
        return window->base;
    }
    case GLFW:
    {
        GLFWWindow* window = (GLFWWindow*)malloc(sizeof(GLFWWindow));
        if (!window)
        {
            fprintf(stderr, "Failed to allocate memory for GLFWWindow.\n");
            return NULL;
        }
        window->base = (GLFWWindow*)malloc(sizeof(GLFWWindow));
        if (!window->base)
        {
            fprintf(stderr, "Failed to allocate memory for VulkanWindow.\n");
            free(window);
            return NULL;
        }
        window->base->CreateGraphicsWindow = Window_GLFW_CreateGraphicsWindow;
        window->base->PollEventHandler = Window_GLFW_PollEventHandler;
        window->base->SwapBuffer = Window_GLFW_SwapBuffer;
        window->base->GetInstanceExtensions = Window_GLFW_GetInstanceExtensions;
        window->base->CreateSurface = Window_GLFW_CreateSurface;
        window->base->GetFrameBufferSize = Window_GLFW_GetFrameBufferSize;
        window->base->DestroyWindow = Window_GLFW_DestroyWindow;
        window->base->WindowShouldClose = Window_GLFW_WindowShouldClose;

        window = window->base->CreateGraphicsWindow(window->base, WindowName, width, height);
        return window->base;
    }
    default:
        return NULL;
    }
}