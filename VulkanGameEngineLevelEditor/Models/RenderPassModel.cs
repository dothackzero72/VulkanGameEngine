using GlmSharp;
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
        ivec2 SwapChainResuloution = new ivec2();
        public List<RenderPipeline> RenderPipelineList { get; set; } = new List<RenderPipeline>();
        public List<RenderedTextureInfoModel> ColorAttachmentList { get; set; } = new List<RenderedTextureInfoModel>();
        public List<RenderedTextureInfoModel> DepthAttachmentList { get; set; } = new List<RenderedTextureInfoModel>();
        public List<RenderedTextureInfoModel> InputAttachmentList { get; set; } = new List<RenderedTextureInfoModel>();
        public List<RenderedTextureInfoModel> ResolveAttachmentList { get; set; } = new List<RenderedTextureInfoModel>();

        public RenderPassModel() 
        {
        }
    }
}
