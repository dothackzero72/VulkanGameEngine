using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineGameObjectScripts
{
    public unsafe class GameObjectTransform2D_CS : IGameObjectComponent
    {
        public mat4 GameObjectTransform { get; set; }
        public vec2 GameObjectPosition { get; set; }
        public vec2 GameObjectRotation { get; set; }
        public vec2 GameObjectScale { get; set; }
        public nint ParentGameObject { get; set; }
        public string Name { get; set; }
        public ulong MemorySize { get; set; }
        float offset { get; set; } = 0.0f;

        public ComponentTypeEnum ComponentType { get; set; } = ComponentTypeEnum.kGameObjectTransform2DComponent;

        public GameObjectTransform2D_CS()
        {

        }

        public GameObjectTransform2D_CS(IntPtr parentGameObject, String name)
        {
        }

        public void BufferUpdate(nint commandBuffer, float deltaTime)
        {
            CommandBuffer commandBuffer1 = new CommandBuffer { Handle = commandBuffer };
            BufferUpdate(commandBuffer1, deltaTime);
        }

        public void BufferUpdate(CommandBuffer commandBuffer, float deltaTime)
        {

        }

        public void Destroy()
        {
   
        }

        public void Draw(nint commandBuffer, nint pipeline, nint shaderPipelineLayout, nint descriptorSet, SceneDataBuffer sceneProperties)
        {
            CommandBuffer commandBuffer1 = new CommandBuffer { Handle = commandBuffer };
            Pipeline pipeline1 = new Pipeline { Handle = (ulong)pipeline };
            PipelineLayout shaderPipelineLayout1 = new PipelineLayout { Handle = (ulong)shaderPipelineLayout };
            DescriptorSet descriptorSet1 = new DescriptorSet { Handle = (ulong)descriptorSet };

            Draw(commandBuffer1, pipeline1, shaderPipelineLayout1, descriptorSet1, sceneProperties);
        }

        public void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
     
        }

        public int GetMemorySize()
        {
            return (int)sizeof(GameObjectTransform2D_CS);
        }

        public void Update(float deltaTime)
        {
            GameObjectPosition = new vec2(offset, 0.0f);
            offset += 0.01f;
            GameObjectTransform = mat4.Identity;
            GameObjectTransform = mat4.Scale(new vec3(GameObjectScale, 0.0f));
            GameObjectTransform = mat4.Rotate(glm.Radians(GameObjectRotation.x), new vec3(1.0f, 0.0f, 0.0f));
            GameObjectTransform = mat4.Rotate(glm.Radians(GameObjectRotation.y), new vec3(0.0f, 1.0f, 0.0f));
            GameObjectTransform = mat4.Translate(new vec3(GameObjectPosition, 0.0f));
        }
    }
}
