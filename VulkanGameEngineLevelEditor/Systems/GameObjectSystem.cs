using GlmSharp;
using Newtonsoft.Json;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Systems
{
    public class GameObject
    {
        public int GameObjectId { get; set; }

        public GameObject()
        {

        }

        public GameObject(int gameObjectId)
        {
            GameObjectId = gameObjectId;
        }
    };

    public class GameObjectComponentLoader
    {
        public vec2 GameObjectPosition { get; set; }
        public vec2 GameObjectRotation { get; set; }
        public vec2 GameObjectScale { get; set; }
        public int ComponentType { get; set; }
        public Guid VramId { get; set; }
    }

    public class LoadGameObjectComponents
    {
        public List<GameObjectComponentLoader> GameObjectComponentList { get; set; }
    }

    public static class GameObjectSystem
    {
        public static Dictionary<int, GameObject> GameObjectMap { get; private set; } = new Dictionary<int, GameObject>();
        public static Dictionary<int, Transform2DComponent> Transform2DComponentMap { get; private set; } = new Dictionary<int, Transform2DComponent>();
        public static Dictionary<int, InputComponent> InputComponentMap { get; private set; } = new Dictionary<int, InputComponent>();

        public static void CreateGameObject(string name, List<ComponentTypeEnum> gameObjectComponentTypeList, Guid vramId, vec2 objectPosition)
        {
            int id = GameObjectMap.Count() + 1;
            GameObjectMap[id] = new GameObject(id);

            foreach (var component in gameObjectComponentTypeList)
            {
                switch (component)
                {
                    case  ComponentTypeEnum.kTransform2DComponent: Transform2DComponentMap[id] = new Transform2DComponent(objectPosition); break;
                    case ComponentTypeEnum.kInputComponent: InputComponentMap[id] = new InputComponent(id); break;
                    case ComponentTypeEnum.kSpriteComponent: SpriteSystem.AddSprite((uint)id, vramId); break;
                }
            }
        }

        public static void CreateGameObject(string gameObjectPath)
        {
            int id = GameObjectMap.Count() + 1;
            GameObjectMap[id] = new GameObject(id);

            string jsonContent = File.ReadAllText(gameObjectPath);
            LoadGameObjectComponents spriteVramJson = JsonConvert.DeserializeObject<LoadGameObjectComponents>(jsonContent);
            foreach(var component in spriteVramJson.GameObjectComponentList)
            {
                int componentType = component.ComponentType;
                switch ((ComponentTypeEnum)componentType)
                {
                    case ComponentTypeEnum.kTransform2DComponent: LoadTransformComponent(component, id, component.GameObjectPosition); break;
                    case ComponentTypeEnum.kInputComponent: LoadInputComponent(component, id); break;
                    case ComponentTypeEnum.kSpriteComponent: LoadSpriteComponent(component, (uint)id); break;
                }
            }
        }

        public static void LoadTransformComponent(GameObjectComponentLoader loader, int gameObjectId, vec2 gameObjectPosition)
        {
            Transform2DComponent transform2D = new Transform2DComponent();
            transform2D.GameObjectPosition = gameObjectPosition;
            transform2D.GameObjectRotation = loader.GameObjectRotation;
            transform2D.GameObjectScale = loader.GameObjectScale;
            Transform2DComponentMap[gameObjectId] = transform2D;
        }

        public static void LoadInputComponent(GameObjectComponentLoader loader, int gameObjectId)
        {
            InputComponentMap[gameObjectId] = new InputComponent();
        }

        public static void LoadSpriteComponent(GameObjectComponentLoader loader, uint gameObjectId)
        {
            SpriteSystem.AddSprite(gameObjectId, loader.VramId);
        }

        public static void DestroyGameObject(int gameObjectId)
        {
            //	GameObjectMap.erase(gameObjectId);
          //  Transform2DComponentMap.erase(gameObjectId);
           // InputComponentMap.erase(gameObjectId);
        }

        public static void DestroyGameObjects()
        {
            //for (auto & gameObject : GameObjectMap)
            //{
            //    DestroyGameObject(gameObject.second.GameObjectId);
            //}
        }
    }
}
