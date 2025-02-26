using Silk.NET.Core.Attributes;
using Silk.NET.Core;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkPipelineDepthStencilStateCreateInfo
    {
        public VkStructureType sType { get; set; } = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO;
        public bool depthTestEnable { get; set; }
        public bool depthWriteEnable { get; set; }
        public VkCompareOp depthCompareOp { get; set; }
        public bool depthBoundsTestEnable { get; set; }
        public bool stencilTestEnable { get; set; }
        public VkStencilOpState front { get; set; } = new VkStencilOpState();
        public VkStencilOpState back { get; set; } = new VkStencilOpState();
        public float minDepthBounds { get; set; }
        public float maxDepthBounds { get; set; }
        public uint flags { get; set; } = 0;
        [JsonIgnore]
        public void* pNext { get; set; } = null;
        public VkPipelineDepthStencilStateCreateInfo() { }
    }
}
