
using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineGameObjectScripts
{
    public static class NativeMethods
    {
        // This will be exported to the unmanaged code
        [RGiesecke.DllExport.DllExport("Add", CallingConvention = CallingConvention.Cdecl)]
        public static int Add(int a, int b)
        {
            return a + b;
        }

        // This will create a Point struct and return its pointer
        [DllExport("CreatePoint", CallingConvention = CallingConvention.Cdecl)]
        public static IntPtr CreatePoint(int x, int y)
        {
            Point point = new Point { X = x, Y = y };
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(point));
            Marshal.StructureToPtr(point, ptr, false);
            return ptr; // Return pointer to the unmanaged memory
        }

        // This will calculate and return the sum of X and Y in Point
        [DllExport("GetPointSum", CallingConvention = CallingConvention.Cdecl)]
        public static int GetPointSum(Point point)
        {
            return point.X + point.Y;
        }

        // This will free the allocated memory for Point
        [DllExport("FreePoint", CallingConvention = CallingConvention.Cdecl)]
        public static void FreePoint(IntPtr ptr)
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
}
