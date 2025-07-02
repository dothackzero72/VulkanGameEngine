using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public struct Transform2DComponent
    {
        public vec2 GameObjectPosition { get; set; } = new vec2(0.0f);
        public vec2 GameObjectRotation { get; set; } = new vec2(0.0f);
        public vec2 GameObjectScale { get; set; } = new vec2(1.0f);

        public Transform2DComponent()
        {

        }

        public Transform2DComponent(vec2 gameObjectPosition)
        {
            GameObjectPosition = gameObjectPosition;
        }

        public Transform2DComponent(vec2 gameObjectPosition, vec2 gameObjectRotation)
        {
            GameObjectPosition = gameObjectPosition;
            GameObjectRotation = gameObjectRotation;
        }

        public Transform2DComponent(vec2 gameObjectPosition, vec2 gameObjectRotation, vec2 gameObjectScale)
        {
            GameObjectPosition = gameObjectPosition;
            GameObjectRotation = gameObjectRotation;
            GameObjectScale = GameObjectScale;
        }
    }
}
