using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts;
using Coral.Managed.Interop;
using VulkanGameEngineGameObjectScripts.Import;
using VulkanGameEngineGameObjectScripts.Input;
using System.Windows.Forms;

namespace VulkanGameEngineGameObjectScripts.Component
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

        public Transform2DComponent(IntPtr cppComponentPtr, IntPtr cppGameObjectPtr, IntPtr csParentGameObject) : 
            base(cppComponentPtr, cppGameObjectPtr, csParentGameObject, ComponentTypeEnum.kGameObjectTransform2DComponent)
        {
            Name = "GameObjectTransform2DComponent";

            GameObjectTransform = mat4.Identity;
            GameObjectPosition = new vec2(0.0f, 0.0f);
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public Transform2DComponent(IntPtr cppComponentPtr, IntPtr cppGameObjectPtr, IntPtr csParentGameObject, NativeString name) : 
            base(cppComponentPtr, cppGameObjectPtr, csParentGameObject, name, ComponentTypeEnum.kGameObjectTransform2DComponent) 
        {
            GameObjectTransform = mat4.Identity;
            GameObjectPosition = new vec2(0.0f, 0.0f);
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public override void Input(KeyBoardKeys key, float deltaTime)
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
