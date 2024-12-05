using GlmSharp;
using Silk.NET.Vulkan;
using System.Runtime.InteropServices;
using VulkanGameEngineGameObjectScripts.CLI;

namespace VulkanGameEngineGameObjectScripts
{
    public unsafe class Transform2DComponent : GameObjectComponent
    {
        private GameObject ParentGameObject { get; set; }

        public mat4 GameObjectTransform;
        public vec2 GameObjectPosition;
        public vec2 GameObjectRotation;
        public vec2 GameObjectScale;

        public Transform2DComponent()
        {
            ParentGameObjectPtr = IntPtr.Zero;
            Name = "GameObjectTransform2DComponent";

            GameObjectTransform = mat4.Identity;
            GameObjectPosition = new vec2(0.0f, 0.0f);
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public Transform2DComponent(IntPtr parentGameObject)
        {
            ParentGameObjectPtr = parentGameObject;

            GCHandle handle = GCHandle.FromIntPtr(ParentGameObjectPtr);
            ParentGameObject = handle.Target as GameObject;

            Name = "GameObjectTransform2DComponent";

            GameObjectTransform = mat4.Identity;
            GameObjectPosition = new vec2(0.0f, 0.0f);
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public Transform2DComponent(IntPtr parentGameObject, String name)
        {
            ParentGameObjectPtr = parentGameObject;

            GCHandle handle = GCHandle.FromIntPtr(ParentGameObjectPtr);
            ParentGameObject = handle.Target as GameObject;

            Name = name;

            GameObjectTransform = mat4.Identity;
            GameObjectPosition = new vec2(0.0f, 0.0f);
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public override void Input(InputKey key, KeyState keyState)
        {

        }

        public override void Update(float deltaTime)
        {
            GameObjectTransform = mat4.Identity;
            GameObjectTransform = mat4.Scale(new vec3(GameObjectScale, 0.0f));
            GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.x), new vec3(1.0f, 0.0f, 0.0f));
            GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.y), new vec3(0.0f, 1.0f, 0.0f));
            GameObjectTransform = mat4.Translate(new vec3(GameObjectPosition, 0.0f));
        }

        public override void BufferUpdate(CommandBuffer commandBuffer, float deltaTime)
        {

        }

        public override void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
 
        }

        public override void Destroy()
        {
       
        }

        public override int GetMemorySize()
        {
            return (int)sizeof(Transform2DComponent);
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
