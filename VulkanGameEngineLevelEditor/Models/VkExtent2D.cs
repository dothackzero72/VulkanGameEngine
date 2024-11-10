using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkExtent2D
    {
        public uint width { get; set; }
        public uint height { get; set; }

        public VkExtent2D()
        {
        }

        public VkExtent2D(Extent2D other)
        {
            width = other.Width;
            height = other.Height;
        }

        public Extent2D Convert()
        {
            return new Extent2D
            {
                Width = width,
                Height = height
            };
        }

        public Extent2D* ConvertPtr()
        {
            Extent2D* extent = (Extent2D*)Marshal.AllocHGlobal(sizeof(Extent2D));
            extent->Width = width;
            extent->Height = height;
            return extent;
        }

        public void Dispose()
        {
            // Implement disposal logic if necessary
        }
    }
}
