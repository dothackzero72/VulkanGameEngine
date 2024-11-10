using Silk.NET.Core;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkPipelineColorBlendAttachmentState
    {
        public Bool32 blendEnable { get; set; }
        public BlendFactor srcColorBlendFactor { get; set; }
        public BlendFactor dstColorBlendFactor { get; set; }
        public BlendOp colorBlendOp { get; set; }
        public BlendFactor srcAlphaBlendFactor { get; set; }
        public BlendFactor dstAlphaBlendFactor { get; set; }
        public BlendOp alphaBlendOp { get; set; }
        public ColorComponentFlags colorWriteMask { get; set; }

        public VkPipelineColorBlendAttachmentState() { }

        public VkPipelineColorBlendAttachmentState(PipelineColorBlendAttachmentState other)
        {
            blendEnable = other.BlendEnable;
            srcColorBlendFactor = other.SrcColorBlendFactor;
            dstColorBlendFactor = other.DstColorBlendFactor;
            colorBlendOp = other.ColorBlendOp;
            srcAlphaBlendFactor = other.SrcAlphaBlendFactor;
            dstAlphaBlendFactor = other.DstAlphaBlendFactor;
            alphaBlendOp = other.AlphaBlendOp;
            colorWriteMask = other.ColorWriteMask;
        }

        public PipelineColorBlendAttachmentState Convert()
        {
            return new PipelineColorBlendAttachmentState
            {
                BlendEnable = blendEnable,
                SrcColorBlendFactor = srcColorBlendFactor,
                DstColorBlendFactor = dstColorBlendFactor,
                ColorBlendOp = colorBlendOp,
                SrcAlphaBlendFactor = srcAlphaBlendFactor,
                DstAlphaBlendFactor = dstAlphaBlendFactor,
                AlphaBlendOp = alphaBlendOp,
                ColorWriteMask = colorWriteMask
            };
        }

        public PipelineColorBlendAttachmentState* ConvertPtr()
        {
            PipelineColorBlendAttachmentState* ptr = (PipelineColorBlendAttachmentState*)Marshal.AllocHGlobal(sizeof(PipelineColorBlendAttachmentState));
            ptr->BlendEnable = blendEnable;
            ptr->SrcColorBlendFactor = srcColorBlendFactor;
            ptr->DstColorBlendFactor = dstColorBlendFactor;
            ptr->ColorBlendOp = colorBlendOp;
            ptr->SrcAlphaBlendFactor = srcAlphaBlendFactor;
            ptr->DstAlphaBlendFactor = dstAlphaBlendFactor;
            ptr->AlphaBlendOp = alphaBlendOp;
            ptr->ColorWriteMask = colorWriteMask;
            return ptr;
        }

        static public PipelineColorBlendAttachmentState[] ConvertPtrArray(List<VkPipelineColorBlendAttachmentState> list)
        {
            return list.Select(x => x.Convert()).ToArray();
        }

        public void Dispose()
        {
            // Implement disposal logic if necessary for managed resources if any
        }
    }
}
