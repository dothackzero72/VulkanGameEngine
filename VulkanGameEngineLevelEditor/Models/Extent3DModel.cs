using Silk.NET.Core.Attributes;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public class Extent3DModel
    {
        public uint Width { get; set; }
        public uint Height { get; set; }
        public uint Depth { get; set; }

        public Extent3DModel(uint? width = null, uint? height = null, uint? depth = null)
        {
            if (width.HasValue)
            {
                Width = width.Value;
            }

            if (height.HasValue)
            {
                Height = height.Value;
            }

            if (depth.HasValue)
            {
                Depth = depth.Value;
            }
        }

        public Extent3D ConvertToVulkan()
        {
            return new Extent3D()
            {
                Width = Width,
                Height = Height,
                Depth = Depth
            };
        }
    }
}
