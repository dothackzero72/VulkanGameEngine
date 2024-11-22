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
#include "MemoryManager.h"

ref class Z {};

int main()
{
    Z^ p = gcnew Z;
    //SystemClock systemClock = SystemClock();
    //FrameTimer deltaTime = FrameTimer();
    //vulkanWindow = Window_CreateWindow(Window_Type::GLFW, "Game", 1280, 720);
    //renderer.RendererSetUp();
    //MemoryManager::SetUpMemoryManager(30);
    //InterfaceRenderPass::StartUp();
    //ImPlot::CreateContext();

    //Scene scene;
    //scene.StartUp();
    //while (!vulkanWindow->WindowShouldClose(vulkanWindow))
    //{
    //    vulkanWindow->PollEventHandler(vulkanWindow);
    //    vulkanWindow->SwapBuffer(vulkanWindow);
    //    scene.Update(deltaTime.GetFrameTime());
    //    scene.ImGuiUpdate(deltaTime.GetFrameTime());
    //    scene.Draw();
    //    deltaTime.EndFrameTime();
    //}

    //vkDeviceWaitIdle(cRenderer.Device);
    //scene.Destroy();
    //ImPlot::DestroyContext();
    //InterfaceRenderPass::Destroy();
    //renderer.DestroyRenderer();
    //vulkanWindow->DestroyWindow(vulkanWindow); 
    return 0;
}

