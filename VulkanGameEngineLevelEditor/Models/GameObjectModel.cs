using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Components;

namespace VulkanGameEngineLevelEditor.Models
{
    public class GameObjectModel
    {
        public string Name { get; private set; } = string.Empty;
        [JsonConverter(typeof(GameObjectComponentConverter))]
        public List<GameObjectComponentModel> GameObjectComponentList { get; private set; } = new List<GameObjectComponentModel>();
        public GameObjectModel() { }
    }
}
