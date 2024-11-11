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
        public VkPipelineColorBlendStateCreateInfo PipelineColorBlendStateCreateInfoModel { get; set; } = new VkPipelineColorBlendStateCreateInfo();
        public VkPipelineRasterizationStateCreateInfo PipelineRasterizationStateCreateInfo { get; set; } = new VkPipelineRasterizationStateCreateInfo();
        public VkPipelineMultisampleStateCreateInfo PipelineMultisampleStateCreateInfo { get; set; } = new VkPipelineMultisampleStateCreateInfo();
        public VkPipelineDepthStencilStateCreateInfo PipelineDepthStencilStateCreateInfo { get; set; }
        public VkPipelineInputAssemblyStateCreateInfo PipelineInputAssemblyStateCreateInfo { get; set; } = new VkPipelineInputAssemblyStateCreateInfo();
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
