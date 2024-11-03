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
    public class PipelineMultisampleStateCreateInfoModel
    {
        public SampleCountFlags RasterizationSamples { get; set; }
        public bool SampleShadingEnable { get; set; }
        public float MinSampleShading { get; set; }
        public unsafe uint* PSampleMask { get; set; }
        public bool AlphaToCoverageEnable { get; set; }
        public bool AlphaToOneEnable { get; set; }
    }
}
