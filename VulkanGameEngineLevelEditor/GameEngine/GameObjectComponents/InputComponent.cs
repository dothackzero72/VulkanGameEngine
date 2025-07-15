using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngine.GameObjectComponents
{
    public struct InputComponent
    {

        public int GameObjectId;

        public InputComponent()
        {
            GameObjectId = 0;
        }

        public InputComponent(int gameObjectId)
        {
            GameObjectId = gameObjectId;
        }
    };
}
