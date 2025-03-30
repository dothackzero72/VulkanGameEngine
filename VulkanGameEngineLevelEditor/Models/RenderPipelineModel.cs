using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class RenderPipelineModel : RenderPassEditorBaseModel
    {
        public String VertexShader { get; set; }
        public String FragmentShader { get; set; }
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

        public RenderPipelineModel()
        {
        }

        public RenderPipelineModel(string name) : base(name)
        {
        }

        public unsafe RenderPipelineDLL ToDLL()
        {
            List<VkDescriptorSetLayoutBinding> LayoutBindingListDLL = LayoutBindingList.Select(x => x.Convert()).ToList();

            fixed (byte* namePtr = System.Text.Encoding.UTF8.GetBytes(_name + "\0"))
            fixed (byte* vertexShaderPtr = System.Text.Encoding.UTF8.GetBytes(VertexShader + "\0"))
            fixed (byte* fragmentShaderPtr = System.Text.Encoding.UTF8.GetBytes(FragmentShader + "\0"))
            fixed (VkViewport* viewportPtr = ViewportList.ToArray())
            fixed (VkRect2D* scissorPtr = ScissorList.ToArray())
            fixed (VkPipelineColorBlendAttachmentState* blendAttachmentPtr = PipelineColorBlendAttachmentStateList.ToArray())
            fixed (VkDescriptorSetLayoutBinding* layoutBindingPtr = LayoutBindingListDLL.ToArray())
            fixed (PipelineDescriptorModel* descriptorPtr = PipelineDescriptorModelsList.ToArray())
            {
                return new RenderPipelineDLL
                {
                    Name = (IntPtr)namePtr,
                    VertexShader = (IntPtr)vertexShaderPtr,
                    FragmentShader = (IntPtr)fragmentShaderPtr,
                    ViewportList = viewportPtr,
                    ScissorList = scissorPtr,
                    PipelineColorBlendAttachmentStateList = blendAttachmentPtr,
                    PipelineColorBlendStateCreateInfo = PipelineColorBlendStateCreateInfoModel.Convert(),
                    PipelineRasterizationStateCreateInfo = PipelineRasterizationStateCreateInfo.Convert(),
                    PipelineMultisampleStateCreateInfo = PipelineMultisampleStateCreateInfo.Convert(),
                    PipelineDepthStencilStateCreateInfo = PipelineDepthStencilStateCreateInfo.Convert(),
                    PipelineInputAssemblyStateCreateInfo = PipelineInputAssemblyStateCreateInfo.Convert(),
                    LayoutBindingList = layoutBindingPtr,
                    PipelineDescriptorList = descriptorPtr,
                    ViewportListCount = (uint)ViewportList.Count,
                    ScissorListCount = (uint)ScissorList.Count,
                    PipelineColorBlendAttachmentStateListCount = (uint)PipelineColorBlendAttachmentStateList.Count,
                    LayoutBindingListCount = (uint)LayoutBindingListDLL.Count,
                    PipelineDescriptorListCount = (uint)PipelineDescriptorModelsList.Count
                };
            }
        }
    }
}
