using Silk.NET.Core.Attributes;
using Silk.NET.Core;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe struct VkPipelineColorBlendStateCreateInfo
    {
        public StructureType sType { get; set; }
        public void* pNext { get; set; }
        public PipelineColorBlendStateCreateFlags flags { get; set; }
        public Bool32 logicOpEnable { get; set; }
        public LogicOp logicOp { get; set; }
        public uint attachmentCount { get; set; }
        public PipelineColorBlendAttachmentState* pAttachments { get; set; }
        public fixed float blendConstants[4];

        public VkPipelineColorBlendStateCreateInfo()
        {
            sType = StructureType.PipelineColorBlendStateCreateInfo;
            pNext = null;
            flags = 0;
            logicOpEnable = Vk.False;
            logicOp = LogicOp.Clear;
            attachmentCount = 0;
            pAttachments = null;
            blendConstants[0] = 0.0f;
            blendConstants[1] = 0.0f;
            blendConstants[2] = 0.0f;
            blendConstants[3] = 0.0f;
        }

        public VkPipelineColorBlendStateCreateInfo(PipelineColorBlendStateCreateInfo other)
        {
            sType = other.SType;
            pNext = other.PNext;
            flags = other.Flags;
            logicOpEnable = other.LogicOpEnable;
            logicOp = other.LogicOp;
            attachmentCount = other.AttachmentCount;
            pAttachments = other.PAttachments;
            for (int i = 0; i < 4; i++)
            {
                blendConstants[i] = other.BlendConstants[i];
            }
        }

        public PipelineColorBlendStateCreateInfo Convert()
        {
            PipelineColorBlendStateCreateInfo result = new PipelineColorBlendStateCreateInfo
            {
                SType = sType,
                PNext = pNext,
                Flags = flags,
                LogicOpEnable = logicOpEnable,
                LogicOp = logicOp,
                AttachmentCount = attachmentCount,
                PAttachments = pAttachments
            };
            for (int i = 0; i < 4; i++)
            {
                result.BlendConstants[i] = blendConstants[i];
            }
            return result;
        }

        public PipelineColorBlendStateCreateInfo* ConvertPtr()
        {
            PipelineColorBlendStateCreateInfo* ptr = (PipelineColorBlendStateCreateInfo*)Marshal.AllocHGlobal(sizeof(PipelineColorBlendStateCreateInfo));
            ptr->SType = sType;
            ptr->PNext = pNext;
            ptr->Flags = flags;
            ptr->LogicOpEnable = logicOpEnable;
            ptr->LogicOp = logicOp;
            ptr->AttachmentCount = attachmentCount;
            ptr->PAttachments = pAttachments;
            for (int i = 0; i < 4; i++)
            {
                ptr->BlendConstants[i] = blendConstants[i];
            }
            return ptr;
        }

        static public PipelineColorBlendStateCreateInfo[] ConvertPtrArray(List<VkPipelineColorBlendStateCreateInfo> list)
        {
            return list.Select(x => x.Convert()).ToArray();
        }

        public void Dispose()
        {
        }
    }
}
