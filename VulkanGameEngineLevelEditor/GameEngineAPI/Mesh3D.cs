using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class Mesh3D : Mesh
    {
        public Mesh3D() : base()
        {
        }

        public Mesh3D(Vertex3D[] vertexList, uint[] indexList, uint MeshBufferIndex) : base()
        {
            base.MeshStartUp(vertexList, indexList);
        }
    }
}
