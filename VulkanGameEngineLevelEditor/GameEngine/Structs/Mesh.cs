using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngine.Systems;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 16)]
    public struct Mesh
    {
        public uint MeshId = 0;
        public uint ParentGameObjectID = 0;
        public uint GameObjectTransform = 0;
        public nuint VertexCount = 0;
        public nuint IndexCount = 0;
        public Guid MaterialId = Guid.Empty;
        public BufferTypeEnum VertexType = BufferTypeEnum.BufferType_Undefined;
        public vec3 MeshPosition = new vec3(0.0f);
        public vec3 MeshRotation = new vec3(0.0f);
        public vec3 MeshScale = new vec3(1.0f);
        public int MeshVertexBufferId = 0;
        public int MeshIndexBufferId = 0;
        public int MeshTransformBufferId = 0;
        public int PropertiesBufferId = 0;
        public MeshPropertiesStruct MeshProperties = new MeshPropertiesStruct();

        public Mesh()
        {
        }
    };
}
