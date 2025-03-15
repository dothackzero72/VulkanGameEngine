using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct PipelineDescriptorModel
    {
        public uint BindingNumber;
        public DescriptorBindingPropertiesEnum BindingPropertiesList;
        public VkDescriptorType DescriptorType;

        public PipelineDescriptorModel(uint bindingNumber, DescriptorBindingPropertiesEnum properties, VkDescriptorType type)
        {
            BindingNumber = bindingNumber;
            BindingPropertiesList = properties;
            DescriptorType = type;
        }
    }
}
