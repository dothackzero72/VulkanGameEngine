using System.Collections.Generic;
using VulkanGameEngineLevelEditor.GameEngine.GameObjectComponents;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class GameObject
    {
        public string Name { get; set; }
        public int GameObjectId { get; set; }
        public List<ComponentTypeEnum> GameObjectComponentTypeList { get; set; } = new List<ComponentTypeEnum>();
        public Dictionary<ComponentTypeEnum, object> Components { get; set; } = new Dictionary<ComponentTypeEnum, object>();

        public GameObject()
        {
        }

        public GameObject(string name, int gameObjectId)
        {
            Name = name;
            GameObjectId = gameObjectId;
        }

        public void AddComponent(ComponentTypeEnum type, object component)
        {
            GameObjectComponentTypeList.Add(type);
            Components[type] = component;
        }
    }
}