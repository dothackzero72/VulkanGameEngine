//#include "DLLWindow.h"
//
//void Window_SDL_CreateGraphicsWindow(struct VulkanWindow* self, const char* WindowName, uint32_t width, uint32_t height)
//{
//
//    self->WindowType = SDL;
//    self->FrameBufferResized = false;
//    self->Width = width;
//    self->Height = height;
//    self->ShouldClose = false;
//    self->mouse.X = 0;
//    self->mouse.Y = 0;
//    self->mouse.WheelOffset = 0;
//    memset(self->mouse.MouseButtonState, 0, sizeof(self->mouse.MouseButtonState));
//    memset(self->keyboard.KeyPressed, 0, sizeof(self->keyboard.KeyPressed));
//
//    if (SDL_Init(SDL_INIT_VIDEO) < 0)
//    {
//        fprintf(stderr, "Could not initialize SDL: %s\n", SDL_GetError());
//        return;
//    }
//
//    SDL_WindowFlags.
//    self->WindowHandle = SDL_CreateWindow(WindowName, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, width, height, SDL_WindowFlags. | SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
//    if (!self->WindowHandle)
//    {
//        fprintf(stderr, "Failed to create window: %s\n", SDL_GetError());
//        SDL_Quit();
//        return;
//    }
//}
//
//void Window_SDL_PollEventHandler(struct VulkanWindow* self)
//{
//    SDL_Event event;
//    while (SDL_PollEvent(&event))
//    {
//        //switch (event.type)
//        //{
//        //case SDL_MOUSEMOTION:           GameEngine_SDL_MouseMoveEvent(self, &event); break;
//        //case SDL_MOUSEBUTTONDOWN:       GameEngine_SDL_MouseButtonPressedEvent(self, &event); break;
//        //case SDL_MOUSEBUTTONUP:         GameEngine_SDL_MouseButtonReleasedEvent(self, &event); break;
//        //case SDL_MOUSEWHEEL:            GameEngine_SDL_MouseWheelEvent(self, &event); break;
//        //case SDL_KEYDOWN:               GameEngine_SDL_KeyboardKeyPressed(self, &event); break;
//        //case SDL_KEYUP:                 GameEngine_SDL_KeyboardKeyReleased(self, &event); break;
//        //    //case SDL_CONTROLLERAXISMOTION:  GameEngine_ControllerMoveAxis(&event); break;
//        //    //case SDL_CONTROLLERBUTTONDOWN:  GameEngine_ControllerButtonDown(&event); break;
//        //    //case SDL_CONTROLLERBUTTONUP:    GameEngine_ControllerButtonUp(&event); break;
//        //case SDL_WINDOWEVENT_MINIMIZED: SDL_MinimizeWindow(self->WindowHandle); break;
//        //case SDL_WINDOWEVENT_MAXIMIZED: SDL_MaximizeWindow(self->WindowHandle); break;
//        //case SDL_WINDOWEVENT_RESTORED:  SDL_RestoreWindow(self->WindowHandle); break;
//        //case SDL_QUIT:                  self->ShouldClose = true; break;
//        //default: break;
//        //}
//    }
//}
//
//void Window_SDL_SwapBuffer(struct VulkanWindow* self)
//{
//
//}
//
//const char** Window_SDL_GetInstanceExtensions(struct VulkanWindow* self, uint32_t* outExtensionCount, bool enableValidationLayers)
//{
//    uint32_t sdlExtensionCount;
//
//    SDL_Vulkan_GetInstanceExtensions(NULL, &sdlExtensionCount, NULL);
//
//    size_t totalCount = sdlExtensionCount + (enableValidationLayers ? 1 : 0);
//    const char** extensions = (const char**)malloc(totalCount * sizeof(const char*));
//    if (!extensions) {
//        fprintf(stderr, "Failed to allocate memory for extensions\n");
//        return NULL;
//    }
//    SDL_Vulkan_GetInstanceExtensions(NULL, outExtensionCount, extensions);
//
//    if (enableValidationLayers) {
//        extensions[sdlExtensionCount] = VK_EXT_DEBUG_UTILS_EXTENSION_NAME;
//    }
//
//    *outExtensionCount = totalCount;
//    return extensions;
//}
//
//void Window_SDL_CreateSurface(struct VulkanWindow* self, VkInstance* instance, VkSurfaceKHR* surface)
//{
//    SDL_Vulkan_CreateSurface(self->WindowHandle, *instance, surface);
//}
//
//void Window_SDL_GetFrameBufferSize(VulkanWindow* self, int* width, int* height)
//{
//    SDL_Vulkan_GetDrawableSize(self->WindowHandle, &*width, &*height);
//}
//
//void Window_SDL_DestroyWindow(struct VulkanWindow* self)
//{
//    SDL_DestroyWindow(self->WindowHandle);
//    SDL_Quit();
//}
//
//bool Window_SDL_WindowShouldClose(VulkanWindow* self)
//{
//    return self->ShouldClose;
//}