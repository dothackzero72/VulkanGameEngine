using Coral.Managed.Interop;
using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts;

namespace ClassLibrary1
{
    public unsafe class GameObject : IGameObject
    {
        public int id;
        public NativeString Name { get; protected set; }
        public List<GameObjectComponent> GameObjectComponentList { get; protected set; } = new List<GameObjectComponent>();


        public GameObject()
        {
            int a = 0;
        }

        public GameObject(int ID)
        {
            id = ID;
        }

        private GameObject(NativeString name)
        {
            Name = name;
        }

        private GameObject(NativeString name, List<GameObjectComponent> gameObjectComponentList)
        {
            Name = name;
            GameObjectComponentList = gameObjectComponentList;
        }

        public void Initialize(NativeString name)
        {
            Name = name;
        }

        public void Initialize(NativeString name, List<GameObjectComponent> componentTypeList)
        {
            Name = name;
            GameObjectComponentList = componentTypeList;
        }

        public void AddComponent(GameObjectComponent newComponent)
        {
            Console.WriteLine("AddComponent()");
            GameObjectComponentList.Add(newComponent);
        }

        public void AddComponent(GameObjectComponent* blightPtr, int className)
        {
            GameObjectComponentList.Add(*blightPtr);
        }

        public virtual void Input(InputKey key, KeyState keyState)
        {
        }

        public virtual void Update(float deltaTime)
        {
            foreach (GameObjectComponent component in GameObjectComponentList)
            {
                component.Update(deltaTime);
            }
        }

        public virtual void BufferUpdate(CommandBuffer commandBuffer, float deltaTime)
        {
            foreach (GameObjectComponent component in GameObjectComponentList)
            {
                component.BufferUpdate(commandBuffer, deltaTime);
            }
        }

        public virtual void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout pipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            foreach (GameObjectComponent component in GameObjectComponentList)
            {
                component.Draw(commandBuffer, pipeline, pipelineLayout, descriptorSet, sceneProperties);
            }
        }

        public virtual void Destroy()
        {
            foreach (GameObjectComponent component in GameObjectComponentList)
            {
                component.Destroy();
            }
        }

        public virtual int GetMemorySize()
        {
            return sizeof(GameObject);
        }

        public unsafe IntPtr GetGameObjectPtr()
        {
            return Marshal.UnsafeAddrOfPinnedArrayElement(GameObjectComponentList.ToArray(), 0);
        }

        public unsafe int* GetIDPtr()
        {
            fixed (int* transformPointer = &id)
            {
                return transformPointer;
            }
        }

        public IntPtr GetGameObjectComponent(int index)
        {
            GameObjectComponent component = GameObjectComponentList[index];
            GCHandle handle = GCHandle.Alloc(component); // Pin the component
            return (IntPtr)handle; // Return the handle as a pointer
        }

        public void ReleaseComponentHandle(IntPtr handle)
        {
            GCHandle gCHandle = (GCHandle)handle;
            if (gCHandle.IsAllocated)
            {
                gCHandle.Free();
            }
        }

        public int GetGameObjectComponentCount()
        {
            return GameObjectComponentList.Count;
        }
    }
}
