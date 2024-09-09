#pragma once
#include <VulkanWindow.h>

void GameEngine_SDL_MouseMoveEvent(VulkanWindow* self, const SDL_Event* event);
void GameEngine_SDL_MouseButtonPressedEvent(VulkanWindow* self, const SDL_Event* event);
void GameEngine_SDL_MouseWheelEvent(VulkanWindow* self, const SDL_Event* event);
void GameEngine_GLFW_MouseMoveEvent(struct VulkanWindow* self, double XPosition, double YPosition);
void GameEngine_GLFW_MouseButtonPressedEvent(struct VulkanWindow* self, int action, int button);
void GameEngine_GLFW_MouseWheelEvent(struct VulkanWindow* self, double Yoffset);
