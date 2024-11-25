#include <windows.h>
#include <iostream>

typedef void (*CallSayHelloFunc)();
typedef int (*AddFunc)(int, int);
typedef int (*MultiplyFunc)(int, int);

int main() {
    // Load the DLL
    HMODULE hModule = LoadLibrary(L"ClassLibrary1.dll");
    if (!hModule) {
        std::cerr << "Could not load the DLL." << std::endl;
        return 1;
    }

    // Retrieve the function addresses
    CallSayHelloFunc CallSayHello = (CallSayHelloFunc)GetProcAddress(hModule, "CallSayHello");
    if (!CallSayHello) {
        std::cerr << "Could not locate CallSayHello function." << std::endl;
        FreeLibrary(hModule);
        return 1;
    }

    AddFunc add = (AddFunc)GetProcAddress(hModule, "Add");
    if (!add) {
        std::cerr << "Could not locate Add function." << std::endl;
        FreeLibrary(hModule);
        return 1;
    }

    MultiplyFunc multiply = (MultiplyFunc)GetProcAddress(hModule, "Multiply");
    if (!multiply) {
        std::cerr << "Could not locate Multiply function." << std::endl;
        FreeLibrary(hModule);
        return 1;
    }

    // Call the managed method
    CallSayHello();

    // Provide input values for add and multiply
    int a = 5, b = 10;
    int sum = add(a, b);           // Pass parameters to Add
    int product = multiply(a, b);  // Pass parameters to Multiply

    // Output the results
    std::cout << "Sum: " << sum << std::endl;
    std::cout << "Product: " << product << std::endl;

    // Clean up
    FreeLibrary(hModule);
    return 0;
}