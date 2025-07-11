using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
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
}
