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

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkPipelineDepthStencilStateCreateInfo
    {
        public StructureType sType { get; set; } = StructureType.PipelineDepthStencilStateCreateInfo;
        public bool depthTestEnable { get; set; }
        public bool depthWriteEnable { get; set; }
        public CompareOp depthCompareOp { get; set; }
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
        public PipelineDepthStencilStateCreateInfo* ConvertPtr()
        {
            PipelineDepthStencilStateCreateInfo* ptr = (PipelineDepthStencilStateCreateInfo*)Marshal.AllocHGlobal(sizeof(PipelineDepthStencilStateCreateInfo));
            ptr->SType = sType;
            ptr->DepthTestEnable = depthTestEnable;
            ptr->DepthWriteEnable = depthWriteEnable;
            ptr->DepthCompareOp = depthCompareOp;
            ptr->DepthBoundsTestEnable = depthBoundsTestEnable;
            ptr->StencilTestEnable = stencilTestEnable;
            ptr->MinDepthBounds = minDepthBounds;
            ptr->MaxDepthBounds = maxDepthBounds;
            ptr->Front = new StencilOpState();
            ptr->Back = new StencilOpState();
            ptr->Flags = 0;
            ptr->PNext = null;

            if(stencilTestEnable)
            {
                ptr->Front = front.Convert();
                ptr->Back = back.Convert();
            }

            return ptr;
        }
    }
}
