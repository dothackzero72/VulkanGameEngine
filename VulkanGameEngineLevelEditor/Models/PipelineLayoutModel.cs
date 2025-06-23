using Silk.NET.Vulkan;
using System.Collections.Generic;

namespace VulkanGameEngineLevelEditor.Models
{
    public class PipelineLayoutModel
    {
        public List<DescriptorSetLayoutBinding> LayoutBindingList { get; set; } = new List<DescriptorSetLayoutBinding>();
       // public List<DescriptorBindingPropertiesEnum> BindingList { get; set; } = new List<DescriptorBindingPropertiesEnum>();
        public List<DescriptorType> DescriptorList { get; set; } = new List<DescriptorType>();
        public List<DescriptorPoolSize> DescriptorPoolList { get; set; } = new List<DescriptorPoolSize>();

    }
}
