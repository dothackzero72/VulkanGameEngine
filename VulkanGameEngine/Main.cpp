
extern "C"
{
#include <VulkanWindow.h>
//#include <VulkanRenderer.h>
}
#include <stdio.h>


int main(int argc, char* argv[]) // Change main signature if needed
{
    vulkanWindow = Window_CreateWindow(Window_Type::GLFW, "Game", 1280, 720);
    if (!vulkanWindow) {
        fprintf(stderr, "Failed to create window.\n");
        return -1;
    }
  //  Renderer_RendererSetUp();
    while (!vulkanWindow->WindowShouldClose(vulkanWindow))
    {
        vulkanWindow->PollEventHandler(vulkanWindow);
        vulkanWindow->SwapBuffer(vulkanWindow);
    }
    vulkanWindow->DestroyWindow(vulkanWindow); 
    return 0;
}