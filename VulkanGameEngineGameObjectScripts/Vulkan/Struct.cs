using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineGameObjectScripts.Vulkan
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VkVertexInputBindingDescription
    {
        public UInt32 binding;
        public UInt32 stride;
        public VkVertexInputRate inputRate;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct VkVertexInputAttributeDescription
    {
        public UInt32 location;
        public UInt32 binding;
        public VkFormat format;
        public UInt32 offset;
    };
}
