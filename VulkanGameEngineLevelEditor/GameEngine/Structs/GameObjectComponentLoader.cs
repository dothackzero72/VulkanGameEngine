using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
    public class GameObjectComponentLoader
    {
        public vec2 GameObjectPosition { get; set; }
        public vec2 GameObjectRotation { get; set; }
        public vec2 GameObjectScale { get; set; }
        public int ComponentType { get; set; }
        public Guid VramId { get; set; }
    }
}
