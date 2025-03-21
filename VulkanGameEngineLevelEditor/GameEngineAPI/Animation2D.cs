using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class Animation2D
    {
        public uint CurrentFrame { get; set; }
        public vec2 CurrentFrameUV { get; set; }
        public float CurrentFrameTime { get; set; }
        public float FrameHoldTime { get; set; }
        public List<ivec2> FrameList { get; set; }
        public string AnimationName { get; set; }

        public Animation2D()
        {

        }

        public Animation2D(string animationName, List<ivec2> frameList, float frameHoldTime)
        {
            AnimationName = animationName;
            FrameList = frameList;
            FrameHoldTime = frameHoldTime;
            CurrentFrameUV = new vec2(0.0f);
            CurrentFrameTime = 0.0f;
            CurrentFrame = 0;
        }
    }
}
