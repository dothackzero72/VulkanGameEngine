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
    public unsafe class VkPipelineMultisampleStateCreateInfoModel
    {
        public VkStructureType sType { get; set; } = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO;
        public VkSampleCountFlagBits rasterizationSamples { get; set; }
        public bool sampleShadingEnable { get; set; }
        public float minSampleShading { get; set; }
        [JsonIgnore]
        public unsafe uint* pSampleMask { get; set; } = null;
        public bool alphaToCoverageEnable { get; set; }
        public bool alphaToOneEnable { get; set; }
        public uint flags { get; set; } = 0;
        [JsonIgnore]
        public void* pNext { get; set; } = null;
        public VkPipelineMultisampleStateCreateInfoModel() { }

        public VkPipelineMultisampleStateCreateInfo* ConvertPtr()
        {
            VkPipelineMultisampleStateCreateInfo* ptr = (VkPipelineMultisampleStateCreateInfo*)Marshal.AllocHGlobal(sizeof(VkPipelineMultisampleStateCreateInfo));
            ptr->sType = sType;
            ptr->rasterizationSamples = rasterizationSamples;
            ptr->sampleShadingEnable = sampleShadingEnable;
            ptr->minSampleShading = minSampleShading;
            ptr->pSampleMask = null;
            ptr->alphaToCoverageEnable = alphaToCoverageEnable;
            ptr->alphaToCoverageEnable = alphaToCoverageEnable;
            ptr->alphaToOneEnable = alphaToOneEnable;
            ptr->flags = 0;
            ptr->pNext = null;
            return ptr;
        }
    }
}
