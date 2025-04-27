using GlmSharp;

namespace VulkanGameEngineLevelEditor.Models
{
    public class Transform2DComponentModel : GameObjectComponentModel
    {
        public vec2 GameObjectPosition { get; set; }
        public vec2 GameObjectRotation { get; set; }
        public vec2 GameObjectScale { get; set; }
    }
}
