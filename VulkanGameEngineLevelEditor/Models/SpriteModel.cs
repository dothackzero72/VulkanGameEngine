using GlmSharp;
using System;
using System.Collections.Generic;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Models
{
    public class SpriteModel
    {
        public vec2 SpriteSize { get; set; } = new vec2(50.0f);
        public vec4 SpriteColor { get; set; } = new vec4(0.0f, 0.0f, 0.0f, 1.0f);
        public vec2 SpritePosition { get; set; } = new vec2(0.0f);
        public vec2 SpriteRotation { get; set; } = new vec2(0.0f);
        public vec2 SpriteScale { get; set; } = new vec2(1.0f);
        public uint SpriteLayer { get; set; } = 0;
        public Guid SpriteMaterial { get; set; }
     //   public SpriteSheet Spritesheet { get; set; }
        public List<Animation2DModel> AnimationList { get; set; } = new List<Animation2DModel>();
    }
}
