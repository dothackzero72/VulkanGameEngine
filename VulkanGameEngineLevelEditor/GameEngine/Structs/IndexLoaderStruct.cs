using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct IndexLoaderStruct
    {
        public uint MeshIndexBufferId { get; set; }
        public size_t SizeofIndex { get; set; }
        public size_t IndexCount { get; set; }
        public void* IndexData { get; set; }
    };
}
