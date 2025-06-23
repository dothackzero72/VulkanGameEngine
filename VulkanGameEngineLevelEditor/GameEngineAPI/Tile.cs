using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class Tile
    {
        public uint TileId { get; set; } = 0;
        public ivec2 TileUVCellOffset { get; set; } = new ivec2();
        public vec2 TileUVOffset { get; set; } = new vec2();
        public int TileLayer { get; set; } = 0;
        public bool IsAnimatedTile { get; set; } = false;
    }
}
