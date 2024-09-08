#include "GLFWWindow.h"
static void error_callback(int error, const char* description) 
{
	fprintf(stderr, "GLFW Error %d: %s\n", error, description);
}


GLFWWindow* Window_GetGLFWWindowPointer(VulkanWindow* self)
{
	GLFWWindow* window = (GLFWWindow*)self;
	window->base = self;
	return window;
}

VulkanWindow* Window_GLFW_CreateGraphicsWindow(VulkanWindow* self, const char* WindowName, uint32_t width, uint32_t height)
{
	self->FrameBufferResized = false;
	self->Width = width;
	self->Height = height;
	self->ShouldClose = false;

	glfwInit();
	glfwWindowHint(GLFW_CLIENT_API, GLFW_NO_API);
	glfwWindowHint(GLFW_RESIZABLE, GLFW_TRUE);

	self->WindowHandle = (void*)glfwCreateWindow(width, height, WindowName, NULL, NULL);

	glfwMakeContextCurrent((GLFWWindow*)self->WindowHandle);
	glfwSetWindowUserPointer((GLFWWindow*)self->WindowHandle, self);
	glfwSetErrorCallback(error_callback);
	glfwSetFramebufferSizeCallback((GLFWWindow*)self->WindowHandle, Window_GLFW_FrameBufferResizeCallBack);
	return self;
}

void Window_GLFW_PollEventHandler(VulkanWindow* self)
{
	glfwPollEvents();
}

void Window_GLFW_FrameBufferResizeCallBack(VulkanWindow* self, int width, int height)
{
	GLFWWindow* glfwWindow = (GLFWWindow*)self;
	VulkanWindow* app = (VulkanWindow*)glfwGetWindowUserPointer(glfwWindow);
	if (app)
	{
		app->FrameBufferResized = true;
		glfwGetFramebufferSize((GLFWWindow*)self->WindowHandle, &width, &height);

		while (width == 0 || height == 0)
		{
			glfwGetFramebufferSize((GLFWWindow*)self->WindowHandle, &width, &height);
			glfwWaitEvents();
		}

		self->Width = width;
		self->Height = height;
	}
}

void Window_GLFW_SwapBuffer(VulkanWindow* self)
{
}

const char** Window_GLFW_GetInstanceExtensions(struct VulkanWindow* self, uint32_t* outExtensionCount, bool enableValidationLayers)
{
	uint32_t glfwExtensionCount = 0;
	const char** glfwExtensions = glfwGetRequiredInstanceExtensions(&glfwExtensionCount);

	size_t totalCount = glfwExtensionCount + (enableValidationLayers ? 1 : 0);

	const char** extensions = malloc(totalCount * sizeof(const char*));
	if (!extensions) {
		fprintf(stderr, "Failed to allocate memory for extensions\n");
		return NULL;
	}

	for (uint32_t i = 0; i < glfwExtensionCount; i++) {
		extensions[i] = glfwExtensions[i];
	}

	if (enableValidationLayers)
	{
		extensions[glfwExtensionCount] = VK_EXT_DEBUG_UTILS_EXTENSION_NAME;
	}

	*outExtensionCount = totalCount;
	return extensions;
}

void Window_GLFW_CreateSurface(VulkanWindow* self, VkInstance* instance, VkSurfaceKHR* surface)
{
	glfwCreateWindowSurface(*instance, (GLFWWindow*)self->WindowHandle, NULL, surface);
}

void Window_GLFW_GetFrameBufferSize(VulkanWindow* self, int* width, int* height)
{
	glfwGetFramebufferSize((GLFWWindow*)self->WindowHandle, &*width, &*height);
}

void Window_GLFW_DestroyWindow(VulkanWindow* self)
{
	glfwDestroyWindow((GLFWWindow*)self->WindowHandle);
	glfwTerminate();
}

bool Window_GLFW_WindowShouldClose(VulkanWindow* self)
{
	return  glfwWindowShouldClose((GLFWWindow*)self->WindowHandle);
}
