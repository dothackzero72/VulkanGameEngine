#include "Mouse.h"
#include <glfw/include/GLFW/glfw3.h>

void GameEngine_SDL_MouseMoveEvent(VulkanWindow* self, const SDL_Event* event)
{
	self->mouse.X = event->motion.x;
	self->mouse.Y = event->motion.y;
}

void GameEngine_SDL_MouseButtonPressedEvent(VulkanWindow* self, const SDL_Event* event)
{
    switch (event->button.button)
    {
		case SDL_BUTTON_LEFT:   self->mouse.MouseButtonState[MB_Left-1] = MS_PRESSED;  break;
		case SDL_BUTTON_MIDDLE: self->mouse.MouseButtonState[MB_Middle-1] = MS_PRESSED; break;
		case SDL_BUTTON_RIGHT:  self->mouse.MouseButtonState[MB_Right-1] = MS_PRESSED; break;
        default: break;
    }
}

void GameEngine_SDL_MouseWheelEvent(VulkanWindow* self, const SDL_Event* event)
{
	self->mouse.WheelOffset += event->wheel.y;
}

void GameEngine_GLFW_MouseMoveEvent(VulkanWindow* self, double XPosition, double YPosition)
{
	self->mouse.X = XPosition;
	self->mouse.Y = YPosition;
}

void GameEngine_GLFW_MouseButtonPressedEvent(VulkanWindow* self, int action, int button)
{
	if (action == GLFW_PRESS)
	{
		if (button < 3)
		{
			self->mouse.MouseButtonState[button] = MS_PRESSED;
		}
	}
	else if (action == GLFW_RELEASE)
	{
		if (button < 3)
		{
			self->mouse.MouseButtonState[button] = MS_UNPRESSED;
		}
	}
}

void GameEngine_GLFW_MouseWheelEvent(VulkanWindow* self, double Yoffset)
{
	self->mouse.WheelOffset += (int)Yoffset;
}
