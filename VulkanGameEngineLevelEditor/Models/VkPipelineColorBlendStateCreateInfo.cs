using Silk.NET.Core.Attributes;
using Silk.NET.Core;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Silk.NET.Core.Native;
using Newtonsoft.Json;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkPipelineColorBlendStateCreateInfo
    {
        public struct Blender
        {
            public float R { get; set; } = 0.0f;
            public float G { get; set; } = 0.0f;
            public float B { get; set; } = 0.0f;
            public float A { get; set; } = 0.0f;
            public Blender()
            {
            }
        }

        public StructureType sType { get; set; }
        [JsonIgnore]
        public void* pNext { get; set; }
        public PipelineColorBlendStateCreateFlags flags { get; set; }
        public bool logicOpEnable { get; set; }
        public LogicOp logicOp { get; set; }
        public uint attachmentCount { get; set; }
        [JsonIgnore]
        public PipelineColorBlendAttachmentState* pAttachments { get; set; }
        public Blender blendConstants;

        public VkPipelineColorBlendStateCreateInfo()
        {
            sType = StructureType.PipelineColorBlendStateCreateInfo;
            pNext = null;
            flags = 0;
            logicOpEnable = false;
            logicOp = LogicOp.Clear;
            attachmentCount = 0;
            pAttachments = null;
            blendConstants.R = 0.0f;
            blendConstants.G = 0.0f;
            blendConstants.B = 0.0f;
            blendConstants.A = 0.0f;
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
            blendConstants.R = 0.0f;
            blendConstants.G = 0.0f;
            blendConstants.B = 0.0f;
            blendConstants.A = 0.0f;
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
            result.BlendConstants[0] = blendConstants.R;
            result.BlendConstants[2] = blendConstants.G;
            result.BlendConstants[3] = blendConstants.B;
            result.BlendConstants[4] = blendConstants.A;
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
            //ptr->BlendConstants[0] = blendConstants.R;
            //ptr->BlendConstants[2] = blendConstants.G;
            //ptr->BlendConstants[3] = blendConstants.B;
            //ptr->BlendConstants[4] = blendConstants.A;
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
