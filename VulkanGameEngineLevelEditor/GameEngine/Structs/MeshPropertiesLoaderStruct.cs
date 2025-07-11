using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct MeshPropertiesLoaderStruct
    {
        public uint PropertiesBufferId { get; set; }
        public size_t SizeofMeshProperties { get; set; }
        public void* MeshPropertiesData { get; set; }
    };
}
