using GlmSharp;
using System;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
   public unsafe struct LevelLayout
    {
        public Guid LevelLayoutId { get; set; } = new Guid();
        public ivec2 LevelBounds { get; set; } = new ivec2();
        public ivec2 TileSizeinPixels { get; set; } = new ivec2();

        public LevelLayout()
        {
        }
    }
}
