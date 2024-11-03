using Silk.NET.Core.Attributes;
using Silk.NET.Core;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public class PipelineDepthStencilStateCreateInfoModel
    {
        public bool DepthTestEnable { get; set; }
        public bool DepthWriteEnable { get; set; }
        public CompareOp DepthCompareOp { get; set; }
        public bool DepthBoundsTestEnable { get; set; }
        public bool StencilTestEnable { get; set; }
        public StencilOpState Front { get; set; }
        public StencilOpState Back { get; set; }
        public float MinDepthBounds { get; set; }
        public float MaxDepthBounds { get; set; }
    }
}
