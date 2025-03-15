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
    public unsafe struct VkPipelineMultisampleStateCreateInfoModel
    {
        public VkStructureType sType { get; set; } = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO;
        public VkSampleCountFlagBits rasterizationSamples { get; set; }
        public VkBool32 sampleShadingEnable { get; set; }
        public float minSampleShading { get; set; }
        [JsonIgnore]
        public uint* pSampleMask { get; set; } = null;
        public VkBool32 alphaToCoverageEnable { get; set; }
        public VkBool32 alphaToOneEnable { get; set; }
        public uint flags { get; set; } = 0;
        [JsonIgnore]
        public void* pNext { get; set; } = null;

        public VkPipelineMultisampleStateCreateInfoModel() 
        { 
        }

        public VkPipelineMultisampleStateCreateInfoDLL ConvertDLL()
        {
            return new VkPipelineMultisampleStateCreateInfoDLL()
            {
                sType = sType,
                rasterizationSamples = rasterizationSamples,
                sampleShadingEnable = sampleShadingEnable,
                minSampleShading = minSampleShading,
                pSampleMask = pSampleMask,
                alphaToCoverageEnable = alphaToCoverageEnable,
                alphaToOneEnable = alphaToOneEnable,
                flags = flags
            };
        }

        public VkPipelineMultisampleStateCreateInfo Convert()
        {
            return new VkPipelineMultisampleStateCreateInfo()
            {
                sType = sType,
                rasterizationSamples = rasterizationSamples,
                sampleShadingEnable = sampleShadingEnable,
                minSampleShading = minSampleShading,
                pSampleMask = null,
                alphaToCoverageEnable = alphaToCoverageEnable,
                alphaToOneEnable = alphaToOneEnable,
                flags = 0
            };
        }
    }
}
