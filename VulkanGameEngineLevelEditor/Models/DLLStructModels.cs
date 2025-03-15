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

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public unsafe struct VkPipelineColorBlendStateCreateInfoDLL
    {
        public VkStructureType sType { get; set; }
        public void* pNext { get; set; }
        public VkPipelineColorBlendStateCreateFlagBits flags { get; set; }
        public VkBool32 logicOpEnable { get; set; }
        public VkLogicOp logicOp { get; set; }
        public uint attachmentCount { get; set; }
        public VkPipelineColorBlendAttachmentState* pAttachments { get; set; }
        public fixed float blendConstants[4];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
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

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
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

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public unsafe struct VkPipelineDepthStencilStateCreateInfoDLL
    {
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

    [StructLayout(LayoutKind.Sequential)]
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
        public VkPipelineColorBlendStateCreateInfoDLL PipelineColorBlendStateCreateInfo;
        public VkPipelineRasterizationStateCreateInfoDLL PipelineRasterizationStateCreateInfo;
        public VkPipelineMultisampleStateCreateInfoDLL PipelineMultisampleStateCreateInfo;
        public VkPipelineDepthStencilStateCreateInfoDLL PipelineDepthStencilStateCreateInfo;
        public VkPipelineInputAssemblyStateCreateInfoDLL PipelineInputAssemblyStateCreateInfo;
    }
}
