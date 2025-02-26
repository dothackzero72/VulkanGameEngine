using Silk.NET.Core;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkPipelineColorBlendAttachmentState
    {
        public Bool32 blendEnable { get; set; }
        public VkBlendFactor srcColorBlendFactor { get; set; }
        public VkBlendFactor dstColorBlendFactor { get; set; }
        public VkBlendOp colorBlendOp { get; set; }
        public VkBlendFactor srcAlphaBlendFactor { get; set; }
        public VkBlendFactor dstAlphaBlendFactor { get; set; }
        public VkBlendOp alphaBlendOp { get; set; }
        public VkColorComponentFlagBits colorWriteMask { get; set; }

        public VkPipelineColorBlendAttachmentState() { }

        public void Dispose()
        {
            // Implement disposal logic if necessary for managed resources if any
        }
    }
}
