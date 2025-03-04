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
    public unsafe class VkPipelineInputAssemblyStateCreateInfoModel
    {
        public VkStructureType sType { get; set; } = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO;
        public VkPrimitiveTopology topology { get; set; }
        public bool primitiveRestartEnable { get; set; }
        public uint flags { get; set; } = 0;
        [JsonIgnore]
        public void* pNext { get; set; } = null;
        public VkPipelineInputAssemblyStateCreateInfoModel() { }

        public VkPipelineInputAssemblyStateCreateInfo* ConvertPtr()
        {
            VkPipelineInputAssemblyStateCreateInfo* ptr = (VkPipelineInputAssemblyStateCreateInfo*)Marshal.AllocHGlobal(sizeof(VkPipelineInputAssemblyStateCreateInfo));
            ptr->sType = sType;
            ptr->topology = topology;
            ptr->primitiveRestartEnable = primitiveRestartEnable;
            ptr->flags = 0;
            ptr->pNext = null;
            return ptr;
        }
    }
}
