using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts;

namespace ClassLibrary1
{
    public interface IGameObject
    {
        public void Input(InputKey key, KeyState keyState);
        public void Update(float deltaTime);
        public void BufferUpdate(IntPtr commandBuffer, float deltaTime);
        public void Draw(IntPtr commandBuffer, IntPtr pipeline, IntPtr shaderPipelineLayout, IntPtr descriptorSet, SceneDataBuffer sceneProperties);
        public void Destroy();
        public int GetMemorySize();
    }
}
