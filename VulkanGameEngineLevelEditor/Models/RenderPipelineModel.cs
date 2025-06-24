using System;
using System.Collections.Generic;
using System.Linq;
using VulkanGameEngineLevelEditor.GameEngineAPI;


namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class RenderPipelineModel : RenderPassEditorBaseModel
    {
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
      //  public List<PipelineDescriptorModel> PipelineDescriptorModelsList { get; set; } = new List<PipelineDescriptorModel>();
        public List<VkVertexInputBindingDescription> VertexInputBindingDescriptionList { get; set; } = new List<VkVertexInputBindingDescription>();
        public List<VkVertexInputAttributeDescription> VertexInputAttributeDescriptionList { get; set; } = new List<VkVertexInputAttributeDescription>();
        public List<VkClearValue> ClearValueList { get; set; } = new List<VkClearValue>();

        public RenderPipelineModel()
        {
        }

        public RenderPipelineModel(string name) : base(name)
        {
        }

        //public unsafe RenderPipelineDLL ToDLL()
        //{
        //    List<VkDescriptorSetLayoutBinding> LayoutBindingListDLL = LayoutBindingList.Select(x => x.Convert()).ToList();

        //    fixed (byte* namePtr = System.Text.Encoding.UTF8.GetBytes(_name + "\0"))
        //    fixed (byte* vertexShaderPtr = System.Text.Encoding.UTF8.GetBytes(VertexShader + "\0"))
        //    fixed (byte* fragmentShaderPtr = System.Text.Encoding.UTF8.GetBytes(FragmentShader + "\0"))
        //    fixed (VkViewport* viewportPtr = ViewportList.ToArray())
        //    fixed (VkRect2D* scissorPtr = ScissorList.ToArray())
        //    fixed (VkPipelineColorBlendAttachmentState* blendAttachmentPtr = PipelineColorBlendAttachmentStateList.ToArray())
        //    fixed (VkDescriptorSetLayoutBinding* layoutBindingPtr = LayoutBindingListDLL.ToArray())
        //    fixed (PipelineDescriptorModel* descriptorPtr = PipelineDescriptorModelsList.ToArray())
        //    fixed (VkVertexInputBindingDescription* vertexInputBindingDescriptionPtr = VertexInputBindingDescriptionList.ToArray())
        //    fixed (VkVertexInputAttributeDescription* vertexInputAttributeDescriptionPtr = VertexInputAttributeDescriptionList.ToArray())
        //    fixed (VkClearValue* clearColorPtr = ClearValueList.ToArray())
        //    {
        //        return new RenderPipelineDLL
        //        {
        //            Name = (IntPtr)namePtr,
        //            VertexShader = (IntPtr)vertexShaderPtr,
        //            FragmentShader = (IntPtr)fragmentShaderPtr,
        //            VertexType = VertexType,
        //            ViewportList = viewportPtr,
        //            ScissorList = scissorPtr,
        //            DescriptorSetCount = DescriptorSetCount,
        //            DescriptorSetLayoutCount = DescriptorSetLayoutCount,
        //            PipelineColorBlendAttachmentStateList = blendAttachmentPtr,
        //            PipelineColorBlendStateCreateInfo = PipelineColorBlendStateCreateInfoModel.Convert(),
        //            PipelineRasterizationStateCreateInfo = PipelineRasterizationStateCreateInfo.Convert(),
        //            PipelineMultisampleStateCreateInfo = PipelineMultisampleStateCreateInfo.Convert(),
        //            PipelineDepthStencilStateCreateInfo = PipelineDepthStencilStateCreateInfo.Convert(),
        //            PipelineInputAssemblyStateCreateInfo = PipelineInputAssemblyStateCreateInfo.Convert(),
        //            VertexInputAttributeDescription = vertexInputAttributeDescriptionPtr,
        //            VertexInputBindingDescription = vertexInputBindingDescriptionPtr,
        //            LayoutBindingList = layoutBindingPtr,
        //            PipelineDescriptorList = descriptorPtr,
        //            ViewportListCount = ViewportList.Count,
        //            ScissorListCount = ScissorList.Count,
        //            PipelineColorBlendAttachmentStateListCount = PipelineColorBlendAttachmentStateList.Count,
        //            LayoutBindingListCount = LayoutBindingListDLL.Count,
        //            PipelineDescriptorListCount = PipelineDescriptorModelsList.Count,
        //            VertexInputAttributeDescriptionCount = VertexInputAttributeDescriptionList.Count(),
        //            VertexInputBindingDescriptionCount = VertexInputBindingDescriptionList.Count(),
        //        };
        //    }
        //}
    }
}
