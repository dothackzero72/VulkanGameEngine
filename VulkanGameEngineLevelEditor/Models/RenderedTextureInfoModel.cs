using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public enum RenderedTextureType
    {
        ColorRenderedTexture,
        DepthRenderedTexture,
        InputAttachmentTexture,
        ResolveAttachmentTexture
    };

    public class RenderedTextureInfoModel
    {
        public RenderedTextureType TextureType { get; set; }
        public ImageCreateInfoModel ImageCreateInfo { get; set; } = new ImageCreateInfoModel();
        public SamplerCreateInfoModel SamplerCreateInfo { get; set; } = new SamplerCreateInfoModel();
        public RenderedTextureInfoModel()
        {
        }
    }
}
