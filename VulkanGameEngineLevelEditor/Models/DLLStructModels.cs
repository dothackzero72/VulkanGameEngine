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
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GPUIncludes
    {
        public VkDescriptorBufferInfo* vertexProperties;
        public VkDescriptorBufferInfo* indexProperties;
        public VkDescriptorBufferInfo* transformProperties;
        public VkDescriptorBufferInfo* meshProperties;
        public VkDescriptorImageInfo* texturePropertiesList;
        public VkDescriptorBufferInfo* materialProperties;
        public uint vertexPropertiesCount;
        public uint indexPropertiesCount;
        public uint transformPropertiesCount;
        public uint meshPropertiesCount;
        public uint texturePropertiesListCount;
        public uint materialPropertiesCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct VkPipelineColorBlendStateCreateInfoDLL
    {
        public VkStructureType sType { get; set; }
        public IntPtr pNext { get; set; }
        public VkPipelineColorBlendStateCreateFlagBits flags { get; set; }
        public VkBool32 logicOpEnable { get; set; }
        public VkLogicOp logicOp { get; set; }
        public uint attachmentCount { get; set; }
        public VkPipelineColorBlendAttachmentState* pAttachments { get; set; }
        public fixed float blendConstants[4];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct VkSamplerCreateInfoDLL
    {
        public IntPtr Name;
        public VkStructureType _sType;
        public VkSamplerCreateFlagBits _flags;
        public void* _pNext = null;
        public VkFilter _magFilter;
        public VkFilter _minFilter;
        public VkSamplerMipmapMode _mipmapMode;
        public VkSamplerAddressMode _addressModeU;
        public VkSamplerAddressMode _addressModeV;
        public VkSamplerAddressMode _addressModeW;
        public float _mipLodBias;
        public bool _anisotropyEnable;
        public float _maxAnisotropy;
        public bool _compareEnable;
        public VkCompareOp _compareOp;
        public float _minLod;
        public float _maxLod;
        public VkBorderColor _borderColor;
        public bool _unnormalizedCoordinates;

        public VkSamplerCreateInfoDLL()
        {
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct RenderedTextureInfoDLL
    {
        //public IntPtr Name;
        //public IntPtr _renderedTextureInfoName;
        public VkImageCreateInfo _imageCreateInfo;
        public VkSamplerCreateInfo _samplerCreateInfo;
        public VkAttachmentDescription _attachmentDescription;
        public RenderedTextureType _textureType;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct RenderPipelineDLL
    {
        public IntPtr Name;
        public IntPtr VertexShader;
        public IntPtr FragmentShader;

        public uint DescriptorSetCount;
        public uint DescriptorSetLayoutCount;
        public uint ViewportListCount;
        public uint ScissorListCount;
        public uint PipelineColorBlendAttachmentStateListCount;
        public uint LayoutBindingListCount;
        public uint PipelineDescriptorListCount;

        public VkViewport* ViewportList;
        public VkRect2D* ScissorList;
        public VkDescriptorSetLayoutBinding* LayoutBindingList;
        public PipelineDescriptorModel* PipelineDescriptorList;
        public VkPipelineColorBlendAttachmentState* PipelineColorBlendAttachmentStateList;
        public VkPipelineColorBlendStateCreateInfo PipelineColorBlendStateCreateInfo;
        public VkPipelineRasterizationStateCreateInfo PipelineRasterizationStateCreateInfo;
        public VkPipelineMultisampleStateCreateInfo PipelineMultisampleStateCreateInfo;
        public VkPipelineDepthStencilStateCreateInfo PipelineDepthStencilStateCreateInfo;
        public VkPipelineInputAssemblyStateCreateInfo PipelineInputAssemblyStateCreateInfo;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct RenderPassBuildInfoDLL
    {
       // public IntPtr Name;
       // public IntPtr* RenderPipelineList;
        public RenderedTextureInfoDLL* RenderedTextureInfoModelList;
        public VkSubpassDependency* SubpassDependencyList;
        public bool IsRenderedToSwapchain;

        public uint RenderPipelineCount;
        public uint RenderedTextureInfoModeCount;
        public uint SubpassDependencyCount;
    }
}
