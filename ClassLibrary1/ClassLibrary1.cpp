
#include "pch.h"
#using <System.dll>
#using <../VulkanGameEngineGameObjectScripts/obj/Debug/net8.0/win-x64/ref/VulkanGameEngineGameObjectScripts.dll>

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace MyExportedFunctions;

namespace MyNamespace {
    public ref class MyManagedClass {
    public:
        void SayHello()
        {
            Console::WriteLine("Hello from C++/CLI");
        }
    };

    // Exported function for native C++ to call


    public ref class MathWrapper {
    public:
        ExportedMethods^ exported;
        // Constructor
        MathWrapper()
        {
            exported = gcnew ExportedMethods();
        }

        // Add two numbers
        int add(int a, int b) {
            return exported->Add(a, b); // Call the C# Add function
        }

        // Multiply two numbers
        int multiply(int a, int b) {
            return exported->Multiply(a, b); // Call the C# Multiply function
        }
    };

    extern "C" __declspec(dllexport) void CallSayHello()
    {
        MyManagedClass^ instance = gcnew MyManagedClass();
        instance->SayHello();
    }

    extern "C" __declspec(dllexport) int Add()
    {
        ExportedMethods^ instance = gcnew ExportedMethods();
        return instance->Add(1, 2);
    }

    extern "C" __declspec(dllexport) int Multiply()
    {
        ExportedMethods^ instance = gcnew ExportedMethods();
        return instance->Multiply(4,4);
    }
}

