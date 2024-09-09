#pragma once
#include <VulkanWindow.h>

void GameEngine_SDL_KeyboardKeyPressed(VulkanWindow* self, const SDL_Event* event);
void GameEngine_GLFW_KeyboardKeyPressed(VulkanWindow* self, int key, int action);