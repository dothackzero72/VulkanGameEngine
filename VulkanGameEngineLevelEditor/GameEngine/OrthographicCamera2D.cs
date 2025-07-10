using GlmSharp;
using System;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class OrthographicCamera2D : Camera
    {
        public OrthographicCamera2D()
        {

        }

        public OrthographicCamera2D(float width, float height)
        {
            Position = new vec3(0.0f);
            ViewScreenSize = new vec2(width, height);
            ProjectionMatrix = mat4.Ortho(0.0f, Width, Height, 0.0f);
            ViewMatrix = mat4.Identity;
        }

        public OrthographicCamera2D(vec2 viewScreenSize)
        {
            Width = viewScreenSize.x;
            Height = viewScreenSize.y;
            AspectRatio = viewScreenSize.x / viewScreenSize.y;
            Zoom = 1.0f;

            Position = new vec3(0.0f);
            ViewScreenSize = viewScreenSize;
            ProjectionMatrix = mat4.Ortho(0.0f, Width, Height, 0.0f);
            ViewMatrix = mat4.Identity;
        }

        public OrthographicCamera2D(vec2 viewScreenSize, vec2 position)
        {
            Width = viewScreenSize.x;
            Height = viewScreenSize.y;
            AspectRatio = viewScreenSize.x / viewScreenSize.y;
            Zoom = 1.0f;

            Position = new vec3(position, 0.0f);
            ViewScreenSize = viewScreenSize;
            ProjectionMatrix = mat4.Ortho(0.0f, Width, Height, 0.0f);
            ViewMatrix = mat4.Identity;
        }

        public override SceneDataBuffer Update( SceneDataBuffer sceneProperties)
        {
            ProjectionMatrix = mat4.Ortho(0.0f, Width, Height, 0.0f);

            sceneProperties.CameraPosition = new vec3(Position.x, Position.y, Position.z);
            sceneProperties.View = ViewMatrix;
            sceneProperties.Projection = ProjectionMatrix;

            return sceneProperties;
        }

        public override void UpdateKeyboard(float deltaTime)
        {
            throw new NotImplementedException();
        }

        public override void UpdateMouse()
        {
            throw new NotImplementedException();
        }
    }
}