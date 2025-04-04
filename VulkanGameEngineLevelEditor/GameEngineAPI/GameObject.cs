using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts.Input;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.Components;
using Newtonsoft.Json;
using VulkanGameEngineLevelEditor.Models;

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
        //    GameObjectComponentList = gameObjectComponentList;
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
         //   GameObjectComponentList = model.GameObjectComponentList;
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

        public void AddComponent(GameObjectComponent* gameObjectComponentPtr, IntPtr componentPtr)
        {
            //GameObjectComponent gameObjectComponent = *gameObjectComponentPtr;
            //gameObjectComponent.CPPcomponentPtr = componentPtr;

            GameObjectComponentList.Add(*gameObjectComponentPtr);
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

        public virtual void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout pipelineLayout, ListPtr<VkDescriptorSet> descriptorSetList, SceneDataBuffer sceneProperties)
        {
            foreach (GameObjectComponent component in GameObjectComponentList)
            {
                component.Draw(commandBuffer, pipeline, pipelineLayout, descriptorSetList, sceneProperties);
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
