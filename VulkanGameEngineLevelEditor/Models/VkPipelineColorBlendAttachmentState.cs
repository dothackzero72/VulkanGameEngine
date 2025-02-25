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
        public VkColorComponentFlags colorWriteMask { get; set; }

        public VkPipelineColorBlendAttachmentState() { }

        public VkPipelineColorBlendAttachmentState(VkPipelineColorBlendAttachmentState other)
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

        public VkPipelineColorBlendAttachmentState Convert()
        {
            return new VkPipelineColorBlendAttachmentState
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

        public VkPipelineColorBlendAttachmentState* ConvertPtr()
        {
            VkPipelineColorBlendAttachmentState* ptr = (VkPipelineColorBlendAttachmentState*)Marshal.AllocHGlobal(sizeof(VkPipelineColorBlendAttachmentState));
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

        static public VkPipelineColorBlendAttachmentState[] ConvertPtrArray(List<VkPipelineColorBlendAttachmentState> list)
        {
            return list.Select(x => x.Convert()).ToArray();
        }

        public void Dispose()
        {
            // Implement disposal logic if necessary for managed resources if any
        }
    }
}
