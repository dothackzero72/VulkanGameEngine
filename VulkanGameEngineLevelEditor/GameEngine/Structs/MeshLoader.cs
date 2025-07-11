using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct MeshLoader
    {
        public uint ParentGameObjectID { get; set; }
        public uint MeshId { get; set; }
        public Guid MaterialId { get; set; }
        public VertexLoaderStruct VertexLoader { get; set; }
        public IndexLoaderStruct IndexLoader { get; set; }
        public TransformLoaderStruct TransformLoader { get; set; }
        public MeshPropertiesLoaderStruct MeshPropertiesLoader { get; set; }
    };
}
