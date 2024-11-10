using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public class VkViewport
    {
        public float x { get; set; }
        public float y { get; set; }
        public float width { get; set; }
        public float height { get; set; }
        public float minDepth { get; set; }
        public float maxDepth { get; set; }

        public VkViewport()
        { }

        public VkViewport(Viewport other)
        {
            x = other.X;
            y = other.Y;
            width = other.Width;
            height = other.Height;
            minDepth = other.MinDepth;
            maxDepth = other.MaxDepth;
        }

        public Viewport Convert()
        {
            return new Viewport
            {
                X = x,
                Y = y,
                Width = width,
                Height = height,
                MinDepth = minDepth,
                MaxDepth = maxDepth
            };
        }

        public void Dispose()
        {
        }
    }
}
