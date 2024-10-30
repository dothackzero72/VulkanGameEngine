using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public class RenderPipeline : RenderPassEditorBaseModel
    {
        public String VertexShader;
        public String FragmentShader;
        public List<Viewport> viewportList = new List<Viewport>();
        public List<Rect2D> scissorList = new List<Rect2D>();
        public PipelineInputAssemblyStateCreateInfoModel PipelineInputAssemblyStateCreateInfo { get; private set; }

        public RenderPipeline()
        {
        }

        public RenderPipeline(string name) : base(name)
        {
        }
    }
}
