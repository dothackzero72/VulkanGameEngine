using Newtonsoft.Json;
using System.Collections.Generic;

namespace VulkanGameEngineLevelEditor.Models
{
    public class GameObjectModel
    {
        public string Name { get; set; }
        [JsonConverter(typeof(GameObjectComponentConverter))]
        public List<GameObjectComponentModel> GameObjectComponentList { get; set; }
        GameObjectModel() { }

    }
}
