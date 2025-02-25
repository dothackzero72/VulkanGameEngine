using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkRect2D
    {
        public VkOffset2D offset { get; set; }
        public VkExtent2D extent { get; set; }

        public VkRect2D()
        {
            offset = new VkOffset2D();
            extent = new VkExtent2D();
        }

        public VkRect2D(VkOffset2D offset, VkExtent2D extent)
        {
            this.offset = offset;
            this.extent = extent;
        }

        public VkRect2D(Rect2D other)
        {
            offset = new VkOffset2D()
            {
                x = other.Offset.X,
                y = other.Offset.Y
            };
            extent = new VkExtent2D()
            {
                width = other.Extent.Width,
                height = other.Extent.Height
            };
        }

        public Rect2D Convert()
        {
            return new Rect2D
            {
                Offset = new Offset2D()
                {
                    X = offset.x,
                    Y = offset.y
                },
                Extent = new Extent2D()
                {
                    Width = extent.width,
                    Height = extent.height
                }
            };
        }

        public Rect2D* ConvertPtr()
        {
            Rect2D* rect = (Rect2D*)Marshal.AllocHGlobal(sizeof(Rect2D));
            rect->Offset = new Offset2D()
            {
                X = offset.x,
                Y = offset.y
            };
            rect->Extent = new Extent2D()
            {
                Width = extent.width,
                Height = extent.height
            };
            return rect;
        }

        public void Dispose()
        {
            // Implement disposal logic if necessary
        }
    }
}
