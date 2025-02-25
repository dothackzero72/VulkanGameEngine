using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkOffset2D
    {
        public int x { get; set; }
        public int y { get; set; }

        public VkOffset2D() { }
        public VkOffset2D(int x, int y) 
        {
            this.x = x;
            this.y = y;
        }

        public VkOffset2D(Offset2D other)
        {
            x = other.X;
            y = other.Y;
        }

        public Offset2D Convert()
        {
            return new Offset2D
            {
                X = x,
                Y = y
            };
        }

        public Offset2D* ConvertPtr()
        {
            Offset2D* nativeOffset = (Offset2D*)Marshal.AllocHGlobal(sizeof(Offset2D));
            nativeOffset->X = x;
            nativeOffset->Y = y;
            return nativeOffset;
        }

        public void Dispose()
        {
            // Implement disposal logic if necessary
        }
    }
}
