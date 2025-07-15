using GlmSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using VulkanGameEngineLevelEditor.GameEngine.GameObjectComponents;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class GameObject
    {
        
        public string Name { get; set; }
        [ReadOnly(true)]
        public int GameObjectId { get; set; }
        public List<ComponentTypeEnum> GameObjectComponentTypeList { get; set; } = new List<ComponentTypeEnum>();

        public GameObject()
        {
        }

        public GameObject(string name, int gameObjectId)
        {
            Name = name;
            GameObjectId = gameObjectId;
        }
    }
}