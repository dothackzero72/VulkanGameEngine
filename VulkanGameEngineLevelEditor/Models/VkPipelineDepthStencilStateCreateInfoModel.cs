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
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public unsafe struct VkPipelineDepthStencilStateCreateInfoModel
    {
        public VkStructureType sType { get; set; } = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO;
        public VkBool32 depthTestEnable { get; set; }
        public VkBool32 depthWriteEnable { get; set; }
        public VkCompareOp depthCompareOp { get; set; }
        public VkBool32 depthBoundsTestEnable { get; set; }
        public VkBool32 stencilTestEnable { get; set; }
        public VkStencilOpStateModel front { get; set; } = new VkStencilOpStateModel();
        public VkStencilOpStateModel back { get; set; } = new VkStencilOpStateModel();
        public float minDepthBounds { get; set; }
        public float maxDepthBounds { get; set; }
        public uint flags { get; set; } = 0;
        [JsonIgnore]
        public void* pNext { get; set; } = null;
        public VkPipelineDepthStencilStateCreateInfoModel() { }

        public VkPipelineDepthStencilStateCreateInfoDLL ConvertDLL()
        {
            return new VkPipelineDepthStencilStateCreateInfoDLL
            {
                sType = sType,
                depthTestEnable = depthTestEnable,
                depthWriteEnable = depthWriteEnable,
                depthCompareOp = depthCompareOp,
                depthBoundsTestEnable = depthBoundsTestEnable,
                stencilTestEnable = stencilTestEnable,
                minDepthBounds = minDepthBounds,
                maxDepthBounds = maxDepthBounds,
                front = front.ConvertDLL(),
                back = back.ConvertDLL(),
                flags = 0,
                pNext = null
            };
        }

        public VkPipelineDepthStencilStateCreateInfo Convert()
        {
            return new VkPipelineDepthStencilStateCreateInfo
            {
                sType = sType,
                depthTestEnable = depthTestEnable,
                depthWriteEnable = depthWriteEnable,
                depthCompareOp = depthCompareOp,
                depthBoundsTestEnable = depthBoundsTestEnable,
                stencilTestEnable = stencilTestEnable,
                minDepthBounds = minDepthBounds,
                maxDepthBounds = maxDepthBounds,
                front = front.Convert(),
                back = back.Convert(),
                flags = 0,
                pNext = null
            };
        }

        public VkPipelineDepthStencilStateCreateInfo* ConvertPtr()
        {
            VkPipelineDepthStencilStateCreateInfo* ptr = (VkPipelineDepthStencilStateCreateInfo*)Marshal.AllocHGlobal(sizeof(VkPipelineDepthStencilStateCreateInfo));
            ptr->sType = sType;
            ptr->depthTestEnable = depthTestEnable;
            ptr->depthWriteEnable = depthWriteEnable;
            ptr->depthCompareOp = depthCompareOp;
            ptr->depthBoundsTestEnable = depthBoundsTestEnable;
            ptr->stencilTestEnable = stencilTestEnable;
            ptr->minDepthBounds = minDepthBounds;
            ptr->maxDepthBounds = maxDepthBounds;
            ptr->front = new VkStencilOpState();
            ptr->back = new VkStencilOpState();
            ptr->flags = 0;
            ptr->pNext = null;

            if (stencilTestEnable == Vk.True)
            {
                ptr->front = front.Convert();
                ptr->back = back.Convert();
            }

            return ptr;
        }
    }
}
