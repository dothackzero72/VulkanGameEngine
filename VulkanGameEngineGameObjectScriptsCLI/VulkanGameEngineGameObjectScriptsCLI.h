#pragma once
#include "ISimpleTestWrapper.h"
#using <../VulkanGameEngineGameObjectScripts/obj/Debug/net8.0/win-x64/ref/VulkanGameEngineGameObjectScripts.dll>
#include <vcclr.h>
#include <msclr/marshal_cppstd.h>

using namespace System;
using namespace VulkanGameEngineGameObjectScripts;

// Define the wrapper class
public ref class SimpleTestWrapper : public ISimpleTestWrapper
{
private:
    SimpleTest^ simpleTest;

public:
    // Constructor
    SimpleTestWrapper() {
        simpleTest = gcnew SimpleTest();
    }

    // Managed function that wraps the managed SimpleTest's SimpleFunction
    virtual int SimpleFunction(int a) {
        return simpleTest->SimpleFunction(a);
    }

    virtual void DestroyFunction() {
        simpleTest->SimpleDestroy();
    }

    virtual int GetCounter() {
        return simpleTest->counter;
    }

    // Destructor to handle cleanup properly
    ~SimpleTestWrapper() {
        this->DestroyFunction();
    }

    // New method to be called natively
    int CallSimpleFunction(int a) {
        return this->SimpleFunction(a);
    }
};