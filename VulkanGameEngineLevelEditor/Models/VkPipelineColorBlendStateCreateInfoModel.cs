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
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
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

    public unsafe class VkPipelineColorBlendStateCreateInfoModel
    {
        public VkStructureType sType { get; set; }
        [JsonIgnore]
        public void* pNext { get; set; }
        public VkPipelineColorBlendStateCreateFlagBits flags { get; set; }
        public bool logicOpEnable { get; set; }
        public VkLogicOp logicOp { get; set; }
        public uint attachmentCount { get; set; }
        [JsonIgnore]
        public VkPipelineColorBlendAttachmentState* pAttachments { get; set; }
        public Blender blendConstants;

        public VkPipelineColorBlendStateCreateInfoModel()
        {
            sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO;
            pNext = null;
            flags = 0;
            logicOpEnable = false;
            logicOp = VkLogicOp.VK_LOGIC_OP_CLEAR;
            attachmentCount = 0;
            pAttachments = null;
            blendConstants.R = 0.0f;
            blendConstants.G = 0.0f;
            blendConstants.B = 0.0f;
            blendConstants.A = 0.0f;
        }

        public VkPipelineColorBlendStateCreateInfo Convert()
        {
            VkPipelineColorBlendStateCreateInfo result = new VkPipelineColorBlendStateCreateInfo
            {
                sType = sType,
                pNext = pNext,
                flags = flags,
                logicOpEnable = logicOpEnable,
                logicOp = logicOp,
                attachmentCount = attachmentCount,
                pAttachments = pAttachments,
                //blendConstants = new Blender()
                //{
                //    R = blendConstants.R,
                //    G = blendConstants.G,
                //    B = blendConstants.B,
                //    A = blendConstants.A
                //}
            };
            result.blendConstants.R = blendConstants.R;
            result.blendConstants.G = blendConstants.G;
            result.blendConstants.B = blendConstants.B;
            result.blendConstants.A = blendConstants.A;
            return result;
        }

        public VkPipelineColorBlendStateCreateInfo* ConvertPtr()
        {
            VkPipelineColorBlendStateCreateInfo* ptr = (VkPipelineColorBlendStateCreateInfo*)Marshal.AllocHGlobal(sizeof(VkPipelineColorBlendStateCreateInfo));
            ptr->sType = sType;
            ptr->pNext = pNext;
            ptr->flags = flags;
            ptr->logicOpEnable = false;
            //ptr->LogicOp = logicOp;
            ptr->attachmentCount = attachmentCount;
            ptr->pAttachments = pAttachments;
            //ptr->BlendConstants[0] = blendConstants.R;
            //ptr->BlendConstants[2] = blendConstants.G;
            //ptr->BlendConstants[3] = blendConstants.B;
            //ptr->BlendConstants[4] = blendConstants.A;
            return ptr;
        }

        public void Dispose()
        {
        }
    }
}
