#include "pch.h"
#include "VulkanGameEngineGameObjectScriptsCLI.h"
#include <msclr/marshal_cppstd.h>
#include <iostream>

using namespace System;
using namespace System::Runtime::InteropServices;


TestScriptComponentDLL::TestScriptComponentDLL()
{
    std::cout << "do you hear me TestScriptComponentDLL" << std::endl;
    component = gcnew TestScriptComponent_CS();
}

TestScriptComponentDLL::TestScriptComponentDLL(void* gameObjectPtr)
{
    std::cout << "do you hear me TestScriptComponentDLL2" << std::endl;
    component = gcnew TestScriptComponent_CS((IntPtr)gameObjectPtr);
}

TestScriptComponentDLL::TestScriptComponentDLL(void* gameObjectPtr, String^ name)
{
    std::cout << "do you hear me TestScriptComponentDLL3" << std::endl;
    component = gcnew TestScriptComponent_CS((IntPtr)gameObjectPtr, name);
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
    DLL_EXPORT_MANAGED void* DLL_CreateTestScriptComponent() {
        try 
        {
            std::cout << "do you hear me DLL_CreateTestScriptComponent" << std::endl;
            TestScriptComponentDLL^ wrapper = gcnew TestScriptComponentDLL();
            GCHandle handle = GCHandle::Alloc(wrapper);
            return (void*)GCHandle::ToIntPtr(handle).ToPointer();
        }
        catch (Exception^ e) {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
            return nullptr;
        }
    }

    DLL_EXPORT_MANAGED void* DLL_CreateTestScriptComponentName(void* wrapperHandle, void* gameObjectPtr, std::string name)
    {
        if (wrapperHandle != nullptr)
        {
            try
            {
                TestScriptComponentDLL^ component = gcnew TestScriptComponentDLL(gameObjectPtr, msclr::interop::marshal_as<String^>(name));
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