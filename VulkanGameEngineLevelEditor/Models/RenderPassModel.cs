using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    public class RenderPassModel
    {
        public ivec2 SwapChainResuloution { get; set; } = new ivec2();
        public List<RenderPipeline> RenderPipelineList { get; set; } = new List<RenderPipeline>();
        public List<RenderedTextureInfoModel> AttachmentList { get; set; } = new List<RenderedTextureInfoModel>();
        public List<SubpassDependencyModel> SubpassDependencyList { get; set; } = new List<SubpassDependencyModel>();

        public RenderPassModel() 
        {
        }
    }
}
