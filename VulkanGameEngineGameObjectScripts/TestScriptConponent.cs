using RGiesecke.DllExport; // Ensure you've added this NuGet package
using System;
using System.Runtime.InteropServices;

namespace MyExportedFunctions
{
    public static class ExportedMethods
    {
        [DllExport("Add", CallingConvention = CallingConvention.StdCall)]
        public static int Add(int a, int b)
        {
            return a + b;
        }

        [DllExport("Multiply", CallingConvention = CallingConvention.StdCall)]
        public static int Multiply(int a, int b)
        {
            return a * b;
        }

        [DllExport("CallSayHello", CallingConvention = CallingConvention.StdCall)]
        public static void CallSayHello()
        {
            Console.WriteLine("Hello from C#");
        }
    }
}