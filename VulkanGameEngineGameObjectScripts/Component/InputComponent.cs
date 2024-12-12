using Coral.Managed.Interop;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VulkanGameEngineGameObjectScripts.Input;
using VulkanGameEngineGameObjectScripts.Interface;

namespace VulkanGameEngineGameObjectScripts.Component
{
    public unsafe class InputComponent : GameObjectComponent
    {
        Transform2DComponent transform;
  
        public InputComponent() : base()
        {
            Name = "InputComponent";
            ComponentType = ComponentTypeEnum.kInputComponent;
        }

        public InputComponent(IntPtr cppComponentPtr, IntPtr cppGameObjectPtr, IntPtr csParentGameObject) : 
            base(cppComponentPtr, cppGameObjectPtr, csParentGameObject, ComponentTypeEnum.kInputComponent)
        {
            Name = "InputComponent";
            transform = ParentGameObject.GameObjectComponentList.Where(x => x.ComponentType == ComponentTypeEnum.kGameObjectTransform2DComponent).First() as Transform2DComponent;
        }

        public InputComponent(IntPtr cppComponentPtr, IntPtr cppGameObjectPtr, IntPtr csParentGameObject, NativeString name) : 
            base(cppComponentPtr, cppGameObjectPtr, csParentGameObject, name, ComponentTypeEnum.kInputComponent)
        {
            transform = ParentGameObject.GameObjectComponentList.Where(x => x.ComponentType == ComponentTypeEnum.kGameObjectTransform2DComponent).First() as Transform2DComponent;
        }

        public override void BufferUpdate(IntPtr commandBuffer, float deltaTime)
        {

        }

        public override void Destroy()
        {

        }

        public override void Draw(nint commandBuffer, ulong pipeline, ulong pipelineLayout, ulong descriptorSet, SceneDataBuffer sceneProperties)
        {

        }

        public override int GetMemorySize()
        {
            return sizeof(InputComponent);
        }

        public override void Input(KeyBoardKeys key, float deltaTime)
        {
            if (key == KeyBoardKeys.W)
            {
                transform.GameObjectPosition.y += 0.01f;
            }
            if (key == KeyBoardKeys.A)
            {
                transform.GameObjectPosition.x -= 0.01f;
            }
            if (key == KeyBoardKeys.S)
            {
                transform.GameObjectPosition.y -= 0.01f;
            }
            if (key == KeyBoardKeys.D)
            {
                transform.GameObjectPosition.x += 0.01f;
            }
        }

        public override void Update(float deltaTime)
        {
      
        }
    }
}
