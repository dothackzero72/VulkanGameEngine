using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
     [StructLayout(LayoutKind.Sequential)]
    public struct VkExtent2D
    {
        public uint width;
        public uint height;
    }
}
