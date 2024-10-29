using GlmSharp;
using Silk.NET.Maths;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
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

      //  public MeshProperitiesBuffer MeshProperties { get; set; }
        public mat4 MeshTransform { get; protected set; }
        public vec3 MeshPosition { get; protected set; }
        public vec3 MeshRotation { get; protected set; }
        public vec3 MeshScale { get; protected set; }

        public VulkanBuffer<Vertex3D> MeshVertexBuffer { get; protected set; }
        public VulkanBuffer<UInt32> MeshIndexBuffer { get; protected set; }

        public VulkanBuffer<UniformBufferObject> uniformBuffers { get; set; }

        UniformBufferObject ubo;

        readonly Vertex3D[] vertices = new Vertex3D[]
{
            new Vertex3D(new (-0.5f, -0.5f, 0.0f), new (1.0f, 0.0f, 0.0f), new (1.0f, 0.0f)),
            new Vertex3D(new (0.5f, -0.5f, 0.0f), new (0.0f, 1.0f, 0.0f), new (0.0f, 0.0f)),
            new Vertex3D(new (0.5f, 0.5f, 0.0f), new (0.0f, 0.0f, 1.0f), new (0.0f, 1.0f)),
            new Vertex3D(new (-0.5f, 0.5f, 0.0f), new (1.0f, 1.0f, 1.0f), new (1.0f, 1.0f)),

            new Vertex3D(new (-0.5f, -0.5f, -0.5f), new (1.0f, 0.0f, 0.0f), new (1.0f, 0.0f)),
            new Vertex3D(new (0.5f, -0.5f, -0.5f), new (0.0f, 1.0f, 0.0f), new (0.0f, 0.0f)),
            new Vertex3D(new (0.5f, 0.5f, -0.5f), new (0.0f, 0.0f, 1.0f), new (0.0f, 1.0f)),
            new Vertex3D(new (-0.5f, 0.5f, -0.5f), new (1.0f, 1.0f, 1.0f), new (1.0f, 1.0f))
};

        readonly uint[] indices = new uint[]
        {
            0, 1, 2, 2, 3, 0,
            4, 5, 6, 6, 7, 4
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct UniformBufferObject
        {
            public Matrix4X4<float> model;
            public Matrix4X4<float> view;
            public Matrix4X4<float> proj;

        }

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


        public void MeshStartUp()
        {
            VertexCount = vertices.Length;
            IndexCount = indices.Length;

            GCHandle vhandle = GCHandle.Alloc(vertices, GCHandleType.Pinned);
            IntPtr vpointer = vhandle.AddrOfPinnedObject();
            MeshVertexBuffer = new VulkanBuffer<Vertex3D>((void*)vpointer, (uint)vertices.Count(), BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit | BufferUsageFlags.VertexBufferBit, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit, true);

            GCHandle fhandle = GCHandle.Alloc(indices, GCHandleType.Pinned);
            IntPtr fpointer = fhandle.AddrOfPinnedObject();
            MeshIndexBuffer = new VulkanBuffer<UInt32>((void*)fpointer, (uint)indices.Count(), BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit | BufferUsageFlags.IndexBufferBit, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit, true);

            GCHandle uhandle = GCHandle.Alloc(ubo, GCHandleType.Pinned);
            IntPtr upointer = uhandle.AddrOfPinnedObject();
            uniformBuffers = new VulkanBuffer<UniformBufferObject>((void*)upointer, 1, BufferUsageFlags.StorageBufferBit | BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit, true);

        }

        public void MeshStartUp(Vertex3D[] vertexList, uint[] indexList)
        {
            VertexCount = vertexList.Length;
            IndexCount = indexList.Length;

            GCHandle vhandle = GCHandle.Alloc(vertexList, GCHandleType.Pinned);
            IntPtr vpointer = vhandle.AddrOfPinnedObject();
            MeshVertexBuffer = new VulkanBuffer<Vertex3D>((void*)vpointer, (uint)vertexList.Count(), BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit | BufferUsageFlags.VertexBufferBit, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit, true);

            GCHandle fhandle = GCHandle.Alloc(indexList, GCHandleType.Pinned);
            IntPtr fpointer = fhandle.AddrOfPinnedObject();
            MeshIndexBuffer = new VulkanBuffer<UInt32>((void*)fpointer, (uint)indexList.Count(), BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit | BufferUsageFlags.IndexBufferBit, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit, true);

            GCHandle uhandle = GCHandle.Alloc(ubo, GCHandleType.Pinned);
            IntPtr upointer = uhandle.AddrOfPinnedObject();
            uniformBuffers = new VulkanBuffer<UniformBufferObject>((void*)upointer, 1, BufferUsageFlags.StorageBufferBit | BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit, true);
        }

        virtual public void Destroy()
        {
            MeshVertexBuffer.DestroyBuffer();
            MeshIndexBuffer.DestroyBuffer();
            uniformBuffers.DestroyBuffer();
        }

        virtual public void Update(float deltaTime)
        {
        }

        public virtual void BufferUpdate(CommandBuffer commandBuffer, long startTime)
        {
            float secondsPassed = (float)TimeSpan.FromTicks(DateTime.Now.Ticks - startTime).TotalSeconds;

            ubo.model = Matrix4X4.CreateFromAxisAngle(
                new Vector3D<float>(0, 0, 1),
                secondsPassed * Scalar.DegreesToRadians(90.0f));

            ubo.view = Matrix4X4.CreateLookAt(
                new Vector3D<float>(2.0f, 2.0f, 2.0f),
                new Vector3D<float>(0.0f, 0.0f, 0.0f),
                new Vector3D<float>(0.0f, 0.0f, -0.1f));

            ubo.proj = Matrix4X4.CreatePerspectiveFieldOfView(
                Scalar.DegreesToRadians(45.0f),
                VulkanRenderer.swapChain.swapchainExtent.Width / (float)VulkanRenderer.swapChain.swapchainExtent.Height,
                0.1f,
                10.0f);

            ubo.proj.M11 *= -1;
            uint dataSize = (uint)Marshal.SizeOf(ubo);
            void* dataPtr = Unsafe.AsPointer(ref ubo);

            uniformBuffers.UpdateBufferData(dataPtr);
        }

        public virtual void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet)
        {
            ulong offsets = 0;
            uint sceneDataSize = (uint)sizeof(SceneDataBuffer);

            var meshBuffer = MeshVertexBuffer.Buffer;
            vk.CmdBindDescriptorSets(commandBuffer, PipelineBindPoint.Graphics, shaderPipelineLayout, 0, 1, &descriptorSet, 0, null);
            vk.CmdBindVertexBuffers(commandBuffer, 0, 1, &meshBuffer, &offsets);
            vk.CmdBindIndexBuffer(commandBuffer, MeshIndexBuffer.Buffer, 0, IndexType.Uint32);
            vk.CmdDrawIndexed(commandBuffer, (uint)IndexCount, 1, 0, 0, 0);
        }

        public VulkanBuffer<UniformBufferObject> GetMeshPropertiesBuffer()
        {
            return uniformBuffers;
        }
    }
}
