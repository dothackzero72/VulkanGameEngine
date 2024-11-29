using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Components
{
    public class TestScriptComponent : GameObjectComponent
    {
        private TestScriptComponent_CS testScript_CS = new TestScriptComponent_CS();

        public TestScriptComponent()
        {
            testScript_CS = new TestScriptComponent_CS();
        }

        public TestScriptComponent(IntPtr parentGameObject, String name)
        {
            testScript_CS = new TestScriptComponent_CS(parentGameObject, name);
        }

        public override void BufferUpdate(CommandBuffer commandBuffer, float deltaTime)
        {
            testScript_CS.BufferUpdate(commandBuffer, deltaTime);
        }

        public override void Destroy()
        {
            testScript_CS.Destroy();
        }

        public override void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            testScript_CS.Draw(commandBuffer, pipeline, shaderPipelineLayout, descriptorSet, sceneProperties);
        }

        public override int GetMemorySize()
        {
            return testScript_CS.GetMemorySize();
        }

        public override void Update(float deltaTime)
        {
            testScript_CS.Update(deltaTime);
        }
    }
}
