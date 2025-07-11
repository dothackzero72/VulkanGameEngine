using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
    public struct Sprite
    {
        public enum SpriteAnimationEnum
        {
            kStanding,
            kWalking
        };

        public uint GameObjectId { get; set; }
        public uint SpriteID { get; set; } = 0;
        public uint CurrentAnimationID { get; set; } = 0;
        public uint CurrentFrame { get; set; } = 0;
        public Guid SpriteVramId { get; set; }
        public float CurrentFrameTime { get; set; } = 0.0f;
        public bool SpriteAlive { get; set; } = true;
        public ivec2 FlipSprite { get; set; } = new ivec2(0);
        public vec2 LastSpritePosition { get; set; } = new vec2(0.0f);
        public vec2 LastSpriteRotation { get; set; } = new vec2(0.0f);
        public vec2 LastSpriteScale { get; set; } = new vec2(1.0f);
        public vec2 SpritePosition { get; set; } = new vec2(0.0f);
        public vec2 SpriteRotation { get; set; } = new vec2(0.0f);
        public vec2 SpriteScale { get; set; } = new vec2(1.0f);

        public Sprite()
        {
        }
    };

}
