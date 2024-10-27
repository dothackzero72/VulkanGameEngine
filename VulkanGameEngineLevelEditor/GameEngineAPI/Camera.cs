using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public abstract class Camera
    {
        public float Width { get; protected set; }
        public float Height { get; protected set; }
        public float AspectRatio { get; protected set; }
        public Vector3 Position { get; protected set; }
        public float Zoom { get; protected set; }
        public Vector2 ViewScreenSize { get; protected set; }
        public Matrix4x4 ProjectionMatrix { get; protected set; }
        public Matrix4x4 ViewMatrix { get; protected set; }

        public Camera()
        { 

        }

        public abstract void Update(SceneDataBuffer sceneProperties);
        public abstract void UpdateKeyboard(float deltaTime);
        public abstract void UpdateMouse();
    }
}
