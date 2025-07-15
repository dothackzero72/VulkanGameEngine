using GlmSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [IgnoreProperty]
        public uint GameObjectId { get; set; }
        [ReadOnly(true)]
        public uint SpriteID { get; set; } = 0;
        [IgnoreProperty]
        public uint CurrentAnimationID { get; set; } = 0;
        [IgnoreProperty]
        public uint CurrentFrame { get; set; } = 0;
        public Guid SpriteVramId { get; set; }
        [IgnoreProperty]
        public float CurrentFrameTime { get; set; } = 0.0f;
        [IgnoreProperty]
        public bool SpriteAlive { get; set; } = true;
        [IgnoreProperty]
        public ivec2 FlipSprite { get; set; } = new ivec2(0);
        [IgnoreProperty]
        public vec2 LastSpritePosition { get; set; } = new vec2(0.0f);
        [IgnoreProperty]
        public vec2 LastSpriteRotation { get; set; } = new vec2(0.0f);
        [IgnoreProperty]
        public vec2 LastSpriteScale { get; set; } = new vec2(1.0f);
        public vec2 SpritePosition { get; set; } = new vec2(0.0f);
        public vec2 SpriteRotation { get; set; } = new vec2(0.0f);
        public vec2 SpriteScale { get; set; } = new vec2(1.0f);

        public Sprite()
        {
        }
    };

}
