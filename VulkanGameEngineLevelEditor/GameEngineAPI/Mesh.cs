using GlmSharp;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineGameObjectScripts.Import;
using VulkanGameEngineLevelEditor.Components;
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

    public unsafe class Mesh<T>
    {
        protected const VkBufferUsageFlagBits MeshBufferUsageSettings = VkBufferUsageFlagBits.VK_BUFFER_USAGE_VERTEX_BUFFER_BIT |
                                                                 VkBufferUsageFlagBits.VK_BUFFER_USAGE_INDEX_BUFFER_BIT |
                                                                 VkBufferUsageFlagBits.VK_BUFFER_USAGE_STORAGE_BUFFER_BIT |
                                                                 VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT;

        protected const VkMemoryPropertyFlagBits MeshBufferPropertySettings = VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                                                                       VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;

        protected IntPtr mesh;
        protected GameObject ParentGameObject { get; private set; }
        protected GameObjectComponent ParentGameObjectComponent { get; private set; }
        protected Transform2DComponent TransformRefrence { get; private set; }

        public uint MeshBufferIndex { get; protected set; }
        public int VertexCount { get; protected set; }
        public int IndexCount { get; protected set; }

        public MeshProperitiesStruct MeshProperties { get; set; }

        public mat4 MeshTransform { get; protected set; }
        public vec3 MeshPosition { get; protected set; }
        public vec3 MeshRotation { get; protected set; }
        public vec3 MeshScale { get; protected set; }

        public VulkanBuffer<Vertex2D> MeshVertexBuffer { get;  set; }
        public VulkanBuffer<UInt32> MeshIndexBuffer { get;  set; }
        public VulkanBuffer<mat4> MeshTransformBuffer { get;  set; }
        public VulkanBuffer<MeshProperitiesStruct> PropertiesBuffer { get; set; }

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

        public Mesh(GameObjectComponent parentGameObjectComponent)
        {
            ParentGameObject = parentGameObjectComponent.ParentGameObject;
            ParentGameObjectComponent = parentGameObjectComponent;
        }


        public void MeshStartUp(Vertex2D[] vertexList, uint[] indexList, Material material)
        {
            //var component = ParentGameObject.GameObjectComponentList.FirstOrDefault(x => x.ComponentType == ComponentTypeEnum.kGameObjectTransform2DComponent);
            //if (component is Transform2DComponent transformComponent)
            //{
            //    TransformRefrence = component as Transform2DComponent;
            //}
            //else
            //{
            //    TransformRefrence = null;
            //}

            VertexCount = vertexList.Length;
            IndexCount = indexList.Length;

            GCHandle vertexListHandle = GCHandle.Alloc(vertexList, GCHandleType.Pinned);
            GCHandle indexListHandle = GCHandle.Alloc(indexList, GCHandleType.Pinned);
            GCHandle transformHandle = GCHandle.Alloc(MeshTransform, GCHandleType.Pinned);
            GCHandle meshPropertiesHandle = GCHandle.Alloc(MeshProperties, GCHandleType.Pinned);

            MeshVertexBuffer = new VulkanBuffer<Vertex2D>(vertexListHandle.AddrOfPinnedObject(), (uint)vertexList.Count(), VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT |
                                                                                                                           VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT |
                                                                                                                           VkBufferUsageFlagBits.VK_BUFFER_USAGE_VERTEX_BUFFER_BIT,
                                                                                                                           VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                                                                                                                           VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, false);

            MeshIndexBuffer = new VulkanBuffer<UInt32>(indexListHandle.AddrOfPinnedObject(), (uint)indexList.Count(), VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT |
                                                                                                                      VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT |
                                                                                                                      VkBufferUsageFlagBits.VK_BUFFER_USAGE_INDEX_BUFFER_BIT,
                                                                                                                      VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                                                                                                                      VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, false);

            MeshTransformBuffer = new VulkanBuffer<mat4>(transformHandle.AddrOfPinnedObject(), 1, VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT |
                                                                                                  VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT |
                                                                                                  VkBufferUsageFlagBits.VK_BUFFER_USAGE_STORAGE_BUFFER_BIT,
                                                                                                  VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                                                                                                  VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, false);

            PropertiesBuffer = new VulkanBuffer<MeshProperitiesStruct>(meshPropertiesHandle.AddrOfPinnedObject(), 1, VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT |
                                                                                                                     VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT |
                                                                                                                     VkBufferUsageFlagBits.VK_BUFFER_USAGE_STORAGE_BUFFER_BIT,
                                                                                                                     VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                                                                                                                     VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, false);
            
            vertexListHandle.Free();
            indexListHandle.Free();
            transformHandle.Free();
            meshPropertiesHandle.Free();
        }

        public void Update(VkCommandBuffer commandBuffer, float deltaTime)
        {
            mat4 gameObjectTransform = mat4.Identity;
            if (TransformRefrence != null)
            {
                var transform = TransformRefrence.GameObjectTransform;
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
            PropertiesBuffer.UpdateBufferMemory(properties);
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
            PropertiesBuffer.DestroyBuffer();
            MeshTransformBuffer.DestroyBuffer();
        }

        public VkDescriptorBufferInfo GetVertexPropertiesBuffer()
        {
            return new VkDescriptorBufferInfo
            {
                buffer = MeshVertexBuffer.Buffer,
                offset = 0,
                range = VulkanConst.VK_WHOLE_SIZE

            };
        }

        public VkDescriptorBufferInfo GetIndexPropertiesBuffer()
        {
            return new VkDescriptorBufferInfo
            {
                buffer = MeshIndexBuffer.Buffer,
                offset = 0,
                range = VulkanConst.VK_WHOLE_SIZE

            };
        }

        public VkDescriptorBufferInfo GetTransformBuffer()
        {
            return new VkDescriptorBufferInfo
            {
                buffer = MeshTransformBuffer.Buffer,
                offset = 0,
                range = VulkanConst.VK_WHOLE_SIZE

            };
        }

        public VkDescriptorBufferInfo GetMeshPropertiesBuffer()
        {
            return new VkDescriptorBufferInfo
            {
                buffer = PropertiesBuffer.Buffer,
                offset = 0,
                range = VulkanConst.VK_WHOLE_SIZE

            };
        }
    }
}
