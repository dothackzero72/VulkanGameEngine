using System.Collections.Generic;
using System.Runtime.InteropServices;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Mesh2D : Mesh<Vertex2D>
    {
        public Mesh2D() : base()
        {
        }

        public Mesh2D(List<Vertex2D> vertexList, List<uint> indexList, Material material) : base()
        {
            MeshStartUp(vertexList.ToArray(), indexList.ToArray(), material);
        }


        public void Update(VkCommandBuffer commandBuffer, float deltaTime)
        {
            base.Update(commandBuffer, deltaTime);
        }

        public void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout shaderPipelineLayout, ListPtr<VkDescriptorSet> descriptorSetList, SceneDataBuffer sceneDataBuffer)
        {
            sceneDataBuffer.MeshBufferIndex = MeshBufferIndex;

            ulong offsets = 0;
            GCHandle vertexHandle = GCHandle.Alloc(MeshVertexBuffer.Buffer, GCHandleType.Pinned);
            VkFunc.vkCmdPushConstants(commandBuffer, shaderPipelineLayout, VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT | VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT, 0, (uint)sizeof(SceneDataBuffer), &sceneDataBuffer);
            VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline);
            VkFunc.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, shaderPipelineLayout, 0, 1, descriptorSetList.Ptr, 0, null);
            VkFunc.vkCmdBindVertexBuffers(commandBuffer, 0, 1, (nint*)vertexHandle.AddrOfPinnedObject(), &offsets);
            VkFunc.vkCmdBindIndexBuffer(commandBuffer, MeshIndexBuffer.Buffer, 0, VkIndexType.VK_INDEX_TYPE_UINT32);
            VkFunc.vkCmdDrawIndexed(commandBuffer, (uint)IndexCount, 1, 0, 0, 0);
        }

        public void InstanceDraw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout shaderPipelineLayout, ListPtr<VkDescriptorSet> descriptorSetList, VulkanBuffer<SpriteInstanceStruct> InstanceBuffer)
        {
        }

        public void Destroy()
        {
            base.Destroy();
        }
    }
}