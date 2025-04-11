using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;


namespace VulkanGameEngineLevelEditor
{
    public class GameObjectComponentConverter : JsonConverter<List<GameObjectComponentModel>>
    {

        public override bool CanWrite => false;

        public override List<GameObjectComponentModel> ReadJson(JsonReader reader, Type objectType, List<GameObjectComponentModel> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            var list = new List<GameObjectComponentModel>();
            foreach (var obj in array)
            {
                var componentType = obj["ComponentType"]?.Value<int>() ?? 0;
                GameObjectComponentModel component = componentType switch
                {
                    (int)ComponentTypeEnum.kTransform2DComponent => new Transform2DComponentModel(),
                    (int)ComponentTypeEnum.kSpriteComponent => new SpriteComponentModel(),
                    _ => throw new JsonSerializationException($"Unknown ComponentType: {componentType}")
                };
                serializer.Populate(obj.CreateReader(), component);
                list.Add(component);
            }

            return list;
        }

        public override void WriteJson(JsonWriter writer, List<GameObjectComponentModel> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
