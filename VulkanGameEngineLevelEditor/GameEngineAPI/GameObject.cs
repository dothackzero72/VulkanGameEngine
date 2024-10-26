using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class GameObject
    {
        public String Name { get; protected set; }
        public List<GameObjectComponent> GameObjectComponentList { get; protected set; }
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

        //public static GameObject CreateGameObject(string name, List<ComponentTypeEnum> componentTypeList)
        //{
        //    GameObject gameObject = pool.AllocateMemoryLocation();

        //    List<GameObjectComponent> componentList = new List<GameObjectComponent>();
        //    // Example of creating a specific type of component
        //    componentList.Add(RenderMesh2DComponent.CreateRenderMesh2DComponent("Mesh Renderer",
        //        (uint)MemoryManager.RenderMesh2DComponentList.Count));

        //    gameObject.Initialize(name, componentList);
        //    return gameObject;
        //}

        private void Initialize(String name)
        {
            Name = name;
        }

        //private void Initialize(String name, List<ComponentTypeEnum> componentTypeList)
        //{
        //    Name = name;
        //}
    }
}
