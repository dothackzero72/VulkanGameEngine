using GlmSharp;
using Silk.NET.Maths;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineGameObjectScripts.Component;
using VulkanGameEngineGameObjectScripts.Import;
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


    public unsafe static class MathHelper
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
        private const VkBufferUsageFlagBits MeshBufferUsageSettings = VkBufferUsageFlagBits.VK_BUFFER_USAGE_VERTEX_BUFFER_BIT |
                                                                 VkBufferUsageFlagBits.VK_BUFFER_USAGE_INDEX_BUFFER_BIT |
                                                                 VkBufferUsageFlagBits.VK_BUFFER_USAGE_STORAGE_BUFFER_BIT |
                                                                 VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT;

        private const VkMemoryPropertyFlagBits MeshBufferPropertySettings = VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                                                                       VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;

        protected IntPtr mesh;
        protected IntPtr ParentGameObject { get; private set; }
        protected IntPtr TransformRefrence { get; private set; }

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

        public void MeshStartUp(nint parentGameObjectPtr, Vertex2D[] vertexList, uint[] indexList)
        {
            GCHandle handle = GCHandle.FromIntPtr(parentGameObjectPtr);
            var gameObject = handle.Target as GameObject;

            ParentGameObject = parentGameObjectPtr;

            var component = gameObject.GameObjectComponentList.FirstOrDefault(x => x.ComponentType == ComponentTypeEnum.kGameObjectTransform2DComponent);
            if (component is Transform2DComponent transformComponent)
            {
                GCHandle handle1 = GCHandle.Alloc(component, GCHandleType.Normal);
                IntPtr transformComponentHandle = GCHandle.ToIntPtr(handle1);
                TransformRefrence = transformComponentHandle;
            }
            else
            {
                TransformRefrence = IntPtr.Zero;
            }

            VertexCount = vertexList.Length;
            IndexCount = indexList.Length;

            GCHandle vhandle = GCHandle.Alloc(vertexList, GCHandleType.Pinned);
            IntPtr vpointer = vhandle.AddrOfPinnedObject();
            MeshVertexBuffer = new VulkanBuffer<Vertex2D>((void*)vpointer, (uint)vertexList.Count(), VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT | 
                VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT | 
                VkBufferUsageFlagBits.VK_BUFFER_USAGE_VERTEX_BUFFER_BIT,
                VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, true);

            GCHandle fhandle = GCHandle.Alloc(indexList, GCHandleType.Pinned);
            IntPtr fpointer = fhandle.AddrOfPinnedObject();
            MeshIndexBuffer = new VulkanBuffer<UInt32>((void*)fpointer, (uint)indexList.Count(), VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT | 
                VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT | 
                VkBufferUsageFlagBits.VK_BUFFER_USAGE_INDEX_BUFFER_BIT,
                VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, true);

            GCHandle uhandle = GCHandle.Alloc(UniformBuffers, GCHandleType.Pinned);
            IntPtr upointer = uhandle.AddrOfPinnedObject();
            UniformBuffers = new VulkanBuffer<MeshProperitiesStruct>((void*)upointer, 1, VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT | 
                VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT | 
                VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT,
                VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, true);
        }

        public void Update(float deltaTime)
        {

        }

        public void BufferUpdate(nint commandBuffer, float deltaTime)
        {
            mat4 gameObjectTransform = mat4.Identity;
            if (TransformRefrence != IntPtr.Zero)
            {
                GCHandle handle = GCHandle.FromIntPtr(TransformRefrence);
                var goTransform = handle.Target as Transform2DComponent;
                var transform = goTransform.GameObjectTransform;

                gameObjectTransform = transform;
            }

            mat4 MeshMatrix = mat4.Identity;
            MeshMatrix = mat4.Scale(MeshScale);
            MeshMatrix = mat4.Rotate(CLIMath.DegreesToRadians(MeshRotation.x), new vec3(1.0f, 0.0f, 0.0f));
            MeshMatrix = mat4.Rotate(CLIMath.DegreesToRadians(MeshRotation.y), new vec3(0.0f, 1.0f, 0.0f));
            MeshMatrix = mat4.Rotate(CLIMath.DegreesToRadians(MeshRotation.z), new vec3(0.0f, 0.0f, 1.0f));
            MeshMatrix = mat4.Translate(MeshPosition);

            MeshProperitiesStruct properties = new MeshProperitiesStruct
            {
                MaterialIndex = MeshBufferIndex,
                MeshTransform = gameObjectTransform * MeshMatrix,
            };

            void* dataPtr = Unsafe.AsPointer(ref properties);
            UniformBuffers.UpdateBufferData(dataPtr);
        }

        public void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout pipelineLayout, VkDescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            sceneProperties.MeshBufferIndex = MeshBufferIndex;

            ulong offsets = 0;
            uint sceneDataSize = (uint)sizeof(SceneDataBuffer);

            var meshBuffer = MeshVertexBuffer.Buffer;
            var descriptorSetRef = descriptorSet;
            VkFunc.vkCmdPushConstants(new VkCommandBuffer(commandBuffer), pipelineLayout, VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT | VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT, 0, (uint)sizeof(SceneDataBuffer), &sceneProperties);
            VkFunc.vkCmdBindPipeline(new VkCommandBuffer(commandBuffer), VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline);
            VkFunc.vkCmdBindDescriptorSets(new VkCommandBuffer(commandBuffer), VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipelineLayout, 0, 1, &descriptorSetRef, 0, null);
            VkFunc.vkCmdBindVertexBuffers(new VkCommandBuffer(commandBuffer), 0, 1, &meshBuffer, &offsets);
            VkFunc.vkCmdBindIndexBuffer(new VkCommandBuffer(commandBuffer), MeshIndexBuffer.Buffer, 0, VkIndexType.VK_INDEX_TYPE_UINT32);
            VkFunc.vkCmdDrawIndexed(new VkCommandBuffer(commandBuffer), (uint)IndexCount, 1, 0, 0, 0);
        }

        public void Destroy()
        {
            MeshVertexBuffer.DestroyBuffer();
            MeshIndexBuffer.DestroyBuffer();
            UniformBuffers.DestroyBuffer();
        }

        public VulkanBuffer<MeshProperitiesStruct> GetMeshPropertiesBuffer()
        {
            return UniformBuffers;
        }
    }
}
