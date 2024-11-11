using Silk.NET.Core.Attributes;
using Silk.NET.Core;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Runtime.InteropServices;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkPipelineRasterizationStateCreateInfo
    {
        public StructureType stype { get; set; } = StructureType.PipelineRasterizationConservativeStateCreateInfoExt;
        public bool depthClampEnable { get; set; }
        public bool rasterizerDiscardEnable { get; set; }
        public PolygonMode polygonMode { get; set; }
        public CullModeFlags cullMode { get; set; }
        public FrontFace frontFace { get; set; }
        public bool depthBiasEnable { get; set; }
        public float depthBiasConstantFactor { get; set; }
        public float depthBiasClamp { get; set; }
        public float depthBiasSlopeFactor { get; set; }
        public float lineWidth { get; set; }
        public uint flags { get; set; } = 0;
        public void* pNext { get; set; } = null;

       public VkPipelineRasterizationStateCreateInfo() { }

        public PipelineRasterizationStateCreateInfo* ConvertPtr()
        {
            PipelineRasterizationStateCreateInfo* ptr = (PipelineRasterizationStateCreateInfo*)Marshal.AllocHGlobal(sizeof(PipelineRasterizationStateCreateInfo));
            ptr->SType = stype;
            ptr->DepthClampEnable = depthClampEnable;
            ptr->RasterizerDiscardEnable = depthClampEnable;
            ptr->PolygonMode = polygonMode;
            ptr->CullMode = cullMode;
            ptr->FrontFace = frontFace;
            ptr->DepthBiasEnable = depthBiasEnable;
            ptr->DepthBiasConstantFactor = depthBiasConstantFactor;
            ptr->DepthBiasClamp = depthBiasClamp;
            ptr->DepthBiasSlopeFactor = depthBiasSlopeFactor;
            ptr->LineWidth = lineWidth;
            ptr->Flags  = 0;
            ptr->PNext  = null;
            return ptr;
        }
    }
}
