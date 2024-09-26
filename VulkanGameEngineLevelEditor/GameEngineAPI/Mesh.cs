using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public MeshProperitiesStruct MeshProperties { get; set; }
        public mat4 MeshTransform { get; protected set; }
        public Vector3 MeshPosition { get; protected set; }
        public Vector3 MeshRotation { get; protected set; }
        public Vector3 MeshScale { get; protected set; }

        public DynamicVulkanBuffer<Vertex2D> MeshVertexBuffer { get; protected set; }
        public DynamicVulkanBuffer<UInt32> MeshIndexBuffer { get; protected set; }
        public DynamicVulkanBuffer<MeshProperitiesStruct> PropertiesBuffer { get; protected set; }

        public Mesh()
        {
            MeshBufferIndex = 0;
            MeshTransform = new mat4();
            MeshPosition = new Vector3(0.0f);
            MeshRotation = new Vector3(0.0f);
            MeshScale = new Vector3(1.0f);

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

        public virtual void BufferUpdate(VkCommandBuffer commandBuffer, float deltaTime)
        {
            // Initialize the mesh transformation matrix as an identity matrix
            Matrix4x4 meshMatrix = Matrix4x4.Identity;

            // Apply transformations
            meshMatrix = Matrix4x4.CreateTranslation(MeshPosition) * meshMatrix;
            meshMatrix = Matrix4x4.CreateRotationX(MathHelper.ToRadians(MeshRotation.X)) * meshMatrix;
            meshMatrix = Matrix4x4.CreateRotationY(MathHelper.ToRadians(MeshRotation.Y)) * meshMatrix;
            meshMatrix = Matrix4x4.CreateRotationZ(MathHelper.ToRadians(MeshRotation.Z)) * meshMatrix;
            meshMatrix = Matrix4x4.CreateScale(MeshScale) * meshMatrix;

            // Create a temporary variable to hold the struct
            var properties = MeshProperties;

            // Modify the fields of the temporary struct
            properties.MeshIndex = 1;
            properties.MaterialIndex++;
            properties.MeshTransform = MeshTransform;

            // Assign the modified struct back
            MeshProperties = properties;

            // You may need to upload the meshMatrix to the GPU or use it as needed
        }

        public virtual unsafe void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout shaderPipelineLayout, VkDescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            SceneDataBuffer SceneProperties = new SceneDataBuffer();
            SceneProperties.Projection = new mat4();
            SceneProperties.View = new mat4();
            SceneProperties.CameraPosition = new vec3(0.0f);

            if (commandBuffer == null) throw new ArgumentNullException(nameof(commandBuffer));
            if (pipeline == null) throw new ArgumentNullException(nameof(pipeline));
            if (shaderPipelineLayout == null) throw new ArgumentNullException(nameof(shaderPipelineLayout));
            if (descriptorSet == null) throw new ArgumentNullException(nameof(descriptorSet));
            //if (sceneProperties == null) throw new ArgumentNullException(nameof(sceneProperties));

            VkDeviceSize offsets = 0;
            uint sceneDataSize = (uint)sizeof(SceneDataBuffer);
            // Ensure the size of SceneDataBuffer matches what the shader expects.
            VulkanAPI.vkCmdPushConstants(commandBuffer, shaderPipelineLayout,
                VkShaderStageFlags.VK_SHADER_STAGE_VERTEX_BIT | VkShaderStageFlags.VK_SHADER_STAGE_FRAGMENT_BIT,
                0, sceneDataSize, &SceneProperties);

            VulkanAPI.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline);
            VulkanAPI.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, shaderPipelineLayout, 0, 1, &descriptorSet, 0, null);

            var meshBuffer = MeshVertexBuffer.Buffer;
            VulkanAPI.vkCmdBindVertexBuffers(commandBuffer, 0, 1, &meshBuffer, &offsets);
            VulkanAPI.vkCmdBindIndexBuffer(commandBuffer, MeshIndexBuffer.Buffer, 0, VkIndexType.VK_INDEX_TYPE_UINT32);

            VulkanAPI.vkCmdDrawIndexed(commandBuffer, 6, 1, 0, 0, 0); // Ensure IndexCount is correct here.
        }
    }
}
