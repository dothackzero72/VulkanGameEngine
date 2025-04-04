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
        public string Name { get; set; }
        List<GameObjectComponent> GameObjectComponentList { get; set; }
        public GameObjectModel() { }
    }
}
