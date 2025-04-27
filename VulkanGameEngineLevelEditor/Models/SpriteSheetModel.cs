using GlmSharp;
using System;

namespace VulkanGameEngineLevelEditor.Models
{
    public class SpriteSheetModel
    {
        public ivec2 SpritePixelSize { get; set; }
        public vec2 SpriteScale { get; set; }
        public Guid MaterialID { get; set; }
    }
}
