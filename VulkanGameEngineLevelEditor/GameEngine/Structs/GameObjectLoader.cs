using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
    public class GameObjectLoader
    {
        public string GameObjectPath { get; set; }
        public List<double> GameObjectPositionOverride { get; set; }
    }
}
