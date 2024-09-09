#pragma once
#include <VulkanWindow.h>

KeyCode ConvertGLFWKeyToUnified(int glfwKey);
KeyCode ConvertSDLKeyToUnified(SDL_Keycode sdlKey);
void GameEngine_SDL_KeyboardKeyPressed(VulkanWindow* self, const SDL_Event* event);
void GameEngine_GLFW_KeyboardKeyPressed(GLFWwindow* window, int key, int scancode, int action, int mods);