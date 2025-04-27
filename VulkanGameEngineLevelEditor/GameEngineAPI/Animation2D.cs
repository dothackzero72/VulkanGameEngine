using GlmSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class Animation2D
    {
        public string AnimationName { get; set; }
        [JsonIgnore]
        public uint CurrentFrame { get; set; }
        [JsonIgnore]
        public vec2 CurrentFrameUV { get; set; }
        [JsonIgnore]
        public float CurrentFrameTime { get; set; }
        public float FrameHoldTime { get; set; }
        public List<ivec2> FrameList { get; set; }

        public Animation2D()
        {

        }

        public Animation2D(Animation2DModel model)
        {
            AnimationName = model.AnimationName;
            FrameList = model.FrameList;
            FrameHoldTime = model.FrameHoldTime;
            CurrentFrameUV = new vec2(0.0f);
            CurrentFrameTime = 0.0f;
            CurrentFrame = 0;
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
