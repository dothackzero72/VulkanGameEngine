using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Models
{
    public class PipelineDescriptorModel
    {
        public uint BindingNumber { get; set; }
        public DescriptorBindingPropertiesEnum BindingPropertiesList { get; set; }
        public DescriptorType DescriptorType { get; set; }
    }
}
