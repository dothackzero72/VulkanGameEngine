#include "GLFWWindow.h"

static GLFWWindow* Window_GetGLFWWindowPointer(VulkanWindow* self)
{
	GLFWWindow* glfwWindow = (GLFWWindow*)malloc(sizeof(GLFWWindow));
	glfwWindow->glfwWindowHandle = (GLFWWindow*)self->WindowHandle;
	glfwWindow->base = self;
	return glfwWindow;
}

VulkanWindow* Window_GLFW_CreateGraphicsWindow(VulkanWindow* self, const char* WindowName, uint32_t width, uint32_t height)
{
	GLFWWindow* glfwWindow = (GLFWWindow*)malloc(sizeof(GLFWWindow));
	glfwWindow->base = self;
	glfwWindow->base->FrameBufferResized = false;
	glfwWindow->base->Width = width;
	glfwWindow->base->Height = height;
	glfwWindow->base->ShouldClose = false;

	glfwInit();
	glfwWindowHint(GLFW_CLIENT_API, GLFW_NO_API);
	glfwWindow->glfwWindowHandle = glfwCreateWindow(width, height, WindowName, NULL, NULL);
	glfwSetWindowUserPointer(glfwWindow->glfwWindowHandle, NULL);
	glfwSetFramebufferSizeCallback(glfwWindow->glfwWindowHandle, Window_GLFW_FrameBufferResizeCallBack);

	glfwWindow->base->WindowHandle = (void*)glfwWindow->glfwWindowHandle;
	return glfwWindow;
}

void Window_GLFW_PollEventHandler(VulkanWindow* self)
{
	GLFWWindow* glfwWindow = Window_GetGLFWWindowPointer(self);
	glfwPollEvents();
}

void Window_GLFW_FrameBufferResizeCallBack(VulkanWindow* self, int width, int height)
{
	GLFWWindow* glfwWindow = Window_GetGLFWWindowPointer(self);
	VulkanWindow* app = (VulkanWindow*)glfwGetWindowUserPointer(glfwWindow);
	if (app)
	{
		app->FrameBufferResized = true;
		glfwGetFramebufferSize(self, &width, &height);

		while (width == 0 || height == 0)
		{
			glfwGetFramebufferSize(self, &width, &height);
			glfwWaitEvents();
		}

		glfwWindow->base->Width = width;
		glfwWindow->base->Height = height;
	}
}

void Window_GLFW_SwapBuffer(VulkanWindow* self)
{
}

void Window_GLFW_GetInstanceExtensions(VulkanWindow* self, uint32_t* pExtensionCount, VkExtensionProperties** extensionProperties)
{
	GLFWWindow* glfwWindow = Window_GetGLFWWindowPointer(self);
	const char** extensions = glfwGetRequiredInstanceExtensions(pExtensionCount);
	if (extensions)
	{
		for (uint32_t x = 0; x < *pExtensionCount; x++)
		{
			((const char**)extensionProperties)[x] = extensions[x];
		}
	}
	else
	{
		*pExtensionCount = 0;
		extensionProperties = NULL;
	}
}

void Window_GLFW_CreateSurface(VulkanWindow* self, VkInstance* instance, VkSurfaceKHR* surface)
{
	GLFWWindow* glfwWindow = Window_GetGLFWWindowPointer(self);
	glfwCreateWindowSurface(instance, glfwWindow, NULL, surface);
}

void Window_GLFW_GetFrameBufferSize(VulkanWindow* self, int* width, int* height)
{
	GLFWWindow* glfwWindow = Window_GetGLFWWindowPointer(self);
	glfwGetFramebufferSize(glfwWindow->glfwWindowHandle, &*width, &*height);
}

void Window_GLFW_DestroyWindow(VulkanWindow* self)
{
	GLFWWindow* glfwWindow = Window_GetGLFWWindowPointer(self);

	glfwDestroyWindow(glfwWindow->glfwWindowHandle);
	glfwTerminate();
}

bool Window_GLFW_WindowShouldClose(VulkanWindow* self)
{
	GLFWWindow* glfwWindow = Window_GetGLFWWindowPointer(self);
	glfwWindow->base->ShouldClose = glfwWindowShouldClose(glfwWindow->glfwWindowHandle);
	return glfwWindow->base->ShouldClose;
}
