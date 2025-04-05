using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class SpriteSheet
    {
        [JsonIgnore]
        public Material SpriteMaterial { get; set; }
        [JsonIgnore]
        public ivec2 SpriteCells { get; set; }
        [JsonIgnore]
        public vec2 SpriteUVSize { get; set; }
        [JsonIgnore]
        public uint SpriteLayer { get; set; }
        public ivec2 SpritePixelSize { get; set; }
        public vec2 SpriteScale { get; set; }

        public SpriteSheet()
        {

        }

        public SpriteSheet(Material spriteMaterial, ivec2 spritePixelSize, uint spriteLayer)
        {
            SpriteMaterial = spriteMaterial;
            SpritePixelSize = spritePixelSize;
            SpriteCells = new ivec2(spriteMaterial.AlbedoMap.Width / spritePixelSize.x, spriteMaterial.AlbedoMap.Height / spritePixelSize.y);
            SpriteUVSize = new vec2(1.0f / (float)SpriteCells.x, 1.0f / (float)SpriteCells.y);
            SpriteLayer = spriteLayer;
            SpriteScale = new vec2(5.0f);
        }

        public SpriteSheet(Material spriteMaterial, ivec2 spritePixelSize, uint spriteLayer, vec2 spriteScale)
        {
            SpriteMaterial = spriteMaterial;
            SpritePixelSize = spritePixelSize;
            SpriteCells = new ivec2(spriteMaterial.AlbedoMap.Width / spritePixelSize.x, spriteMaterial.AlbedoMap.Height / spritePixelSize.y);
            SpriteUVSize = new vec2(1.0f / (float)SpriteCells.x, 1.0f / (float)SpriteCells.y);
            SpriteLayer = spriteLayer;
            SpriteScale = spriteScale;
        }
    }
}
