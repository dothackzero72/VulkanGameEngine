using Newtonsoft.Json;
using System.Runtime.InteropServices;
using Vulkan;


namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe struct VkDescriptorSetLayoutBindingModel
    {
        public uint binding { get; set; }
        public VkDescriptorType descriptorType { get; set; }
        public uint descriptorCount { get; set; }
        public VkShaderStageFlagBits stageFlags { get; set; }
        [JsonIgnore]
        public VkSampler* pImmutableSamplers { get; set; }

        public VkDescriptorSetLayoutBindingModel() { }

        public VkDescriptorSetLayoutBinding Convert()
        {
            return new VkDescriptorSetLayoutBinding
            {
                binding = binding,
                descriptorType = descriptorType,
                descriptorCount = descriptorCount,
                stageFlags = stageFlags,
                pImmutableSamplers = pImmutableSamplers
            };
        }

        public VkDescriptorSetLayoutBinding* ConvertPtr()
        {
            VkDescriptorSetLayoutBinding* ptr = (VkDescriptorSetLayoutBinding*)Marshal.AllocHGlobal(sizeof(VkDescriptorSetLayoutBinding));
            ptr->binding = binding;
            ptr->descriptorType = descriptorType;
            ptr->descriptorCount = descriptorCount;
            ptr->stageFlags = stageFlags;
            ptr->pImmutableSamplers = pImmutableSamplers;
            return ptr;
        }

        public void Dispose()
        {
            // Implement disposal logic if necessary for managed resources if any
        }
    }
}
