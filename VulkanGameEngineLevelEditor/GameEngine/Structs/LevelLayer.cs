using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
    public unsafe struct LevelLayer
    {
        public Guid LevelId { get; set; }
        public uint MeshId { get; set; }
        public Guid MaterialId { get; set; }
        public Guid TileSetId { get; set; }
        public int LevelLayerIndex { get; set; }
        public ivec2 LevelBounds { get; set; }
        public uint* TileIdMap { get; set; }
        public Tile* TileMap { get; set; }
        public Vertex2D* VertexList { get; set; }
        public uint* IndexList { get; set; }
        public size_t TileIdMapCount { get; set; }
        public size_t TileMapCount { get; set; }
        public size_t VertexListCount { get; set; }
        public size_t IndexListCount { get; set; }
    };
}
