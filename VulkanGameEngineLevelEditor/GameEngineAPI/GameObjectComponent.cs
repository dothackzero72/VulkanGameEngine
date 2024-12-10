using ClassLibrary1;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VulkanGameEngineGameObjectScripts;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public abstract class GameObjectComponent2 : GameObjectComponent
    {
        public nint ParentGameObject { get; set; }
        public String Name { get; set; }
        public ulong MemorySize { get; set; }
        public ComponentTypeEnum ComponentType { get; set; }

        public GameObjectComponent2()
        {

        }

    }
}
