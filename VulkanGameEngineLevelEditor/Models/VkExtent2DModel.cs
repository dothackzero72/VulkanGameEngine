using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkExtent2DModel
    {
        public uint width { get; set; }
        public uint height { get; set; }

        public VkExtent2DModel()
        {
        }

        public VkExtent2D Convert()
        {
            return new VkExtent2D
            {
                width = width,
                height = height
            };
        }

        public VkExtent2D* ConvertPtr()
        {
            VkExtent2D* extent = (VkExtent2D*)Marshal.AllocHGlobal(sizeof(VkExtent2D));
            extent->width = width;
            extent->height = height;
            return extent;
        }

        public void Dispose()
        {
            // Implement disposal logic if necessary
        }
    }
}
