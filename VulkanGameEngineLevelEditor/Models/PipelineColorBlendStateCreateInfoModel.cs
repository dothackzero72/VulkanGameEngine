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
    public unsafe struct PipelineColorBlendStateCreateInfoModel
    {
        public Bool32 LogicOpEnable;
        public LogicOp LogicOp;
        public float[] BlendConstants = new float[4];

        public PipelineColorBlendStateCreateInfoModel()
        {
        }
    }
}
