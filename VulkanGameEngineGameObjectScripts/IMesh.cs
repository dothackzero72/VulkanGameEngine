using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts.Import;

namespace VulkanGameEngineGameObjectScripts
{
    public interface IMesh
    {
        public void MeshStartUp(IntPtr parentGameObjectPtr, Vertex2D[] vertexList, uint[] indexList);
        public void Update(float deltaTime);
        public void BufferUpdate(VkCommandBuffer commandBuffer, float deltaTime);
        public void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout shaderPipelineLayout, VkDescriptorSet descriptorSet, SceneDataBuffer sceneProperties);
        public void Destroy();
    }
}
