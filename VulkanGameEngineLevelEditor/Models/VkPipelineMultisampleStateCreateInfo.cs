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
    public unsafe class VkPipelineMultisampleStateCreateInfo
    {
        public StructureType sType { get; set; } = StructureType.PipelineMultisampleStateCreateInfo;
        public SampleCountFlags rasterizationSamples { get; set; }
        public bool sampleShadingEnable { get; set; }
        public float minSampleShading { get; set; }
        [JsonIgnore]
        public unsafe uint* pSampleMask { get; set; } = null;
        public bool alphaToCoverageEnable { get; set; }
        public bool alphaToOneEnable { get; set; }
        public uint flags { get; set; } = 0;
        [JsonIgnore]
        public void* pNext { get; set; } = null;
        public VkPipelineMultisampleStateCreateInfo() { }
        public PipelineMultisampleStateCreateInfo* ConvertPtr()
        {
            PipelineMultisampleStateCreateInfo* ptr = (PipelineMultisampleStateCreateInfo*)Marshal.AllocHGlobal(sizeof(PipelineMultisampleStateCreateInfo));
            ptr->SType = sType;
            ptr->RasterizationSamples = rasterizationSamples;
            ptr->SampleShadingEnable = sampleShadingEnable;
            ptr->MinSampleShading = minSampleShading;
            ptr->PSampleMask = null;
            ptr->AlphaToCoverageEnable = alphaToCoverageEnable;
            ptr->AlphaToCoverageEnable = alphaToCoverageEnable;
            ptr->AlphaToOneEnable = alphaToOneEnable;
            ptr->Flags = 0;
            ptr->PNext = null;
            return ptr;
        }
    }
}
