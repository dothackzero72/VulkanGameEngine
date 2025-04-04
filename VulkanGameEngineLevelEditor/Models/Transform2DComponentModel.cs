using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public class Transform2DComponentModel : GameObjectComponentModel
    {
        public vec2 GameObjectPosition { get; private set; }
        public vec2 GameObjectRotation { get; private set; }
        public vec2 GameObjectScale { get; private set; }
    }
}
