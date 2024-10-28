using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

            List<GameObjectComponent> componentList = new List<GameObjectComponent>();
            // Example of creating a specific type of component
            //componentList.Add(RenderMesh2DComponent.CreateRenderMesh2DComponent("Mesh Renderer",
            //    (uint)MemoryManager.RenderMesh2DComponentList.Count));

            //gameObject.Initialize(name, componentList);
            return gameObject;
        }

        private void Initialize(String name)
        {
            Name = name;
        }

        private void Initialize(String name, List<ComponentTypeEnum> componentTypeList)
        {
            Name = name;
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
                component.Update(commandBuffer, deltaTime);
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