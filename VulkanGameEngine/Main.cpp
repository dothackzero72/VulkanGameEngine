
extern "C"
{
#include <VulkanWindow.h>
#include <VulkanRenderer.h>
#include <GLFWWindow.h>
}
#include <stdio.h>
#include "InterfaceRenderPass.h"
#include "Scene.h"

int main(int argc, char* argv[])
{ 
    vulkanWindow = Window_CreateWindow(Window_Type::GLFW, "Game", 1280, 720);
    Renderer_RendererSetUp();
    InterfaceRenderPass::StartUp();

    Scene scene;
    scene.StartUp();
    while (!vulkanWindow->WindowShouldClose(vulkanWindow))
    {
        vulkanWindow->PollEventHandler(vulkanWindow);
        vulkanWindow->SwapBuffer(vulkanWindow);
        scene.Update();
        scene.ImGuiUpdate();
        scene.Draw();
    }
    vkDeviceWaitIdle(renderer.Device);
    InterfaceRenderPass::Destroy();
    scene.Destroy();
    Renderer_DestroyRenderer();
    vulkanWindow->DestroyWindow(vulkanWindow); 
    return 0;
}