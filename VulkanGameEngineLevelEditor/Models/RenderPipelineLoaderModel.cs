﻿using System;
using System.Collections.Generic;
using System.Linq;
using Vulkan;
using VulkanGameEngineLevelEditor.GameEngineAPI;


namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class RenderPipelineLoaderModel : RenderPassEditorBaseModel
    {
        public int RenderPipelineId = 0;
        public String VertexShader { get; set; }
        public String FragmentShader { get; set; }
        public size_t DescriptorSetCount { get; set; }
        public size_t DescriptorSetLayoutCount { get; set; }
        public VertexTypeEnum VertexType { get; set; }
        public List<VkViewport> ViewportList { get; set; } = new List<VkViewport>();
        public List<VkRect2D> ScissorList { get; set; } = new List<VkRect2D>();
        public List<VkPipelineColorBlendAttachmentState> PipelineColorBlendAttachmentStateList { get; set; } = new List<VkPipelineColorBlendAttachmentState>();
        public VkPipelineColorBlendStateCreateInfoModel PipelineColorBlendStateCreateInfoModel { get; set; } = new VkPipelineColorBlendStateCreateInfoModel();
        public VkPipelineRasterizationStateCreateInfoModel PipelineRasterizationStateCreateInfo { get; set; } = new VkPipelineRasterizationStateCreateInfoModel();
        public VkPipelineMultisampleStateCreateInfoModel PipelineMultisampleStateCreateInfo { get; set; } = new VkPipelineMultisampleStateCreateInfoModel();
        public VkPipelineDepthStencilStateCreateInfoModel PipelineDepthStencilStateCreateInfo { get; set; }
        public VkPipelineInputAssemblyStateCreateInfoModel PipelineInputAssemblyStateCreateInfo { get; set; } = new VkPipelineInputAssemblyStateCreateInfoModel();
        public List<VkDescriptorSetLayoutBindingModel> LayoutBindingList { get; set; } = new List<VkDescriptorSetLayoutBindingModel>();
        public List<PipelineDescriptorModel> PipelineDescriptorModelsList { get; set; } = new List<PipelineDescriptorModel>();
        public List<VkVertexInputBindingDescription> VertexInputBindingDescriptionList { get; set; } = new List<VkVertexInputBindingDescription>();
        public List<VkVertexInputAttributeDescription> VertexInputAttributeDescriptionList { get; set; } = new List<VkVertexInputAttributeDescription>();

        public RenderPipelineLoaderModel()
        {
        }

        public RenderPipelineLoaderModel(string name) : base(name)
        {
        }
    }
}
