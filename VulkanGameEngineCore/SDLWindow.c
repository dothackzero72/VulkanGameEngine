#include "SDLWindow.h"

SDLWindow* Window_GetGLFWWindowPointer(VulkanWindow* self)
{
    SDLWindow* sdlWindow = (SDLWindow*)malloc(sizeof(SDLWindow));
    sdlWindow->sdlWindowHandle = (SDLWindow*)self->WindowHandle;
    sdlWindow->base = self;
    return sdlWindow;
}

VulkanWindow* Window_SDL_CreateGraphicsWindow(struct VulkanWindow* self, const char* WindowName, uint32_t width, uint32_t height)
{
    SDLWindow* sdlWindow = Window_GetGLFWWindowPointer(self);
    sdlWindow->base = self;
    sdlWindow->base->FrameBufferResized = false;
    sdlWindow->base->Width = width;
    sdlWindow->base->Height = height;
    sdlWindow->base->ShouldClose = false;

    if (SDL_Init(SDL_INIT_VIDEO) < 0)
    {
        fprintf(stderr, "Could not initialize SDL: %s\n", SDL_GetError());
        return;
    }

    sdlWindow->sdlWindowHandle = SDL_CreateWindow(WindowName, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, width, height, SDL_WINDOW_VULKAN | SDL_WINDOW_SHOWN | SDL_WINDOW_RESIZABLE);
    if (!sdlWindow->sdlWindowHandle)
    {
        fprintf(stderr, "Failed to create window: %s\n", SDL_GetError());
        SDL_Quit();
        return;
    }
    sdlWindow->base->WindowHandle = (void*)sdlWindow->sdlWindowHandle;
    return sdlWindow;
}

void Window_SDL_PollEventHandler(struct VulkanWindow* self)
{
    SDLWindow* sdlWindow = Window_GetGLFWWindowPointer(self);
    SDL_Event event;
    while (SDL_PollEvent(&event))
    {
        switch (event.type)
        {
            //case SDL_MOUSEMOTION:           GameEngine_SDL_MouseMoveEvent(&event); break;
            //case SDL_MOUSEBUTTONDOWN:       GameEngine_SDL_MouseButtonPressedEvent(&event); break;
            //case SDL_MOUSEBUTTONUP:         GameEngine_SDL_MouseButtonUnPressedEvent(&event); break;
            //case SDL_MOUSEWHEEL:            GameEngine_SDL_MouseWheelEvent(&event); break;
            //case SDL_KEYDOWN:               GameEngine_KeyboardKeyDown(&event); break;
            //case SDL_KEYUP:                 GameEngine_KeyboardKeyUp(&event); break;
            //case SDL_CONTROLLERAXISMOTION:  GameEngine_ControllerMoveAxis(&event); break;
            //case SDL_CONTROLLERBUTTONDOWN:  GameEngine_ControllerButtonDown(&event); break;
            //case SDL_CONTROLLERBUTTONUP:    GameEngine_ControllerButtonUp(&event); break;
        case SDL_WINDOWEVENT_MINIMIZED: SDL_MinimizeWindow(sdlWindow->sdlWindowHandle); break;
        case SDL_WINDOWEVENT_MAXIMIZED: SDL_MaximizeWindow(sdlWindow->sdlWindowHandle); break;
        case SDL_WINDOWEVENT_RESTORED:  SDL_RestoreWindow(sdlWindow->sdlWindowHandle); break;
        case SDL_QUIT:                  sdlWindow->base->ShouldClose = true; break;
        default: break;
        }
    }
}

void Window_SDL_SwapBuffer(struct VulkanWindow* self)
{

}

void Window_SDL_GetInstanceExtensions(struct VulkanWindow* self, uint32_t* pExtensionCount, VkExtensionProperties** extensionProperties)
{
    SDLWindow* sdlWindow = Window_GetGLFWWindowPointer(self);
    SDL_Vulkan_GetInstanceExtensions(NULL, pExtensionCount, NULL);
    const char** extensions = malloc(sizeof(const char*) * (*pExtensionCount + 1));
    if (!extensions)
    {
        *pExtensionCount = 0;
        *extensionProperties = NULL;
        return;
    }
    SDL_Vulkan_GetInstanceExtensions(NULL, pExtensionCount, extensions);
    extensions[*pExtensionCount] = VK_EXT_DEBUG_UTILS_EXTENSION_NAME;
    (*pExtensionCount)++;
    *extensionProperties = (VkExtensionProperties*)extensions;
}

void Window_SDL_CreateSurface(struct VulkanWindow* self, VkInstance* instance, VkSurfaceKHR* surface)
{
    SDLWindow* sdlWindow = Window_GetGLFWWindowPointer(self);
    SDL_Vulkan_CreateSurface(sdlWindow->sdlWindowHandle, *instance, surface);
}

void Window_SDL_GetFrameBufferSize(VulkanWindow* self, int* width, int* height)
{
    SDLWindow* sdlWindow = Window_GetGLFWWindowPointer(self);
    SDL_Vulkan_GetDrawableSize(sdlWindow->sdlWindowHandle, &*width, &*height);
}

void Window_SDL_DestroyWindow(struct VulkanWindow* self)
{
    SDLWindow* sdlWindow = Window_GetGLFWWindowPointer(self);
    SDL_DestroyWindow(sdlWindow->sdlWindowHandle);
    SDL_Quit();
}

bool Window_SDL_WindowShouldClose(VulkanWindow* self)
{
    SDLWindow* sdlWindow = Window_GetGLFWWindowPointer(self);
    return sdlWindow->base->ShouldClose;
}
