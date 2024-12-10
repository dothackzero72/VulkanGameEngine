using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts;
using Coral.Managed.Interop;
using VulkanGameEngineGameObjectScripts.Import;

namespace VulkanGameEngineGameObjectScripts
{
    public unsafe class Transform2DComponent : GameObjectComponent
    {
        public mat4 GameObjectTransform;
        public vec2 GameObjectPosition;
        public vec2 GameObjectRotation;
        public vec2 GameObjectScale;

        public Transform2DComponent() : base()
        {
            Name = "GameObjectTransform2DComponent";

            GameObjectTransform = mat4.Identity;
            GameObjectPosition = new vec2(0.0f, 0.0f);
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public Transform2DComponent(IntPtr parentGameObject) : base(parentGameObject, ComponentTypeEnum.kGameObjectTransform2DComponent)
        {
            Name = "GameObjectTransform2DComponent";

            GameObjectTransform = mat4.Identity;
            GameObjectPosition = new vec2(0.0f, 0.0f);
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public Transform2DComponent(IntPtr parentGameObject, NativeString name) : base(parentGameObject, name, ComponentTypeEnum.kGameObjectTransform2DComponent)
        {
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

        public override void BufferUpdate(IntPtr commandBuffer, float deltaTime)
        {
            GameObjectTransform = mat4.Identity;
            GameObjectTransform = mat4.Scale(new vec3(GameObjectScale, 0.0f));
            GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.x), new vec3(1.0f, 0.0f, 0.0f));
            GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.y), new vec3(0.0f, 1.0f, 0.0f));
            GameObjectTransform = mat4.Translate(new vec3(GameObjectPosition, 0.0f));
        }

        public override void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout pipelineLayout, VkDescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            Console.WriteLine("Draw called");
        }

        public override void Destroy()
        {
            Console.WriteLine("Destroy called");
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
