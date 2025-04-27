using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using VulkanGameEngineLevelEditor.Components;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class GameObject
    {
        [JsonIgnore]
        public static uint NextGameObjectId { get; private set; } = 0;
        [JsonIgnore]
        public uint GameObjectId { get; private set; } = 0;
        [JsonIgnore]
        public size_t ObjectComponentMemorySize { get; private set; } = 0;
        [JsonIgnore]
        public bool GameObjectAlive { get; private set; } = true;
        public string Name { get; protected set; }
        public List<GameObjectComponent> GameObjectComponentList { get; protected set; } = new List<GameObjectComponent>();

        public GameObject()
        {
        }

        public GameObject(string name)
        {
            Name = name;
        }

        public GameObject(string name, List<ComponentTypeEnum> gameObjectComponentList)
        {
            GameObjectId = ++NextGameObjectId;
            Name = name;
        }

        public GameObject(string name, List<ComponentTypeEnum> gameObjectComponentList, SpriteSheet spriteSheet)
        {
            GameObjectId = ++NextGameObjectId;
            Name = name;
        }

        public GameObject(GameObjectModel model)
        {
            GameObjectId = ++NextGameObjectId;

            Name = model.Name;
            foreach (GameObjectComponentModel component in model.GameObjectComponentList)
            {
                switch (component.ComponentType)
                {
                    case ComponentTypeEnum.kTransform2DComponent: AddComponent(new Transform2DComponent(this, component)); break;
                    case ComponentTypeEnum.kSpriteComponent: AddComponent(new SpriteComponent(this, component)); break;
                }
            }
        }

        static public List<GameObjectComponent> GetComponentFromGameObjects(List<GameObject> gameObjectList, ComponentTypeEnum componentType)
        {
            return gameObjectList
                .Where(x => x.GameObjectComponentList.Any(y => y.ComponentType == componentType))
                .SelectMany(x => x.GameObjectComponentList
                    .Where(y => y.ComponentType == componentType))
                .ToList();
        }

        public void AddComponent(GameObjectComponent newComponent)
        {

            GameObjectComponentList.Add(newComponent);
        }

        public virtual void Input(KeyBoardKeys key, float deltaTime)
        {
        }

        public virtual void Update(VkCommandBuffer commandBuffer, float deltaTime)
        {
            foreach (GameObjectComponent component in GameObjectComponentList)
            {
                component.Update(commandBuffer, deltaTime);
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

        public IntPtr GetGameObjectComponent(int index)
        {
            GameObjectComponent component = GameObjectComponentList[index];
            GCHandle handle = GCHandle.Alloc(component);
            return (IntPtr)handle;
        }

        public void RemoveComponent(GameObjectComponent component)
        {
            GameObjectComponentList.Remove(component);
        }

        public int GetGameObjectComponentCount()
        {
            return GameObjectComponentList.Count;
        }

        public GameObjectComponent GetComponentByName(string name)
        {
            return GameObjectComponentList.Where(x => x.Name == name).First();
        }

        public GameObjectComponent GetComponentByComponentType(ComponentTypeEnum type)
        {
            return GameObjectComponentList.Where(x => x.ComponentType == type).First();
        }
    }
}
