using GlmSharp;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanCS;

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
