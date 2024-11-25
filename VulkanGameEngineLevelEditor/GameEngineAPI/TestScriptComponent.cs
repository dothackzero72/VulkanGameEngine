using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class TestScriptComponent
    {
        // Exporting methods using DllExport
        [DllExport("Update", CallingConvention = CallingConvention.StdCall)]
        public static void Update(long param)
        {
            // Implementation
        }

        [DllExport("Update2", CallingConvention = CallingConvention.StdCall)]
        public static void Update2(IntPtr commandBuffer, long param)
        {
            // Implementation
        }

        [DllExport("Destroy", CallingConvention = CallingConvention.StdCall)]
        public static void Destroy()
        {
            // Cleanup code
        }

        [DllExport("CreateTestScriptComponent", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr CreateTestScriptComponent(int memorySize)
        {
            // Create and return a pointer
            return IntPtr.Zero;
        }
    }
}
