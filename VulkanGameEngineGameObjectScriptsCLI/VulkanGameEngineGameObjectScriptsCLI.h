#using <System.dll>
#using <../VulkanGameEngineGameObjectScripts/obj/Debug/net8.0/win-x64/ref/VulkanGameEngineGameObjectScripts.dll>

extern "C"
{
    extern "C" __declspec(dllexport) void CallSayHello();
    extern "C" __declspec(dllexport) int Add(int a, int b);
    extern "C" __declspec(dllexport) int Multiply(int a, int b);
}