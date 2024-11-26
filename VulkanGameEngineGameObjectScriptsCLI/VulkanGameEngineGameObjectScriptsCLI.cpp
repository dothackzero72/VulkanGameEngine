#include "pch.h"
#include "VulkanGameEngineGameObjectScriptsCLI.h"
#include <iostream>

using namespace System;
using namespace System::Runtime::InteropServices;

extern "C" {
    // Create a simple test wrapper
    __declspec(dllexport) void* CreateSimpleTestWrapper() {
        try {
            SimpleTestWrapper^ wrapper = gcnew SimpleTestWrapper();
            GCHandle handle = GCHandle::Alloc(wrapper);
            return (void*)GCHandle::ToIntPtr(handle).ToPointer();
        }
        catch (Exception^ e) {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
            return nullptr;
        }
    }

    __declspec(dllexport) void DestroySimpleTestWrapper(void* wrapperHandle) {
        if (wrapperHandle != nullptr) {
            IntPtr handlePtr(wrapperHandle);
            GCHandle handle = GCHandle::FromIntPtr(handlePtr);
            SimpleTestWrapper^ managedWrapper = static_cast<SimpleTestWrapper^>(handle.Target);
            if (managedWrapper != nullptr) {
                delete managedWrapper;
            }
            handle.Free();
        }
    }

    // New exported function to call CallSimpleFunction
    __declspec(dllexport) int CallSimpleTestWrapperFunction(void* wrapperHandle, int a) {
        if (wrapperHandle != nullptr) {
            IntPtr handlePtr(wrapperHandle);
            GCHandle handle = GCHandle::FromIntPtr(handlePtr);
            SimpleTestWrapper^ managedWrapper = static_cast<SimpleTestWrapper^>(handle.Target);
            if (managedWrapper != nullptr) {
                return managedWrapper->CallSimpleFunction(a);
            }
        }
        return -1; // Error case
    }
}