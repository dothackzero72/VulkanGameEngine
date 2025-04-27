using GlmSharp;
using System.Runtime.InteropServices;

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
