using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 16)]
    public struct MeshPropertiesStruct
    {
        public uint ShaderMaterialBufferIndex = 0;
        private uint _padding1 = 0;
        private uint _padding2 = 0;
        private uint _padding3 = 0;
        public mat4 MeshTransform = mat4.Identity;

        public MeshPropertiesStruct()
        {

        }
    };

}
