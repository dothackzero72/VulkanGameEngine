using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineGameObjectScripts.Component;
using VulkanGameEngineGameObjectScripts.Input;
using VulkanGameEngineGameObjectScripts.Vulkan;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    //RenderMesh2DComponent is basically just a skeletion container for linking from C++ to C#. 
    //Mesh and Draw calls are too diffrent to cleanly run them on C#.
    public unsafe class MeshRenderer2DComponent : RenderMesh2DComponent
    {
        public Mesh2D mesh { get; protected set; } = new Mesh2D();
        public MeshRenderer2DComponent()
        {

        }

        public MeshRenderer2DComponent(IntPtr parentGameObject, String name, uint meshBufferIndex)
        {
            List<Vertex2D> spriteVertexList = new List<Vertex2D>
            {
                new Vertex2D(new vec2(0.0f, 0.5f), new vec2(0.0f, 0.0f), new vec4(1.0f, 0.0f, 0.0f, 1.0f)),
                new Vertex2D(new vec2(0.5f, 0.5f), new vec2(1.0f, 0.0f), new vec4(0.0f, 1.0f, 0.0f, 1.0f)),
                new Vertex2D(new vec2(0.5f, 0.0f), new vec2(1.0f, 1.0f), new vec4(0.0f, 0.0f, 1.0f, 1.0f)),
                new Vertex2D(new vec2(0.0f, 0.0f), new vec2(0.0f, 1.0f), new vec4(1.0f, 1.0f, 0.0f, 1.0f))
            };

            List<uint> spriteIndexList = new List<uint> { 0, 1, 3, 1, 2, 3 };

            mesh = new Mesh2D(parentGameObject);
        }

        public override void Input(KeyBoardKeys key, float deltaTime)
        {

        }

        public override void Update(float deltaTime)
        {
            mesh.Update(deltaTime);
        }

        public override void BufferUpdate(VkCommandBuffer commandBuffer, float deltaTime)
        {
            mesh.BufferUpdate(commandBuffer, deltaTime);
            mesh.Update(deltaTime);
        }

        public override void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout pipelineLayout, VkDescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            mesh.Draw(commandBuffer, pipeline, pipelineLayout, descriptorSet, sceneProperties);
        }

        public override void Destroy()
        {
            mesh.Destroy();
        }

        public override int GetMemorySize()
        {
            return (int)sizeof(MeshRenderer2DComponent);
        }
    }
}