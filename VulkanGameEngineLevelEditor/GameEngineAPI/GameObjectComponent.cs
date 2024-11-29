using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VulkanGameEngineGameObjectScripts;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public abstract class GameObjectComponent : IGameObjectComponent
    {
        public nint ParentGameObject { get; set; }
        public String Name { get; set; }
        public ulong MemorySize { get; set; }
        public ComponentTypeEnum ComponentType { get; set; }

        public GameObjectComponent()
        {

        }

        public GameObjectComponent(nint parentGameObject, ComponentTypeEnum componentType)
        {
            ParentGameObject = parentGameObject;
            Name = "unnamed";
            ComponentType = componentType;
        }

        public GameObjectComponent(nint parentGameObject, String name, ComponentTypeEnum componentType)
        {
            ParentGameObject = parentGameObject;
            Name = name;
            ComponentType = componentType;
        }

        public abstract void Update(float deltaTime);
        public abstract void BufferUpdate(CommandBuffer commandBuffer, float deltaTime);
        public abstract void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties);
        public abstract void Destroy();
        public abstract int GetMemorySize();

        public void BufferUpdate(nint commandBuffer, float deltaTime)
        {
            throw new NotImplementedException();
        }

        public void Draw(nint commandBuffer, nint pipeline, nint shaderPipelineLayout, nint descriptorSet, SceneDataBuffer sceneProperties)
        {
            CommandBuffer commandBuffer1 = new CommandBuffer { Handle = commandBuffer };
            Pipeline pipeline1 = new Pipeline { Handle = (ulong)pipeline };
            PipelineLayout shaderPipelineLayout1 = new PipelineLayout { Handle = (ulong)shaderPipelineLayout };
            DescriptorSet descriptorSet1 = new DescriptorSet { Handle = (ulong)descriptorSet };

            Draw(commandBuffer1, pipeline1, shaderPipelineLayout1, descriptorSet1, sceneProperties);
        }
    }
}
