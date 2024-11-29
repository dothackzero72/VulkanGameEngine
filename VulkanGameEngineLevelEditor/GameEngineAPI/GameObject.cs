using Microsoft.CodeAnalysis.CSharp.Syntax;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.Components;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class GameObject
    {
        public String Name { get; protected set; }
        public List<GameObjectComponent> GameObjectComponentList { get; protected set; } = new List<GameObjectComponent>();
        public GameObject() { }
        private GameObject(String name)
        {
            Name = name;
        }

        private GameObject(String name, List<GameObjectComponent> gameObjectComponentList)
        {
            Name = name;
            GameObjectComponentList = gameObjectComponentList;
        }
        public static GameObject CreateGameObject(string name)
        {
            GameObject gameObject = MemoryManager.AllocateGameObject();
            gameObject.Initialize(name);
            return gameObject;
        }

        public static GameObject CreateGameObject(string name, List<ComponentTypeEnum> componentTypeList)
        {
            GameObject gameObject = MemoryManager.AllocateGameObject();
            gameObject.Initialize(name);

            GCHandle handle = GCHandle.Alloc(gameObject, GCHandleType.Normal);
            IntPtr parentGameObjectPtr = GCHandle.ToIntPtr(handle);

            foreach (var component in componentTypeList)
            {
                String asdf = "adsfasd";
                switch (component)
                {
                    case ComponentTypeEnum.kGameObjectTransform2DComponent: gameObject.AddComponent(new GameObjectTransform2D(parentGameObjectPtr, "Testing")); break;
                    case ComponentTypeEnum.kRenderMesh2DComponent: gameObject.AddComponent(MeshRenderer2DComponent.CreateRenderMesh2DComponent(parentGameObjectPtr, "Mesh Renderer", (uint)MemoryManager.RenderMesh2DComponentList.Count)); break;
                    case ComponentTypeEnum.kTestScriptComponent: gameObject.AddComponent(new TestScriptComponent(parentGameObjectPtr, "Testing")); break;
                }
            }
            return gameObject;
        }

        private void Initialize(String name)
        {
            Name = name;
        }

        private void Initialize(String name, List<GameObjectComponent> componentTypeList)
        {
            Name = name;
            GameObjectComponentList = componentTypeList;
        }

        public virtual void Update(long startTime)
        {
            foreach (IGameObjectComponent component in GameObjectComponentList)
            {
                component.Update(startTime);
            }
        }

        public virtual void BufferUpdate(CommandBuffer commandBuffer, long startTime)
        {
            foreach (GameObjectComponent component in GameObjectComponentList)
            {
                component.BufferUpdate(commandBuffer, startTime);
            }
        }

        public virtual void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            foreach (GameObjectComponent component in GameObjectComponentList)
            {
                component.Draw(commandBuffer, pipeline, shaderPipelineLayout, descriptorSet, sceneProperties);
            }
        }

        public virtual void Destroy()
        {
            foreach (GameObjectComponent component in GameObjectComponentList)
            {
                component.Destroy();
            }
        }

        public void AddComponent(GameObjectComponent newComponent)
        {
            GameObjectComponentList.Add(newComponent);
        }

        public void RemoveComponent(GameObjectComponent gameObjectComponent)
        {
            GameObjectComponentList.Remove(gameObjectComponent);
        }

        public GameObjectComponent GetComponentByName(String name)
        {
            return GameObjectComponentList.Where(x => x.Name == name).First();
        }

        public List<GameObjectComponent> GetComponentByComponentType(ComponentTypeEnum type)
        {
            return GameObjectComponentList.Where(x => x.ComponentType == type).ToList();
        }
    }
}