using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct TransformLoaderStruct
    {
        public uint MeshTransformBufferId { get; set; }
        public size_t SizeofTransform { get; set; }
        public void* TransformData { get; set; }
    };
}
