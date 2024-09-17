
extern "C"
{
#include <VulkanWindow.h>
#include <VulkanRenderer.h>
#include <GLFWWindow.h>
}
#include <stdio.h>
#include "InterfaceRenderPass.h"
#include "Scene.h"
#include <nlohmann/json.hpp>
#include <ImPlot/implot.h>
#include "SystemClock.h"
#include "FrameTime.h"
#include <iostream>

int main()
{
    SystemClock systemClock = SystemClock();
    FrameTimer deltaTime = FrameTimer();
    vulkanWindow = Window_CreateWindow(Window_Type::GLFW, "Game", 1280, 720);
    Renderer_RendererSetUp();
    InterfaceRenderPass::StartUp();
    ImPlot::CreateContext();

    Scene scene;
    scene.StartUp();
    while (!vulkanWindow->WindowShouldClose(vulkanWindow))
    {
        vulkanWindow->PollEventHandler(vulkanWindow);
        vulkanWindow->SwapBuffer(vulkanWindow);
        scene.Update(deltaTime.GetFrameTime());
        scene.ImGuiUpdate(deltaTime.GetFrameTime());
        scene.Draw();
        deltaTime.EndFrameTime();
    }
    vkDeviceWaitIdle(renderer.Device);
    ImPlot::DestroyContext();
    InterfaceRenderPass::Destroy();
    scene.Destroy();
    Renderer_DestroyRenderer();
    vulkanWindow->DestroyWindow(vulkanWindow); 
    return 0;
}