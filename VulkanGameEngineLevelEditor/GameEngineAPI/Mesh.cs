using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public static class MathHelper
    {
        public static float ToRadians(float degrees)
        {
            return degrees * ((float)Math.PI / 180f);
        }

        public static float ToDegrees(float radians)
        {
            return radians * (180f / (float)Math.PI);
        }
    }

    public unsafe class Mesh
    {
        Vk vk = Vk.GetApi();
        private const BufferUsageFlags MeshBufferUsageSettings = BufferUsageFlags.VertexBufferBit |
                                                                 BufferUsageFlags.IndexBufferBit |
                                                                 BufferUsageFlags.StorageBufferBit |
                                                                 BufferUsageFlags.TransferDstBit;

        private const MemoryPropertyFlags MeshBufferPropertySettings = MemoryPropertyFlags.HostVisibleBit |
                                                                       MemoryPropertyFlags.HostCoherentBit;

        protected IntPtr mesh;
        public uint MeshBufferIndex { get; protected set; }
        public int VertexCount { get; protected set; }
        public int IndexCount { get; protected set; }

        public MeshProperitiesBuffer MeshProperties { get; set; }
        public mat4 MeshTransform { get; protected set; }
        public vec3 MeshPosition { get; protected set; }
        public vec3 MeshRotation { get; protected set; }
        public vec3 MeshScale { get; protected set; }

        public VulkanBuffer<Vertex2D> MeshVertexBuffer { get; protected set; }
        public VulkanBuffer<UInt32> MeshIndexBuffer { get; protected set; }
        public VulkanBuffer<MeshProperitiesBuffer> PropertiesBuffer { get; protected set; }

        public Mesh()
        {
            MeshBufferIndex = 0;
            MeshTransform = new mat4();
            MeshPosition = new vec3(0.0f);
            MeshRotation = new vec3(0.0f);
            MeshScale = new vec3(1.0f);

            VertexCount = 0;
            IndexCount = 0;
        }


        public void MeshStartUp(List<Vertex2D> vertexList, List<uint> indexList)
        {
            VertexCount = vertexList.Count;
            IndexCount = indexList.Count;

            GCHandle vhandle = GCHandle.Alloc(vertexList, GCHandleType.Pinned);
            IntPtr vpointer = vhandle.AddrOfPinnedObject();
            MeshVertexBuffer = new VulkanBuffer<Vertex2D>((void*)vpointer, (uint)vertexList.Count(), BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit | BufferUsageFlags.VertexBufferBit, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit, true);

            GCHandle fhandle = GCHandle.Alloc(indexList, GCHandleType.Pinned);
            IntPtr fpointer = fhandle.AddrOfPinnedObject();
            MeshIndexBuffer = new VulkanBuffer<UInt32>((void*)fpointer, (uint)indexList.Count(), BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit | BufferUsageFlags.IndexBufferBit, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit, true);

            GCHandle uhandle = GCHandle.Alloc(MeshProperties, GCHandleType.Pinned);
            IntPtr upointer = uhandle.AddrOfPinnedObject();
            PropertiesBuffer = new VulkanBuffer<MeshProperitiesBuffer>((void*)upointer, 1, BufferUsageFlags.UniformBufferBit | BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit, true);
        }

        virtual public void Destroy()
        {
            MeshVertexBuffer.DestroyBuffer();
            MeshIndexBuffer.DestroyBuffer();
            PropertiesBuffer.DestroyBuffer();
        }

        virtual public void Update(float deltaTime)
        {
        }

        public virtual void BufferUpdate(CommandBuffer commandBuffer, float deltaTime)
        {
            mat4 meshMatrix = mat4.Identity;
            meshMatrix = mat4.Translate(MeshPosition) * meshMatrix;
            meshMatrix = mat4.RotateX(MathHelper.ToRadians(MeshRotation.x)) * meshMatrix;
            meshMatrix = mat4.RotateY(MathHelper.ToRadians(MeshRotation.y)) * meshMatrix;
            meshMatrix = mat4.RotateZ(MathHelper.ToRadians(MeshRotation.z)) * meshMatrix;
            meshMatrix = mat4.Scale(MeshScale) * meshMatrix;

            var properties = MeshProperties;
            properties.MaterialIndex  = MeshBufferIndex;
            properties.MeshTransform = meshMatrix;

            MeshProperties = properties;
        }

        public virtual void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            SceneDataBuffer SceneProperties = new SceneDataBuffer();
            SceneProperties.Projection = new mat4();
            SceneProperties.View = new mat4();
            SceneProperties.CameraPosition = new vec3(0.0f);

            ulong offsets = 0;
            uint sceneDataSize = (uint)sizeof(SceneDataBuffer);

            var meshBuffer = MeshVertexBuffer.Buffer;
            vk.CmdPushConstants(commandBuffer, shaderPipelineLayout, ShaderStageFlags.VertexBit | ShaderStageFlags.FragmentBit, 0, sceneDataSize, &SceneProperties);
            vk.CmdBindPipeline(commandBuffer, PipelineBindPoint.Graphics, pipeline);
            vk.CmdBindDescriptorSets(commandBuffer, PipelineBindPoint.Graphics, shaderPipelineLayout, 0, 1, &descriptorSet, 0, null);
            vk.CmdBindVertexBuffers(commandBuffer, 0, 1, &meshBuffer, &offsets);
            vk.CmdBindIndexBuffer(commandBuffer, MeshIndexBuffer.Buffer, 0, IndexType.Uint32);
            vk.CmdDrawIndexed(commandBuffer, 6, 1, 0, 0, 0);
        }

        public VulkanBuffer<MeshProperitiesBuffer> GetMeshPropertiesBuffer() 
        { 
            return PropertiesBuffer; 
        }
    }
}
