using Coral.Managed.Interop;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineGameObjectScripts
{
    public unsafe class GameObject : IGameObject
    {
        public NativeString Name { get; protected set; }
        public List<GameObjectComponent> GameObjectComponentList { get; protected set; } = new List<GameObjectComponent>();

        public GameObject()
        {
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

        public virtual void BufferUpdate(VkCommandBuffer commandBuffer, float deltaTime)
        {
            foreach (GameObjectComponent component in GameObjectComponentList)
            {
                component.BufferUpdate(commandBuffer, deltaTime);
            }
        }

        public virtual void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout pipelineLayout, VkDescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
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

        public IntPtr GetGameObjectComponent(int index)
        {
            GameObjectComponent component = GameObjectComponentList[index];
            GCHandle handle = GCHandle.Alloc(component);
            return (IntPtr)handle;
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
