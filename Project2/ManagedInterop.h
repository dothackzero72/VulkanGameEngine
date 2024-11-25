//#using <System.dll>
//using namespace System;
//using namespace System::Runtime::InteropServices;
//
//namespace MyNamespace {
//    public ref class MyManagedClass {
//    public:
//        void SayHello() 
//        {
//            Console::WriteLine("Hello from C++/CLI");
//        }
//    };
//
//    // Native function wrapper
//    extern "C" __declspec(dllexport) void CallSayHello() 
//    {
//        MyManagedClass^ instance = gcnew MyManagedClass();
//        instance->SayHello();
//    }
//}

#pragma once

using namespace System;
using namespace MyExportedFunctions;

namespace VulkanGameEngineGameObjectScripts
{
    public ref class MathWrapper
    {
    public:
        static int Add(int a, int b)
        {
            ExportedMethods^ math = gcnew ExportedMethods();
            return math->Add(a, b);
        }

        static int Multiply(int a, int b)
        {
            ExportedMethods^ math = gcnew ExportedMethods();
            return math->Multiply(a, b);
        }
    };
    // ExportedFunctions.h
    //extern "C" __declspec(dllexport) int Add(int a, int b)
    //{
    //    return 1;
    //}
    //extern "C" __declspec(dllexport) int Multiply(int a, int b)
    //{
    //    return 2;
    //}
}