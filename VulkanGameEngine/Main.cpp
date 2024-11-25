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
#include <windows.h>
#include <iostream>

typedef void (*CallSayHelloFunc)();
typedef int (*AddFunc)(int, int);
typedef int (*MultiplyFunc)(int, int);

int main() {
    // Load the DLL
    HMODULE hModule = LoadLibrary(L"C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanGameEngineGameObjectScriptsCLI.dll");
    if (!hModule) {
        std::cerr << "Could not load the DLL." << std::endl;
        return 1;
    }

    // Retrieve the function addresses
    CallSayHelloFunc CallSayHello = (CallSayHelloFunc)GetProcAddress(hModule, "CallSayHello");
    if (!CallSayHello) {
        std::cerr << "Could not locate CallSayHello function." << std::endl;
        FreeLibrary(hModule);
        return 1;
    }

    AddFunc add = (AddFunc)GetProcAddress(hModule, "Add");
    if (!add) {
        std::cerr << "Could not locate Add function." << std::endl;
        FreeLibrary(hModule);
        return 1;
    }

    MultiplyFunc multiply = (MultiplyFunc)GetProcAddress(hModule, "Multiply");
    if (!multiply) {
        std::cerr << "Could not locate Multiply function." << std::endl;
        FreeLibrary(hModule);
        return 1;
    }

    // Call the managed method
    CallSayHello();

    // Provide input values for add and multiply
    int a = 5, b = 10;
    int sum = add(a, b);           // Pass parameters to Add
    int product = multiply(a, b);  // Pass parameters to Multiply

    // Output the results
    std::cout << "Sum: " << sum << std::endl;
    std::cout << "Product: " << product << std::endl;

    SystemClock systemClock = SystemClock();
    FrameTimer deltaTime = FrameTimer();
    vulkanWindow = Window_CreateWindow(Window_Type::GLFW, "Game", 1280, 720);
    renderer.RendererSetUp();
    MemoryManager::SetUpMemoryManager(30);
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

    vkDeviceWaitIdle(cRenderer.Device);
    scene.Destroy();
    ImPlot::DestroyContext();
    InterfaceRenderPass::Destroy();
    renderer.DestroyRenderer();
    vulkanWindow->DestroyWindow(vulkanWindow);
    // Clean up
    FreeLibrary(hModule);
    return 0;
}