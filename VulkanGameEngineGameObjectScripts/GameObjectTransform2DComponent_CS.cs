using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts.CLI;

namespace VulkanGameEngineGameObjectScripts
{
    public unsafe class Transform2DComponent_CS : IGameObjectComponent
    {
        public mat4 GameObjectTransform;
        public vec2 GameObjectPosition;
        public vec2 GameObjectRotation;
        public vec2 GameObjectScale;
        public nint ParentGameObject;
        public string Name;
        public ulong MemorySize;
        float offset;

        public ComponentTypeEnum ComponentType { get; set; } = ComponentTypeEnum.kGameObjectTransform2DComponent;

        public Transform2DComponent_CS()
        {
            ParentGameObject = IntPtr.Zero;
            Name = "GameObjectTransform2DComponent";
            GameObjectPosition = new vec2(0.0f, 0.0f);
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public Transform2DComponent_CS(IntPtr parentGameObject)
        {
            ParentGameObject = parentGameObject;
            Name = "GameObjectTransform2DComponent";
            GameObjectPosition = new vec2(0.0f, 0.0f);
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public Transform2DComponent_CS(IntPtr parentGameObject, String name)
        {
            ParentGameObject = parentGameObject;
            Name = name;
            GameObjectPosition = new vec2(0.0f, 00.0f);
        }

        public void BufferUpdate(nint commandBuffer, float deltaTime)
        {
        }

        public void Destroy()
        {

        }

        public void Draw(nint commandBuffer, nint pipeline, nint shaderPipelineLayout, nint descriptorSet, SceneDataBuffer sceneProperties)
        {

        }

        public int GetMemorySize()
        {
            return (int)sizeof(Transform2DComponent_CS);
        }

        public void Update(float deltaTime)
        {
            GameObjectTransform = mat4.Identity;
            GameObjectTransform = mat4.Scale(new vec3(GameObjectScale, 0.0f));
            GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.x), new vec3(1.0f, 0.0f, 0.0f));
            GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.y), new vec3(0.0f, 1.0f, 0.0f));
            GameObjectTransform = mat4.Translate(new vec3(GameObjectPosition, 0.0f));
        }

        public vec2* GetPositionPtr()
        {
            fixed (vec2* positionPointer = &GameObjectPosition)
            {
                return positionPointer;
            }
        }

        public vec2* GetRotationPtr()
        {
            fixed (vec2* rotationPointer = &GameObjectRotation)
            {
                return rotationPointer;
            }
        }

        public vec2* GetScalePtr()
        {
            fixed (vec2* scalePointer = &GameObjectScale)
            {
                return scalePointer;
            }
        }

        public mat4* GetTransformMatrixPtr()
        {
            fixed (mat4* transformPointer = &GameObjectTransform)
            {
                return transformPointer;
            }
        }
    }
}
