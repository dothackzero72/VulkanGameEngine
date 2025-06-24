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
        public IntPtr LevelLayerList { get; set; } = IntPtr.Zero;
        public size_t LevelLayerCount { get; set; } = 0;
        public size_t LevelLayerMapCount { get; set; } = 0;

        public LevelLayout()
        {
        }
    }
}
