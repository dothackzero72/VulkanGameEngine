//using GlmSharp;
//using Silk.NET.Vulkan;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlTypes;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VulkanGameEngineLevelEditor.Vulkan;

//namespace VulkanGameEngineLevelEditor.GameEngineAPI
//{
//    public unsafe class MeshRenderer3DComponent : GameObjectComponent
//    {
//        public Mesh3D mesh { get; protected set; } = new Mesh3D();
//        public MeshRenderer3DComponent()
//        {
//        }

//        private void Initialize(String name, Vertex3D[] vertexList, uint[] indexList, uint meshBufferIndex)
//        {
//            Name = name;
//            mesh = new Mesh3D(vertexList, indexList, meshBufferIndex);
//        }

//        public static MeshRenderer3DComponent CreateRenderMesh3DComponent(String name, Vertex3D[] vertexList, uint[] indexList, uint meshBufferIndex)
//        {
//            MeshRenderer3DComponent gameObject = MemoryManager.AllocateGameRenderMesh3DComponent();
//            gameObject.Initialize(name, vertexList, indexList, meshBufferIndex);
//            return gameObject;
//        }

//        public override void Update(long startTime)
//        {
//            mesh.Update(startTime);
//        }

//        public override void Update(CommandBuffer commandBuffer, long startTime)
//        {
//            mesh.BufferUpdate(commandBuffer, startTime);
//            mesh.Update(startTime);
//        }

//        public override void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
//        {
//            mesh.Draw(commandBuffer, pipeline, shaderPipelineLayout, descriptorSet, sceneProperties);
//        }

//        public override void Destroy()
//        {
//            mesh.Destroy();
//        }

//        public override int GetMemorySize()
//        {
//            return sizeof(MeshRenderer3DComponent);
//        }

//        public VulkanBuffer<MeshProperitiesStruct> GetMeshPropertiesBuffer()
//        {
//            return mesh.GetMeshPropertiesBuffer();
//        }
//    }
//}