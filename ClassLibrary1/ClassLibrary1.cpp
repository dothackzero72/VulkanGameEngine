#include "pch.h"
#include "ClassLibrary1.h"

using namespace System;
using namespace MyExportedFunctions;

extern "C" {
    MY_DLL_API void CallSayHello() {
        ExportedMethods::CallSayHello(); // Call the C# method
    }

    MY_DLL_API int Add(int a, int b) {
        return ExportedMethods::Add(a, b); // Call the C# method
    }

    MY_DLL_API int Multiply(int a, int b) {
        return ExportedMethods::Multiply(a, b); // Call the C# method
    }
}