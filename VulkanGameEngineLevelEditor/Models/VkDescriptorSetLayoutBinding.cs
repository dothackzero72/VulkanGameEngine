using Newtonsoft.Json;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkDescriptorSetLayoutBinding
    {
        public uint binding { get; set; }
        public DescriptorType descriptorType { get; set; }
        public uint descriptorCount { get; set; }
        public ShaderStageFlags stageFlags { get; set; }
        [JsonIgnore]
        public Sampler* pImmutableSamplers { get; set; }

        public VkDescriptorSetLayoutBinding() { }

        public VkDescriptorSetLayoutBinding(DescriptorSetLayoutBinding other)
        {
            binding = other.Binding;
            descriptorType = other.DescriptorType;
            descriptorCount = other.DescriptorCount;
            stageFlags = other.StageFlags;
            pImmutableSamplers = other.PImmutableSamplers; // Note: Copying pointers directly
        }

        public DescriptorSetLayoutBinding Convert()
        {
            return new DescriptorSetLayoutBinding
            {
                Binding = binding,
                DescriptorType = descriptorType,
                DescriptorCount = descriptorCount,
                StageFlags = stageFlags,
                PImmutableSamplers = pImmutableSamplers
            };
        }

        public DescriptorSetLayoutBinding* ConvertPtr()
        {
            DescriptorSetLayoutBinding* ptr = (DescriptorSetLayoutBinding*)Marshal.AllocHGlobal(sizeof(DescriptorSetLayoutBinding));
            ptr->Binding = binding;
            ptr->DescriptorType = descriptorType;
            ptr->DescriptorCount = descriptorCount;
            ptr->StageFlags = stageFlags;
            ptr->PImmutableSamplers = pImmutableSamplers;
            return ptr;
        }

        public void Dispose()
        {
            // Implement disposal logic if necessary for managed resources if any
        }
    }
}
