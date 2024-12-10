using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineGameObjectScripts
{
    public interface IGameObject
    {
        public void Input(InputKey key, KeyState keyState);
        public void Update(float deltaTime);
        public void BufferUpdate(VkCommandBuffer commandBuffer, float deltaTime);
        public void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout pipelineLayout, VkDescriptorSet descriptorSet, SceneDataBuffer sceneProperties);
        public void Destroy();
        public int GetMemorySize();
    }
}
