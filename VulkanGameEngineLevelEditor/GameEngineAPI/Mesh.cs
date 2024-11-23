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
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MeshProperitiesStruct
    {
        public uint MaterialIndex { get; set; }
        public long Padding { get; set; }
        public int Padding2 { get; set; }
        public mat4 MeshTransform { get; set; }
        public long Padding3 { get; set; }
        public MeshProperitiesStruct()
        {
            MaterialIndex = uint.MaxValue;
            MeshTransform = new mat4(1.0f, 2.0f, 3.0f, 4.0f,
                                     5.0f, 6.0f, 7.0f, 8.0f,
                                     9.0f, 10.0f, 11.0f, 12.0f,
                                     13.0f, 14.0f, 15.0f, 16.0f);
            Padding = 0;
            Padding2 = 0;
            Padding3 = 0;
        }
    };


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
        public MeshProperitiesStruct MeshProperties { get; set; }
        public mat4 MeshTransform { get; protected set; }
        public vec3 MeshPosition { get; protected set; }
        public vec3 MeshRotation { get; protected set; }
        public vec3 MeshScale { get; protected set; }

        public VulkanBuffer<Vertex2D> MeshVertexBuffer { get; protected set; }
        public VulkanBuffer<UInt32> MeshIndexBuffer { get; protected set; }
        public VulkanBuffer<MeshProperitiesStruct> UniformBuffers { get; set; }

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

        public void MeshStartUp(Vertex2D[] vertexList, uint[] indexList)
        {
            VertexCount = vertexList.Length;
            IndexCount = indexList.Length;

            GCHandle vhandle = GCHandle.Alloc(vertexList, GCHandleType.Pinned);
            IntPtr vpointer = vhandle.AddrOfPinnedObject();
            MeshVertexBuffer = new VulkanBuffer<Vertex2D>((void*)vpointer, (uint)vertexList.Count(), BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit | BufferUsageFlags.VertexBufferBit, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit, true);

            GCHandle fhandle = GCHandle.Alloc(indexList, GCHandleType.Pinned);
            IntPtr fpointer = fhandle.AddrOfPinnedObject();
            MeshIndexBuffer = new VulkanBuffer<UInt32>((void*)fpointer, (uint)indexList.Count(), BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit | BufferUsageFlags.IndexBufferBit, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit, true);

            GCHandle uhandle = GCHandle.Alloc(UniformBuffers, GCHandleType.Pinned);
            IntPtr upointer = uhandle.AddrOfPinnedObject();
            UniformBuffers = new VulkanBuffer<MeshProperitiesStruct>((void*)upointer, 1, BufferUsageFlags.StorageBufferBit | BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit, MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit, true);
        }

        virtual public void Destroy()
        {
            MeshVertexBuffer.DestroyBuffer();
            MeshIndexBuffer.DestroyBuffer();
            UniformBuffers.DestroyBuffer();
        }

        virtual public void Update(float deltaTime)
        {
        }

        public static mat4 Transpose(mat4 matrix)
        {
            return new mat4(
                matrix[0, 0], matrix[1, 0], matrix[2, 0], matrix[3, 0],
                matrix[0, 1], matrix[1, 1], matrix[2, 1], matrix[3, 1],
                matrix[0, 2], matrix[1, 2], matrix[2, 2], matrix[3, 2],
                matrix[0, 3], matrix[1, 3], matrix[2, 3], matrix[3, 3]
            );
        }

        public virtual void BufferUpdate(CommandBuffer commandBuffer, long startTime)
        {
            mat4 MeshMatrix = mat4.Identity;
            MeshMatrix = mat4.Scale(MeshScale);
            MeshMatrix = mat4.Rotate(glm.Radians(MeshRotation.x), new vec3(1.0f, 0.0f, 0.0f));
            MeshMatrix = mat4.Rotate(glm.Radians(MeshRotation.y), new vec3(0.0f, 1.0f, 0.0f));
            MeshMatrix = mat4.Rotate(glm.Radians(MeshRotation.z), new vec3(0.0f, 0.0f, 1.0f));
            MeshMatrix = mat4.Translate(MeshPosition);

            MeshProperitiesStruct properties = new MeshProperitiesStruct
            {
                MaterialIndex = MeshBufferIndex,
                MeshTransform = MeshMatrix,
            };

            void* dataPtr = Unsafe.AsPointer(ref properties);
            UniformBuffers.UpdateBufferData(dataPtr);
        }

        public virtual void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            sceneProperties.MeshBufferIndex = MeshBufferIndex;

            ulong offsets = 0;
            uint sceneDataSize = (uint)sizeof(SceneDataBuffer);

            var meshBuffer = MeshVertexBuffer.Buffer;
            vk.CmdPushConstants(commandBuffer, shaderPipelineLayout, ShaderStageFlags.VertexBit | ShaderStageFlags.FragmentBit, 0, (uint)sizeof(SceneDataBuffer), &sceneProperties);
            vk.CmdBindPipeline(commandBuffer, PipelineBindPoint.Graphics, pipeline);
            vk.CmdBindDescriptorSets(commandBuffer, PipelineBindPoint.Graphics, shaderPipelineLayout, 0, 1, &descriptorSet, 0, null);
            vk.CmdBindVertexBuffers(commandBuffer, 0, 1, &meshBuffer, &offsets);
            vk.CmdBindIndexBuffer(commandBuffer, MeshIndexBuffer.Buffer, 0, IndexType.Uint32);
            vk.CmdDrawIndexed(commandBuffer, (uint)IndexCount, 1, 0, 0, 0);
        }

        public VulkanBuffer<MeshProperitiesStruct> GetMeshPropertiesBuffer()
        {
            return UniformBuffers;
        }
    }
}
