using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngine.Systems;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct VertexLoaderStruct
    {
        public BufferTypeEnum VertexType { get; set; }
        public uint MeshVertexBufferId { get; set; }
        public size_t SizeofVertex { get; set; }
        public size_t VertexCount { get; set; }
        public void* VertexData { get; set; }
    };
}
