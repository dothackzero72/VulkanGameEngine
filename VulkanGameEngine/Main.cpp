extern "C"
{
#include <VulkanWindow.h>
#include <GLFWWindow.h>
}
#include "VulkanRenderer.h"
#include <stdio.h>
#include "InterfaceRenderPass.h"

#include <nlohmann/json.hpp>
#include <ImPlot/implot.h>
#include "SystemClock.h"
#include <iostream>
#include "FrameTimer.h"
#include "GameSystem.h"

int main(int argc, char** argv)
{
    SystemClock systemClock = SystemClock();
    FrameTimer deltaTime = FrameTimer();
    vulkanWindow = Window_CreateWindow(Window_Type::GLFW, "Game", 1920, 1080);
    ImPlot::CreateContext();

    gameSystem.StartUp();
    while (!vulkanWindow->WindowShouldClose(vulkanWindow))
    {
        const float frameTime = deltaTime.GetFrameTime();

        vulkanWindow->PollEventHandler(vulkanWindow);
        vulkanWindow->SwapBuffer(vulkanWindow);
        gameSystem.Input(frameTime);
        gameSystem.Update(frameTime);
        gameSystem.DebugUpdate(frameTime);
        gameSystem.Draw(frameTime);
        deltaTime.EndFrameTime();
    }

    vkDeviceWaitIdle(cRenderer.Device);
    ImPlot::DestroyContext();
    gameSystem.Destroy();
    vulkanWindow->DestroyWindow(vulkanWindow);
    return 0;
}