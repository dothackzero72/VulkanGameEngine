using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    public class RenderPipelineModel : RenderPassEditorBaseModel
    {
        public String VertexShader { get; set; }
        public String FragmentShader { get; set; }
        public List<VkViewport> ViewportList { get; set; } = new List<VkViewport>();
        public List<VkRect2D> ScissorList { get; set; } = new List<VkRect2D>();
        public List<VkPipelineColorBlendAttachmentState> PipelineColorBlendAttachmentStateList { get; set; } = new List<VkPipelineColorBlendAttachmentState>();
        public PipelineColorBlendStateCreateInfoModel PipelineColorBlendStateCreateInfoModel { get; set; } = new PipelineColorBlendStateCreateInfoModel();
        public PipelineRasterizationStateCreateInfoModel PipelineRasterizationStateCreateInfo { get; set; } = new PipelineRasterizationStateCreateInfoModel();
        public PipelineMultisampleStateCreateInfoModel PipelineMultisampleStateCreateInfo { get; set; } = new PipelineMultisampleStateCreateInfoModel();
        public PipelineDepthStencilStateCreateInfoModel PipelineDepthStencilStateCreateInfo { get; set; }
        public PipelineInputAssemblyStateCreateInfoModel PipelineInputAssemblyStateCreateInfo { get; set; } = new PipelineInputAssemblyStateCreateInfoModel();
        public List<VkDescriptorSetLayoutBinding> LayoutBindingList { get; set; } = new List<VkDescriptorSetLayoutBinding>();
        public List<PipelineDescriptorModel> PipelineDescriptorModelsList { get; set; } = new List<PipelineDescriptorModel>();

        public RenderPipelineModel()
        {
        }

        public RenderPipelineModel(string name) : base(name)
        {
        }
    }
}
