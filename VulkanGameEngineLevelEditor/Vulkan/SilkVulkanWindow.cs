using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Windowing;

namespace VulkanGameEngineLevelEditor
{
    public unsafe class SilkVulkanWindow
    {
        public IWindow window { get; private set; }
        public SilkVulkanWindow() { }
        public IWindow CreateWindow(string windowName, Vector2D<int> size, out string[] requiredExtensions)
        {
            var options = WindowOptions.DefaultVulkan;
            options.Title = windowName;
            options.Size = size;
            /// options.IsResizable = true;

            // Create the window
            window = Window.Create(options);
            window.Initialize();

            var vulkanRequiredExtensions = window.VkSurface.GetRequiredExtensions(out uint extensions);
            requiredExtensions = new string[extensions];
            for (var x = 0; x < extensions; x++)
            {
                requiredExtensions[x] = Marshal.PtrToStringAnsi((IntPtr)vulkanRequiredExtensions[x]);
            }

            return window;
        }

    }
}
