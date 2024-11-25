#include <windows.h>
#include <iostream>
#include "../ClassLibrary1/ClassLibrary1.h"

int main() 
{
    CallSayHello();  // Call the C# method
    std::cout << "Add Result: " << Add(5, 10) << std::endl; // Outputs: 15
    std::cout << "Multiply Result: " << Multiply(5, 10) << std::endl; // Outputs: 50

    return 0;
}