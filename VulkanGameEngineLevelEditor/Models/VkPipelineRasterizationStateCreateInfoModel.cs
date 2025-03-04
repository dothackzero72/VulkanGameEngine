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
using Newtonsoft.Json;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkPipelineRasterizationStateCreateInfoModel
    {
        public VkStructureType sType { get; set; } = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO;
        public bool depthClampEnable { get; set; }
        public bool rasterizerDiscardEnable { get; set; }
        public VkPolygonMode polygonMode { get; set; }
        public VkCullModeFlagBits cullMode { get; set; }
        public VkFrontFace frontFace { get; set; }
        public bool depthBiasEnable { get; set; }
        public float depthBiasConstantFactor { get; set; }
        public float depthBiasClamp { get; set; }
        public float depthBiasSlopeFactor { get; set; }
        public float lineWidth { get; set; }
        public uint flags { get; set; } = 0;
        [JsonIgnore]
        public void* pNext { get; set; } = null;

       public VkPipelineRasterizationStateCreateInfoModel() { }

        public VkPipelineRasterizationStateCreateInfo* ConvertPtr()
        {
            VkPipelineRasterizationStateCreateInfo* ptr = (VkPipelineRasterizationStateCreateInfo*)Marshal.AllocHGlobal(sizeof(VkPipelineRasterizationStateCreateInfo));
            ptr->sType = sType;
            ptr->depthClampEnable = depthClampEnable;
            ptr->rasterizerDiscardEnable = depthClampEnable;
            ptr->polygonMode = polygonMode;
            ptr->cullMode = cullMode;
            ptr->frontFace = frontFace;
            ptr->depthBiasEnable = depthBiasEnable;
            ptr->depthBiasConstantFactor = depthBiasConstantFactor;
            ptr->depthBiasClamp = depthBiasClamp;
            ptr->depthBiasSlopeFactor = depthBiasSlopeFactor;
            ptr->lineWidth = lineWidth;
            ptr->flags = 0;
            ptr->pNext = null;
            return ptr;
        }
    }
}
