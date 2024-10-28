using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class MeshRenderer2DComponent : GameObjectComponent
    {
        public Mesh2D mesh { get; protected set; } = new Mesh2D();
        public MeshRenderer2DComponent()
        {

        }

        private void Initialize(String name, uint meshBufferIndex)
        {
            List<Vertex2D> spriteVertexList = new List<Vertex2D>
            {
              new Vertex2D(new VertexVec2(0.0f, 0.5f), new VertexVec2(0.0f, 0.0f), new VertexVec4(1.0f, 0.0f, 0.0f, 1.0f)),
                new Vertex2D(new VertexVec2(0.5f, 0.5f), new VertexVec2(1.0f, 0.0f), new VertexVec4(0.0f, 1.0f, 0.0f, 1.0f)),
                new Vertex2D(new VertexVec2(0.5f, 0.0f), new VertexVec2(1.0f, 1.0f), new VertexVec4(0.0f, 0.0f, 1.0f, 1.0f)),
                new Vertex2D(new VertexVec2(0.0f, 0.0f), new VertexVec2(0.0f, 1.0f), new VertexVec4(1.0f, 1.0f, 0.0f, 1.0f))
            };

            List<uint> spriteIndexList = new List<uint> { 0, 1, 3, 1, 2, 3 };

            mesh = new Mesh2D(meshBufferIndex);
        }

        public static MeshRenderer2DComponent CreateRenderMesh2DComponent(String name, uint meshBufferIndex) 
        {
            List<Vertex2D> spriteVertexList = new List<Vertex2D>
            {
              new Vertex2D(new VertexVec2(0.0f, 0.5f), new VertexVec2(0.0f, 0.0f), new VertexVec4(1.0f, 0.0f, 0.0f, 1.0f)),
                new Vertex2D(new VertexVec2(0.5f, 0.5f), new VertexVec2(1.0f, 0.0f), new VertexVec4(0.0f, 1.0f, 0.0f, 1.0f)),
                new Vertex2D(new VertexVec2(0.5f, 0.0f), new VertexVec2(1.0f, 1.0f), new VertexVec4(0.0f, 0.0f, 1.0f, 1.0f)),
                new Vertex2D(new VertexVec2(0.0f, 0.0f), new VertexVec2(0.0f, 1.0f), new VertexVec4(1.0f, 1.0f, 0.0f, 1.0f))
            };

            List<uint> spriteIndexList = new List<uint> { 0, 1, 3, 1, 2, 3 };

            MeshRenderer2DComponent gameObject = MemoryManager.AllocateGameRenderMesh2DComponent();
            gameObject.Initialize(name, meshBufferIndex);
            return gameObject;
        }

        public override void Update(float deltaTime)
        {
            mesh.Update(deltaTime);
        }

        public override void Update(CommandBuffer commandBuffer, float deltaTime)
        {
            mesh.BufferUpdate(commandBuffer, deltaTime);
            mesh.Update(deltaTime);
        }

        public override void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            mesh.Draw(commandBuffer, pipeline, shaderPipelineLayout, descriptorSet, sceneProperties);
        }

        public override void Destroy()
        {
            mesh.Destroy();
        }

        public override int GetMemorySize()
        {
            return sizeof(MeshRenderer2DComponent);
        }

        public VulkanBuffer<MeshProperitiesBuffer> GetMeshPropertiesBuffer()
        {
            return mesh.GetMeshPropertiesBuffer();
        }
    }
}