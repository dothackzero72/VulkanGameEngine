using Silk.NET.Core;
using System.Runtime.InteropServices;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkPipelineColorBlendAttachmentStateModel
    {
        public Bool32 blendEnable { get; set; }
        public VkBlendFactor srcColorBlendFactor { get; set; }
        public VkBlendFactor dstColorBlendFactor { get; set; }
        public VkBlendOp colorBlendOp { get; set; }
        public VkBlendFactor srcAlphaBlendFactor { get; set; }
        public VkBlendFactor dstAlphaBlendFactor { get; set; }
        public VkBlendOp alphaBlendOp { get; set; }
        public VkColorComponentFlagBits colorWriteMask { get; set; }

        public VkPipelineColorBlendAttachmentStateModel() { }

        public VkPipelineColorBlendAttachmentState Convert()
        {
            return new VkPipelineColorBlendAttachmentState
            {
                blendEnable = blendEnable,
                srcColorBlendFactor = srcColorBlendFactor,
                dstColorBlendFactor = dstColorBlendFactor,
                colorBlendOp = colorBlendOp,
                srcAlphaBlendFactor = srcAlphaBlendFactor,
                dstAlphaBlendFactor = dstAlphaBlendFactor,
                alphaBlendOp = alphaBlendOp,
                colorWriteMask = colorWriteMask
            };
        }

        public VkPipelineColorBlendAttachmentState* ConvertPtr()
        {
            VkPipelineColorBlendAttachmentState* ptr = (VkPipelineColorBlendAttachmentState*)Marshal.AllocHGlobal(sizeof(VkPipelineColorBlendAttachmentState));
            ptr->blendEnable = blendEnable;
            ptr->srcColorBlendFactor = srcColorBlendFactor;
            ptr->dstColorBlendFactor = dstColorBlendFactor;
            ptr->colorBlendOp = colorBlendOp;
            ptr->srcAlphaBlendFactor = srcAlphaBlendFactor;
            ptr->dstAlphaBlendFactor = dstAlphaBlendFactor;
            ptr->alphaBlendOp = alphaBlendOp;
            ptr->colorWriteMask = colorWriteMask;
            return ptr;
        }

        public void Dispose()
        {
            // Implement disposal logic if necessary for managed resources if any
        }
    }
}
