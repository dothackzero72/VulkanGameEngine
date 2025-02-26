using Newtonsoft.Json;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkDescriptorSetLayoutBinding
    {
        public uint binding { get; set; }
        public VkDescriptorType descriptorType { get; set; }
        public uint descriptorCount { get; set; }
        public VkShaderStageFlagBits stageFlags { get; set; }
        [JsonIgnore]
        public VkSampler* pImmutableSamplers { get; set; }

        public VkDescriptorSetLayoutBinding() { }

        public void Dispose()
        {
            // Implement disposal logic if necessary for managed resources if any
        }
    }
}
