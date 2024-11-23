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
    public enum ComponentTypeEnum
    {
        kUndefined,
        kRenderMesh2DComponent,
        kTestScriptConponent
    };

    //public class GameObjectComponent : IGameObjectComponent
    //{
    //    //private const string DLLPath = "C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\VulkanGameEngineGameObjectScripts\\bin\\Debug\\net8.0\\VulkanGameEngineGameObjectScripts.dll";
    //    //public String Name { get; protected set; }
    //    //public ulong MemorySize { get; protected set; }
    //    //public ComponentTypeEnum ComponentType { get; protected set; }

    //    //public GameObjectComponent()
    //    //{

    //    //}

    //    //public GameObjectComponent(ComponentTypeEnum componentType)
    //    //{
    //    //    Name = "unnamed";
    //    //    ComponentType = componentType;
    //    //}

    //    //public GameObjectComponent(String name, ComponentTypeEnum componentType)
    //    //{
    //    //    Name = name;
    //    //    ComponentType = componentType;
    //    //}
    //    //virtual public void Destroy()
    //    //{
    //    //    throw new NotImplementedException();
    //    //}

    //    //virtual public void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
    //    //{
    //    //}

    //    //virtual unsafe public int GetMemorySize()
    //    //{
    //    //    return sizeof(GameObjectComponent);
    //    //}

    //    //virtual public void Update(long startTime)
    //    //{
    //    //}

    //    //virtual public void Update(CommandBuffer commandBuffer, long startTime)
    //    //{
    //    //}

    //    //public void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, VulkanGameEngineGameObjectScripts.SceneDataBuffer sceneProperties)
    //    //{
    //    //    throw new NotImplementedException();
    //    //}
    //}
}