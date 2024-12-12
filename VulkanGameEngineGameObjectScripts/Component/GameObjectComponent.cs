using Coral.Managed.Interop;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using VulkanGameEngineGameObjectScripts.Input;
using VulkanGameEngineGameObjectScripts.Interface;

namespace VulkanGameEngineGameObjectScripts.Component
{
    public unsafe class GameObjectComponent : IGameObjectComponent
    {
        public IntPtr CPPgameObjectPtr;
        public IntPtr CPPcomponentPtr;

        public GameObject ParentGameObject;
        public ComponentTypeEnum ComponentType { get; set; }
        public NativeString Name { get; set; } = new NativeString();

        public GameObjectComponent()
        {

        }

        public GameObjectComponent(IntPtr cppComponentPtr, IntPtr cppGameObjectPtr, IntPtr csParentGameObject, ComponentTypeEnum componentType)
        {
            CPPcomponentPtr = cppComponentPtr;
            CPPgameObjectPtr = cppGameObjectPtr;

            GCHandle handle = GCHandle.FromIntPtr(csParentGameObject);
            ParentGameObject = (GameObject)handle.Target;

            Name = "GameObjectComponent";
            ComponentType = componentType;
        }

        public GameObjectComponent(IntPtr cppComponentPtr, IntPtr cppGameObjectPtr, IntPtr csParentGameObject, NativeString nString, ComponentTypeEnum componentType)
        {
            CPPcomponentPtr = cppComponentPtr;
            CPPgameObjectPtr = cppGameObjectPtr;

            GCHandle handle = GCHandle.FromIntPtr(csParentGameObject);
            ParentGameObject = (GameObject)handle.Target;

            Name = nString;
            ComponentType = componentType;
        }

        public virtual void Input(KeyBoardKeys key, float deltaTime)
        {
        }

        public virtual void Update(float deltaTime)
        {
            Console.WriteLine("GameObjectComponent Updated");
        }

        public virtual void BufferUpdate(VkCommandBuffer commandBuffer, float deltaTime)
        {

        }

        public virtual void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout pipelineLayout, VkDescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {

        }

        public virtual void Destroy()
        {

        }

        public virtual int GetMemorySize()
        {
            return sizeof(GameObjectComponent);
        }

        public IntPtr GetCPPgameObjectPtr()
        {
            return CPPgameObjectPtr;
        }

        public IntPtr GetCPPComponentPtr()
        {
            return CPPcomponentPtr;
        }

        //public NativeString* GetPositionPtr()
        //{
        //    fixed (NativeString* positionPointer = &Name)
        //    {
        //        return positionPointer;
        //    }
        //}

        //public unsafe ComponentTypeEnum* GetComponentTypePtr()
        //{
        //    // Get the address of the field: `ComponentType`
        //    fixed (ComponentTypeEnum* ptr = &ComponentType)
        //    {
        //        return ptr;
        //    }
        //}
    }
}
