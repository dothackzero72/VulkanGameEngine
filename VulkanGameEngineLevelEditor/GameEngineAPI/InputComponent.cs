using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Systems;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class InputComponent
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
