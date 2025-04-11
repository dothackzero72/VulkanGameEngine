using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public enum ComponentTypeEnum
    {
        kUndefined,
        kRenderMesh2DComponent,
        kTransform2DComponent,
        kInputComponent,
        kSpriteComponent
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct SceneDataBuffer
    {
        public uint MeshBufferIndex;
        public ulong buffer;
        public mat4 Projection;
        public mat4 View;
        public vec3 CameraPosition;

        public SceneDataBuffer()
        {
            MeshBufferIndex = uint.MaxValue;
            Projection = new mat4();
            View = new mat4();
            CameraPosition = new vec3(0.0f);
            buffer = 0;
        }
    };
}
