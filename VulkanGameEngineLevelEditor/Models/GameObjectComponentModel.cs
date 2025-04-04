using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts;

namespace VulkanGameEngineLevelEditor.Models
{
    public class GameObjectComponentModel
    {
        public string Name { get; private set; }
        public ComponentTypeEnum ComponentType { get; private set; }

        public GameObjectComponentModel() 
            { }
    }
}
