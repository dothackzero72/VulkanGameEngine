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

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct VkPipelineRasterizationStateCreateInfoDLL
    {
        public VkStructureType sType;
        public VkBool32 depthClampEnable;
        public VkBool32 rasterizerDiscardEnable;
        public VkPolygonMode polygonMode;
        public VkCullModeFlagBits cullMode;
        public VkFrontFace frontFace;
        public VkBool32 depthBiasEnable;
        public float depthBiasConstantFactor;
        public float depthBiasClamp;
        public float depthBiasSlopeFactor;
        public float lineWidth;
        public uint flags;
        public IntPtr pNext;
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
    public unsafe struct VkPipelineMultisampleStateCreateInfoDLL
    {
        public VkStructureType sType { get; set; }
        public VkSampleCountFlagBits rasterizationSamples { get; set; }
        public VkBool32 sampleShadingEnable { get; set; }
        public float minSampleShading { get; set; }
        public uint* pSampleMask { get; set; }
        public VkBool32 alphaToCoverageEnable { get; set; }
        public VkBool32 alphaToOneEnable { get; set; }
        public uint flags { get; set; }
        public void* pNext { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct VkPipelineInputAssemblyStateCreateInfoDLL
    {
        public VkStructureType sType { get; set; }
        public VkPrimitiveTopology topology { get; set; }
        public VkBool32 primitiveRestartEnable { get; set; }
        public uint flags { get; set; }
        public void* pNext { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public unsafe struct VkStencilOpStateDLL
    {
        public VkStencilOp failOp { get; set; }
        public VkStencilOp passOp { get; set; }
        public VkStencilOp depthFailOp { get; set; }
        public VkCompareOp compareOp { get; set; }
        public uint compareMask { get; set; }
        public uint writeMask { get; set; }
        public uint reference { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct VkExtent3DDLL
    {
        public IntPtr Name;
        public uint _width { get; set; }
        public uint _height { get; set; }
        public uint _depth { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct VkAttachmentDescriptionDLL
    {
        public IntPtr Name;
        public VkStructureType _structureType;
        public VkAttachmentDescriptionFlagBits _flags;
        public void* _pNext;
        public VkFormat _format;
        public VkSampleCountFlagBits _samples;
        public VkAttachmentLoadOp _loadOp;
        public VkAttachmentStoreOp _storeOp;
        public VkAttachmentLoadOp _stencilLoadOp;
        public VkAttachmentStoreOp _stencilStoreOp;
        public VkImageLayout _initialLayout;
        public VkImageLayout _finalLayout;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct VkImageCreateInfoDLL
    {
        public IntPtr Name;
        public VkStructureType _sType;
        public VkImageCreateFlagBits _flags;
        public void* _pNext;
        public VkImageType _imageType;
        public VkFormat _format;
        public VkExtent3DDLL _extent;
        public uint _mipLevels;
        public uint _arrayLayers;
        public VkSampleCountFlagBits _samples;
        public VkImageTiling _tiling;
        public VkImageUsageFlagBits _usage;
        public VkSharingMode _sharingMode;
        public uint _queueFamilyIndexCount;
        public unsafe uint* _pQueueFamilyIndices;
        public VkImageLayout _initialLayout;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct VkSubpassDependencyDLL
    {
        public IntPtr Name;
        public uint _srcSubpass;
        public uint _dstSubpass;
        public VkPipelineStageFlagBits _srcStageMask;
        public VkPipelineStageFlagBits _dstStageMask;
        public VkAccessFlagBits _srcAccessMask;
        public VkAccessFlagBits _dstAccessMask;
        public VkDependencyFlagBits _dependencyFlags;
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
    public unsafe struct VkPipelineDepthStencilStateCreateInfoDLL
    {
        public IntPtr Name;
        public VkStructureType sType { get; set; }
        public VkBool32 depthTestEnable { get; set; }
        public VkBool32 depthWriteEnable { get; set; }
        public VkCompareOp depthCompareOp { get; set; }
        public VkBool32 depthBoundsTestEnable { get; set; }
        public VkBool32 stencilTestEnable { get; set; }
        public VkStencilOpStateDLL front { get; set; }
        public VkStencilOpStateDLL back { get; set; }
        public float minDepthBounds { get; set; }
        public float maxDepthBounds { get; set; }
        public uint flags { get; set; }
        public void* pNext { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct TextureMemoryDLL
    {
        VkDescriptorSet ImGuiDescriptorSet;
        VkImage Image;
        VkDeviceMemory Memory;
        VkImageView View;
        VkSampler Sampler;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct RenderPipelineDLL
    {
        public IntPtr Name;
        public IntPtr VertexShader;
        public IntPtr FragmentShader;

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
