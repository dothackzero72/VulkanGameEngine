using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace VulkanGameEngineGameObjectScripts
{
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

    public enum ComponentTypeEnum
    {
        kUndefined,
        kRenderMesh2DComponent,
        kGameObjectTransform2DComponent,
        kTestScriptComponent
    };

    public interface IGameObjectComponent
    {
        IntPtr ParentGameObject { get; set; }
        String Name { get; set; }
        ulong MemorySize { get; }
        ComponentTypeEnum ComponentType { get; }
        void Update(float deltaTime);
        void BufferUpdate(IntPtr commandBuffer, float deltaTime);
        void Draw(IntPtr commandBuffer, IntPtr pipeline, IntPtr shaderPipelineLayout, IntPtr descriptorSet, SceneDataBuffer sceneProperties);
        void Destroy();
        int GetMemorySize();
    }
}
