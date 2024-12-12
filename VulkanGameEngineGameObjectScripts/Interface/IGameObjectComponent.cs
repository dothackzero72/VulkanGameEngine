using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineGameObjectScripts.Input;

namespace VulkanGameEngineGameObjectScripts.Interface
{
    public interface IGameObjectComponent
    {
        public void Input(KeyBoardKeys key, float deltaTime);
        public void Update(float deltaTime);
        public void BufferUpdate(IntPtr commandBuffer, float deltaTime);
        public void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout pipelineLayout, VkDescriptorSet descriptorSet, SceneDataBuffer sceneProperties);
        public void Destroy();
        public int GetMemorySize();
    }
}
