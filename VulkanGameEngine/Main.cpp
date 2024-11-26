#include <windows.h>
#include <iostream>
#include <vector>
#include <string>

extern "C" {
    typedef void* (*CreateSimpleTestWrapper)();
    typedef void (*DestroySimpleTestWrapper)(void* wrapperHandle);
    typedef int (*CallSimpleTestWrapperFunctionType)(void* wrapperHandle, int a);
}

class SimpleTestWrapperContainer {
private:
    HMODULE hModule;
    void* wrapper;

    CreateSimpleTestWrapper CreateSimpleTestWrapperPtr;
    DestroySimpleTestWrapper DestroySimpleTestWrapperPtr;
    CallSimpleTestWrapperFunctionType CallSimpleTestWrapperFunctionTypePtr;

public:
    SimpleTestWrapperContainer(const std::wstring& dllPath) 
    {
        hModule = LoadLibrary(dllPath.c_str());
        if (!hModule) {
            throw std::runtime_error("Could not load the DLL. Error code: " + std::to_string(GetLastError()));
        }

        CreateSimpleTestWrapperPtr = (CreateSimpleTestWrapper)GetProcAddress(hModule, "CreateSimpleTestWrapper");
        if (!CreateSimpleTestWrapperPtr)
        {
            throw std::runtime_error("Could not find CreateSimpleTestWrapper function.");
        }

        DestroySimpleTestWrapperPtr = (DestroySimpleTestWrapper)GetProcAddress(hModule, "DestroySimpleTestWrapper");
        if (!DestroySimpleTestWrapperPtr) 
        {
            throw std::runtime_error("Could not find DestroySimpleTestWrapper function.");
        }

        CallSimpleTestWrapperFunctionTypePtr = (CallSimpleTestWrapperFunctionType)GetProcAddress(hModule, "CallSimpleTestWrapperFunction");
        if (!CallSimpleTestWrapperFunctionTypePtr) {
            throw std::runtime_error("Could not find CallSimpleTestWrapperFunction function.");
        }
    }

    ~SimpleTestWrapperContainer()
    {
        DestroySimpleTestWrapperPtr(wrapper);
        FreeLibrary(hModule);
    }

    void* createWrapper() 
    {
        void* wrapperHandle = CreateSimpleTestWrapperPtr();
        if (!wrapperHandle) {
            throw std::runtime_error("Component creation failed.");
        }

        wrapper = wrapperHandle;
        return wrapperHandle;
    }

    int callSimpleFunction(void* wrapperHandle, int a) 
    {
        return CallSimpleTestWrapperFunctionTypePtr(wrapperHandle, a);
    }
};


int main() 
{
    try 
    {
        SimpleTestWrapperContainer container(L"C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanGameEngineGameObjectScriptsCLI.dll");

        void* wrapperHandle = container.createWrapper();

        int result = container.callSimpleFunction(wrapperHandle, 5); 
         result = container.callSimpleFunction(wrapperHandle, 5);
         result = container.callSimpleFunction(wrapperHandle, 5);
         result = container.callSimpleFunction(wrapperHandle, 5);
         result = container.callSimpleFunction(wrapperHandle, 5);
         result = container.callSimpleFunction(wrapperHandle, 5);
        std::cout << "SimpleFunction result: " << result << std::endl;
    }
    catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << std::endl;
        return 1;
    }

    return 0;
}