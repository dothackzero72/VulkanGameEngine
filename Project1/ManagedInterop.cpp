#using <System.dll>
using namespace System;
using namespace System::Runtime::InteropServices;

namespace MyNamespace {
    public ref class MyManagedClass {
    public:
        void SayHello()
        {
            Console::WriteLine("Hello from C++/CLI");
        }
    };

    // Exported function for native C++ to call
    extern "C" __declspec(dllexport) void CallSayHello()
    {
        MyManagedClass^ instance = gcnew MyManagedClass();
        instance->SayHello();
    }
}