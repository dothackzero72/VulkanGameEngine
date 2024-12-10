using Coral.Managed.Interop;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts;

namespace ClassLibrary1
{
    public unsafe class GameObjectComponent : IGameObjectComponent
    {
        public GameObject ParentGameObject;
        public ComponentTypeEnum ComponentType { get; set; }
        public NativeString Name { get; set; } = new NativeString();

        public GameObjectComponent()
        {

        }

        public GameObjectComponent(IntPtr parentGameObject)
        {
            GCHandle handle = GCHandle.FromIntPtr(parentGameObject);
            ParentGameObject = (GameObject)handle.Target;
        }

        public GameObjectComponent(IntPtr parentGameObject, NativeString nString)
        {
            GCHandle handle = GCHandle.FromIntPtr(parentGameObject);
            ParentGameObject = (GameObject)handle.Target;
            Name = nString;
        }

        public virtual void Input(InputKey key, KeyState keyState)
        {
        }

        public virtual void Update(float deltaTime)
        {
            Console.WriteLine("GameObjectComponent Updated");
        }

        public virtual void BufferUpdate(IntPtr commandBuffer, float deltaTime)
        {

        }

        public virtual void Draw(IntPtr commandBuffer, IntPtr pipeline, IntPtr pipelineLayout, IntPtr descriptorSet, SceneDataBuffer sceneProperties)
        {

        }

        public virtual void Destroy()
        {

        }

        public virtual int GetMemorySize()
        {
            return sizeof(GameObjectComponent);
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
