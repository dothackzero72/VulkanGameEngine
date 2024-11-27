using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
        kTestScriptConponent
    };

    public abstract class GameObjectComponent
    {
        public String Name { get; protected set; }
        public ulong MemorySize { get; protected set; }
        public ComponentTypeEnum ComponentType { get; protected set; }

        public GameObjectComponent()
        {

        }

        public GameObjectComponent(ComponentTypeEnum componentType)
        {
            Name = "unnamed";
            ComponentType = componentType;
        }

        public GameObjectComponent(String name, ComponentTypeEnum componentType)
        {
            Name = name;
            ComponentType = componentType;
        }

        public abstract void Update(long startTime);
        public abstract void BufferUpdate(CommandBuffer commandBuffer, long startTime);
        public abstract void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties);
        public abstract void Destroy();
        public abstract int GetMemorySize();
    }
}
