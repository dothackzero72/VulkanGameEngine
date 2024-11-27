#include "pch.h"
#include "VulkanGameEngineGameObjectScriptsCLI.h"
#include <iostream>

using namespace System;
using namespace System::Runtime::InteropServices;

TestScriptComponentDLL::TestScriptComponentDLL() : GameObjectComponentDLL()
{
    Console::WriteLine("Drawing with buffer update");
    component = gcnew TestScriptComponent(ComponentTypeEnum::kTestScriptConponent);
}

TestScriptComponentDLL::TestScriptComponentDLL(String^ name)
{
    Console::WriteLine("Drawing with buffer update");
    component = gcnew TestScriptComponent(name, ComponentTypeEnum::kTestScriptConponent);
}

void TestScriptComponentDLL::Update(long startTime)
{
    Console::WriteLine("Drawing with buffer update");
    std::cout << "Test Object 1" << std::endl;
    component->Update(startTime);
}

void TestScriptComponentDLL::BufferUpdate(VkCommandBuffer commandBuffer, long startTime)
{
    Console::WriteLine("Drawing with buffer update");
 //   component->Update(static_cast<void*>(commandBuffer), startTime);
}

void TestScriptComponentDLL::Destroy()
{
    Console::WriteLine("Destroying component: {0}", Name);
}

int TestScriptComponentDLL::GetMemorySize()
{
    return static_cast<int>(MemorySize);
}

extern "C" 
{
    DLL_EXPORT_MANAGED void* DLL_CreateTestScriptComponent(void* wrapperHandle)
    {
        if (wrapperHandle != nullptr)
        {
            try
            {
                TestScriptComponentDLL^ component = gcnew TestScriptComponentDLL();
                GCHandle handle = GCHandle::Alloc(component);
                return (void*)GCHandle::ToIntPtr(handle).ToPointer();
            }
            catch (Exception^ e)
            {
                std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
                return nullptr;
            }
        }
    }

    DLL_EXPORT_MANAGED void DLL_TestScriptComponent_Update(void* wrapperHandle, long startTime)
    {
        try
        {
            if (wrapperHandle != nullptr)
            {
                IntPtr handlePtr(wrapperHandle);
                GCHandle handle = GCHandle::FromIntPtr(handlePtr);
                GameObjectComponentDLL^ managedWrapper = static_cast<GameObjectComponentDLL^>(handle.Target);
                if (managedWrapper != nullptr)
                {
                    managedWrapper->Update(startTime);
                }
            }
        }
        catch (Exception^ e)
        {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
        }
    }

    DLL_EXPORT_MANAGED void DLL_TestScriptComponent_BufferUpdate(void* wrapperHandle, VkCommandBuffer commandBuffer, long startTime)
    {
        try
        {
            if (wrapperHandle != nullptr)
            {
                IntPtr handlePtr(wrapperHandle);
                GCHandle handle = GCHandle::FromIntPtr(handlePtr);
                GameObjectComponentDLL^ managedWrapper = static_cast<GameObjectComponentDLL^>(handle.Target);
                if (managedWrapper != nullptr)
                {
                    managedWrapper->BufferUpdate(commandBuffer, startTime);
                }
            }
        }
        catch (Exception^ e)
        {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
        }
    }

    DLL_EXPORT_MANAGED void DLL_TestScriptComponent_Destroy(void* wrapperHandle)
    {
        try
        {
            if (wrapperHandle != nullptr)
            {
                IntPtr handlePtr(wrapperHandle);
                GCHandle handle = GCHandle::FromIntPtr(handlePtr);
                GameObjectComponentDLL^ managedWrapper = static_cast<GameObjectComponentDLL^>(handle.Target);
                if (managedWrapper != nullptr) 
                {
                    managedWrapper->Destroy();
                }
            }
        }
        catch (Exception^ e)
        {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
        }
    }

    DLL_EXPORT_MANAGED int DLL_TestScriptComponent_GetMemorySize(void* wrapperHandle)
    {
        try
        {
            if (wrapperHandle != nullptr)
            {
                IntPtr handlePtr(wrapperHandle);
                GCHandle handle = GCHandle::FromIntPtr(handlePtr);
                GameObjectComponentDLL^ managedWrapper = static_cast<GameObjectComponentDLL^>(handle.Target);
                if (managedWrapper != nullptr) 
                {
                    return managedWrapper->GetMemorySize();
                }
            }
            return -1;
        }
        catch (Exception^ e)
        {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
            return -1;
        }
    }
}