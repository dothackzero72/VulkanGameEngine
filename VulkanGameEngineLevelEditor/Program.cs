using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDL2;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using System.Threading;
using Silk.NET.Maths;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Windowing;
using GlmSharp;
using Silk.NET.SDL;
using VulkanGameEngineLevelEditor.Vulkan;
using Window = Silk.NET.Windowing.Window;
using System.Runtime.InteropServices;

namespace VulkanGameEngineLevelEditor
{
    public class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDpiAwarenessContext(IntPtr value);

        private const int DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = -4;

        [STAThread]
        static void Main()
        {
            SetProcessDpiAwarenessContext((IntPtr)DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}