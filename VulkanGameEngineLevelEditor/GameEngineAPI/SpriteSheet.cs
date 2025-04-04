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
        public Material SpriteMaterial;
        public ivec2 SpritePixelSize;
        public ivec2 SpriteCells;
        public vec2 SpriteUVSize;
        public uint SpriteLayer;
        public vec2 SpriteScale;

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
