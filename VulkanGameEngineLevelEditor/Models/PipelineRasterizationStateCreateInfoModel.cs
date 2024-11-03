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
    public class PipelineRasterizationStateCreateInfoModel
    {
        public bool DepthClampEnable { get; set; }
        public bool RasterizerDiscardEnable { get; set; }
        public PolygonMode PolygonMode { get; set; }
        public CullModeFlags CullMode { get; set; }
        public FrontFace FrontFace { get; set; }
        public bool DepthBiasEnable { get; set; }
        public float DepthBiasConstantFactor { get; set; }
        public float DepthBiasClamp { get; set; }
        public float DepthBiasSlopeFactor { get; set; }
        public float LineWidth { get; set; }
    }
}
