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
    public class PipelineInputAssemblyStateCreateInfoModel
    {
        public PrimitiveTopology Topology { get; set; }
        public Bool32 PrimitiveRestartEnable { get; set; }
    }
}
