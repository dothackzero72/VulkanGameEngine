extern "C"
{
#include <VulkanWindow.h>
#include <GLFWWindow.h>
}
#include "VulkanRenderer.h"
#include <stdio.h>
#include "InterfaceRenderPass.h"
#include "Scene.h"
#include <nlohmann/json.hpp>
#include <ImPlot/implot.h>
#include "SystemClock.h"
#include "FrameTime.h"
#include <iostream>

int main(int argc, char** argv)
{
    SystemClock systemClock = SystemClock();
    FrameTimer deltaTime = FrameTimer();
    vulkanWindow = Window_CreateWindow(Window_Type::GLFW, "Game", 1920, 1080);
    renderer.RendererSetUp();
    InterfaceRenderPass::StartUp();
    ImPlot::CreateContext();

    Scene scene;
    scene.StartUp();
    while (!vulkanWindow->WindowShouldClose(vulkanWindow))
    {
        const float frameTime = deltaTime.GetFrameTime();

        vulkanWindow->PollEventHandler(vulkanWindow);
        vulkanWindow->SwapBuffer(vulkanWindow);
        scene.Input(frameTime);
        scene.Update(frameTime);
        scene.ImGuiUpdate(frameTime);
        scene.Draw(frameTime);
        deltaTime.EndFrameTime();
    }

    vkDeviceWaitIdle(cRenderer.Device);
    scene.Destroy();
    ImPlot::DestroyContext();
    InterfaceRenderPass::Destroy();

    vulkanWindow->DestroyWindow(vulkanWindow);
    return 0;
}