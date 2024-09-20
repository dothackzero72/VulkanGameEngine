#include "DLLMesh.h"

DLLMesh::DLLMesh() : Mesh()
{
}

DLLMesh::~DLLMesh()
{
}

void DLLMesh::Update(const float& deltaTime)
{
	Mesh::Update(deltaTime);
}

void DLLMesh::BufferUpdate(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
	Mesh::BufferUpdate(commandBuffer, deltaTime);
}

void DLLMesh::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
	Mesh::Draw(commandBuffer, pipeline, shaderPipelineLayout, descriptorSet, sceneProperties);
}

void DLLMesh::Destroy()
{
	Mesh::Destroy();
}

//DLLMesh2D::DLLMesh2D() : Mesh2D()
//{
//}
//
//DLLMesh2D::~DLLMesh2D()
//{
//}
