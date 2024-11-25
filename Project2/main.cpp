#include "ManagedInterop.h"

using namespace System;
using namespace VulkanGameEngineGameObjectScripts;
int main(array<System::String^>^ args)
{
    MathWrapper^ math = gcnew MathWrapper();
    int adsf = math->Add(2, 5);

    Console::WriteLine("Welcome to the C++/CLI Application!");

    if (args->Length == 0)
    {
        Console::WriteLine("No arguments provided. Please provide at least one argument.");
        return 1; // Error code
    }

    // Loop through all provided arguments and print them
    Console::WriteLine("You provided the following arguments:");
    for (int i = 0; i < args->Length; ++i)
    {
        Console::WriteLine("Argument {0}: {1}", i + 1, args[i]);
    }

    // For demonstration purposes, execute a simple command based on the first argument
    String^ command = args[0];
    if (command == "hello")
    {
        Console::WriteLine("Hello, world!");
    }
    else
    {
        Console::WriteLine("Unknown command: {0}", command);
    }

    return 0; // Success
}