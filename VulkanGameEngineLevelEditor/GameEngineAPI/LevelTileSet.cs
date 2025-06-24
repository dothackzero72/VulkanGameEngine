using GlmSharp;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe struct LevelTileSet
    {
        public Guid TileSetId { get; set; } = Guid.Empty;
        public Guid MaterialId { get; set; } = Guid.Empty;
        public vec2 TilePixelSize { get; set; } = new vec2();
        public ivec2 TileSetBounds { get; set; } = new ivec2();
        public vec2 TileScale { get; set; } = new vec2(5.0f);
        public vec2 TileUVSize { get; set; } = new vec2();
        public Tile* LevelTileListPtr { get; set; } = null;
        public size_t LevelTileCount { get; set; } = 0;
        public LevelTileSet()
        {
        }
    };
}
