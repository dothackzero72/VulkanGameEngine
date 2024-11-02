using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public class RenderPipelineModel : RenderPassEditorBaseModel
    {
        public String VertexShader { get; set; }
        public String FragmentShader { get; set; }
        public List<Viewport> ViewportList { get; set; } = new List<Viewport>();
        public List<Rect2D> ScissorList { get; set; } = new List<Rect2D>();
        public List<PipelineColorBlendAttachmentState> PipelineColorBlendAttachmentStateList { get; set; } = new List<PipelineColorBlendAttachmentState>();
        public PipelineColorBlendStateCreateInfoModel PipelineColorBlendStateCreateInfoModel { get; set; } = new PipelineColorBlendStateCreateInfoModel();
        public PipelineRasterizationStateCreateInfo PipelineRasterizationStateCreateInfo { get; set; } = new PipelineRasterizationStateCreateInfo();
        public PipelineMultisampleStateCreateInfo PipelineMultisampleStateCreateInfo { get; set; } = new PipelineMultisampleStateCreateInfo();
        public PipelineDepthStencilStateCreateInfo PipelineDepthStencilStateCreateInfo { get; set; } = new PipelineDepthStencilStateCreateInfo();
        public PipelineInputAssemblyStateCreateInfoModel PipelineInputAssemblyStateCreateInfo { get; set; } = new PipelineInputAssemblyStateCreateInfoModel();
        public List<DescriptorSetLayoutBinding> LayoutBindingList { get; set; } = new List<DescriptorSetLayoutBinding>();
        public List<PipelineDescriptorModel> PipelineDescriptorModelsList { get; set; } = new List<PipelineDescriptorModel>();

        public RenderPipelineModel()
        {
        }

        public RenderPipelineModel(string name) : base(name)
        {
        }
    }
}
