#include <windows.h>
#include <iostream>

typedef void (*CallSayHelloFunc)();
typedef int (*Add)();
typedef int (*Multiply)();

int main() {
    // Load the DLL
    HMODULE hModule = LoadLibrary(L"Project1.dll");
    if (!hModule) {
        std::cerr << "Could not load the DLL." << std::endl;
        return 1;
    }

    // Retrieve the function address
    CallSayHelloFunc CallSayHello = (CallSayHelloFunc)GetProcAddress(hModule, "CallSayHello");
    if (!CallSayHello) {
        std::cerr << "Could not locate the function." << std::endl;
        FreeLibrary(hModule);
        return 1;
    }

    Add add = (Add)GetProcAddress(hModule, "Add");
    if (!add) {
        std::cerr << "Could not locate the function." << std::endl;
        FreeLibrary(hModule);
        return 1;
    }

    Multiply multiply = (Multiply)GetProcAddress(hModule, "Multiply");
    if (!multiply) {
        std::cerr << "Could not locate the function." << std::endl;
        FreeLibrary(hModule);
        return 1;
    }

    // Call the managed method
    CallSayHello();
    auto a = add();
    auto b = multiply();

    // Clean up
    FreeLibrary(hModule);
    return 0;
}