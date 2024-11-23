using RGiesecke.DllExport;
using Silk.NET.Vulkan;
using System;
using System.Runtime.InteropServices;

namespace VulkanGameEngineGameObjectScripts
{
    public class TestScriptComponentDLL
    {
        public int memorySize { get; set; } = 0;

        public TestScriptComponentDLL(int memorySize)
        {
            this.memorySize = memorySize;
        }

        [DllExport("DLL_TestScriptComponent_Update", CallingConvention = CallingConvention.Cdecl)]
        public void Update(long startTime)
        {
            memorySize++;
        }

        [DllExport("DLL_TestScriptComponent_Update2", CallingConvention = CallingConvention.Cdecl)]
        public void Update(CommandBuffer commandBuffer, long startTime)
        {
            memorySize++;
        }

        [DllExport("DLL_TestScriptComponent_Destroy", CallingConvention = CallingConvention.Cdecl)]
        public void Destroy()
        {
            Console.WriteLine("Destroying component.");
        }

        [DllExport("DLL_TestScriptComponent_CreateTestScriptComponent", CallingConvention = CallingConvention.Cdecl)]
        public static TestScriptComponentDLL CreateTestScriptComponent(int memorySize)
        {
            return new TestScriptComponentDLL(memorySize);
        }
    }
}