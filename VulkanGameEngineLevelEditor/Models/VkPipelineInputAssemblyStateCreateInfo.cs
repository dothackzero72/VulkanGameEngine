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
    public unsafe class VkPipelineInputAssemblyStateCreateInfo
    {
        public StructureType sType { get; set; } = StructureType.PipelineInputAssemblyStateCreateInfo;
        public PrimitiveTopology topology { get; set; }
        public bool primitiveRestartEnable { get; set; }
        public uint flags { get; set; } = 0;
        public void* pNext { get; set; } = null;
        public VkPipelineInputAssemblyStateCreateInfo() { }
        public PipelineInputAssemblyStateCreateInfo* ConvertPtr()
        {
            PipelineInputAssemblyStateCreateInfo* ptr = (PipelineInputAssemblyStateCreateInfo*)Marshal.AllocHGlobal(sizeof(PipelineInputAssemblyStateCreateInfo));
            ptr->SType = sType;
            ptr->Topology = topology;
            ptr->PrimitiveRestartEnable = primitiveRestartEnable;
            ptr->Flags = 0;
            ptr->PNext = null;
            return ptr;
        }
    }
}
