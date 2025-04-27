using GlmSharp;
using System.Collections.Generic;

namespace VulkanGameEngineLevelEditor.Models
{
    public class Animation2DModel
    {
        public string AnimationName { get; set; }
        public float FrameHoldTime { get; set; }
        public List<ivec2> FrameList { get; set; }
    }
}
