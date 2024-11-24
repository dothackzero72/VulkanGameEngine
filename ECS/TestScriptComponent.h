#pragma once
#include <iostream>
#include <Windows.h>
#include <vcclr.h>
#include "vulkan/vulkan_core.h"

// Ensure the necessary managed namespace is accessible
using namespace System;
using namespace VulkanGameEngineGameObjectScripts;  // Correctly include your C# namespace

namespace VulkanGameEngineComponent
{
    public ref class TestScriptComponent : public GameObjectComponent
    {
    private:
        TestScriptComponentDLL^ testScriptComponentDLL;  // Managed reference to the C# class

    public:
        TestScriptComponent(int memorySize)
        {
            // Creating an instance of the C# class
            testScriptComponentDLL = TestScriptComponentDLL::CreateTestScriptComponent(memorySize);
        }

        void Update(long startTime)
        {
            // Calling the Update method from the C# class
            testScriptComponentDLL->Update(startTime);
        }

        void Update(VkCommandBuffer commandBuffer, long startTime)
        {
            // Calling the overloaded Update method from the C# class
            testScriptComponentDLL->Update(commandBuffer, startTime);
        }

        void Destroy()
        {
            // Call to destroy the component in C# code
            testScriptComponentDLL->Destroy();
        }
    };
}