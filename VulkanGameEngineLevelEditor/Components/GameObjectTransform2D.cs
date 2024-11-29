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
    public class GameObjectTransform2D : GameObjectComponent
    {
        public GameObjectTransform2D_CS transform2D_CS { get; set; } = new GameObjectTransform2D_CS();

        public GameObjectTransform2D()
        {
            transform2D_CS = new GameObjectTransform2D_CS();
            ComponentType = transform2D_CS.ComponentType;
        }

        public GameObjectTransform2D(IntPtr parentGameObject, String name)
        {
            transform2D_CS = new GameObjectTransform2D_CS(parentGameObject, name);
            ComponentType = transform2D_CS.ComponentType;
        }

        public override void BufferUpdate(CommandBuffer commandBuffer, float deltaTime)
        {
            transform2D_CS.BufferUpdate(commandBuffer, deltaTime);
            ComponentType = transform2D_CS.ComponentType;
        }

        public override void Destroy()
        {
            transform2D_CS.Destroy();
        }

        public override void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            transform2D_CS.Draw(commandBuffer, pipeline, shaderPipelineLayout, descriptorSet, sceneProperties);
        }

        public override int GetMemorySize()
        {
            return transform2D_CS.GetMemorySize();
        }

        public override void Update(float deltaTime)
        {
            transform2D_CS.Update(deltaTime);
        }


    }
}
