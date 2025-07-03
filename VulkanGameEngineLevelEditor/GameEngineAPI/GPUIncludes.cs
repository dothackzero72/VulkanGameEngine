using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Systems;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe class GPUIncludes
    {
        public VkDescriptorBufferInfo* vertexPropertiesList { get; set; }
        public VkDescriptorBufferInfo* indexPropertiesList { get; set; } 
        public VkDescriptorBufferInfo* transformPropertiesList { get; set; } 
        public VkDescriptorBufferInfo* meshPropertiesList { get; set; }
        public VkDescriptorBufferInfo* levelLayerMeshPropertiesList { get; set; }
        public VkDescriptorImageInfo* texturePropertiesList { get; set; }
        public VkDescriptorBufferInfo* materialPropertiesList { get; set; }
        public size_t vertexPropertiesListCount { get; set; } = 0;
        public size_t indexPropertiesListCount { get; set; } = 0;
        public size_t transformPropertiesListCount { get; set; } = 0;
        public size_t meshPropertiesListCount { get; set; } = 0;
        public size_t levelLayerMeshPropertiesListCount { get; set; } = 0;
        public size_t texturePropertiesListCount { get; set; } = 0;
        public size_t materialPropertiesListCount { get; set; } = 0;
    };
}

