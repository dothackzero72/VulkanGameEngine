using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Mesh 
    {
        private const VkBufferUsageFlags MeshBufferUsageSettings = VkBufferUsageFlags.VK_BUFFER_USAGE_VERTEX_BUFFER_BIT |
                                                                   VkBufferUsageFlags.VK_BUFFER_USAGE_INDEX_BUFFER_BIT |
                                                                   VkBufferUsageFlags.VK_BUFFER_USAGE_STORAGE_BUFFER_BIT |
                                                                   VkBufferUsageFlags.VK_BUFFER_USAGE_TRANSFER_DST_BIT;

        private const VkMemoryPropertyFlagBits MeshBufferPropertySettings = VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                                                                            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT;

        protected IntPtr mesh;
        public UInt64 MeshBufferIndex { get; protected set; }
        public int VertexCount { get; protected set; }
        public int IndexCount { get; protected set; }

        public MeshProperitiesStruct MeshProperties { get; protected set; }
        public mat4 MeshTransform { get; protected set; }
        public vec3 MeshPosition { get; protected set; }
        public vec3 MeshRotation { get; protected set; }
        public vec3 MeshScale { get; protected set; }

        public DynamicVulkanBuffer<Vertex2D> MeshVertexBuffer { get; protected set; }
        public DynamicVulkanBuffer<UInt32> MeshIndexBuffer { get; protected set; }
        public DynamicVulkanBuffer<MeshProperitiesStruct> PropertiesBuffer { get; protected set; }

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

            MeshVertexBuffer = new DynamicVulkanBuffer<Vertex2D>(vertexList, MeshBufferUsageSettings, MeshBufferPropertySettings);
            MeshIndexBuffer = new DynamicVulkanBuffer<UInt32>(indexList, MeshBufferUsageSettings, MeshBufferPropertySettings);
            PropertiesBuffer = new DynamicVulkanBuffer<MeshProperitiesStruct>(MeshProperties, MeshBufferUsageSettings, MeshBufferPropertySettings);
        }

        virtual public void Destroy()
        {
            MeshVertexBuffer.DestroyBuffer();
            MeshIndexBuffer.DestroyBuffer();
            PropertiesBuffer.DestroyBuffer();
        }

        virtual public void Update(float deltaTime)
        {
            GameEngineDLL.DLL_Mesh_Update(mesh, deltaTime);
        }

        virtual public void BufferUpdate(VkCommandBuffer commandBuffer,  float deltaTime)
        {
            //mat4 MeshMatrix = mat4(1.0f);
            //MeshMatrix = glm::translate(MeshMatrix, MeshPosition);
            //MeshMatrix = glm::rotate(MeshMatrix, glm::radians(MeshRotation.x), vec3(1.0f, 0.0f, 0.0f));
            //MeshMatrix = glm::rotate(MeshMatrix, glm::radians(MeshRotation.y), vec3(0.0f, 1.0f, 0.0f));
            //MeshMatrix = glm::rotate(MeshMatrix, glm::radians(MeshRotation.z), vec3(0.0f, 0.0f, 1.0f));
            //MeshMatrix = glm::scale(MeshMatrix, MeshScale);
        }

        virtual public void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout shaderPipelineLayout, VkDescriptorSet descriptorSet, MeshProperitiesStruct sceneProperties)
        {
            //VkDeviceSize offsets[] = { 0 };
            //VulkanAPI.vkCmdPushConstants(commandBuffer, shaderPipelineLayout, VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT, 0, sizeof(SceneDataBuffer), &sceneProperties);
            //VulkanAPI.vkCmdBindPipeline(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline);
            //VulkanAPI.vkCmdBindDescriptorSets(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, shaderPipelineLayout, 0, 1, &descriptorSet, 0, nullptr);
            //VulkanAPI.vkCmdBindVertexBuffers(commandBuffer, 0, 1, &MeshVertexBuffer.Buffer, offsets);
            //VulkanAPI.vkCmdBindIndexBuffer(commandBuffer, MeshIndexBuffer.Buffer, 0, VK_INDEX_TYPE_UINT32);
            //VulkanAPI.vkCmdDrawIndexed(commandBuffer, 6, 1, 0, 0, 0);
        }
    }
}
