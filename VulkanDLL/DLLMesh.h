#pragma once
#include "DLL.h"
#include <Mesh.h>
#include <Mesh2D.h>

class DLL_EXPORT DLLMesh : public Mesh
{
private:
public:
	DLLMesh();
	virtual ~DLLMesh();

	template<class T>
	void MeshStartUp(List<T>& vertexList, List<uint32>& indexList)
	{
		Mesh::MeshStartUp();
	}
	virtual void Update(const float& deltaTime) override;
	virtual void BufferUpdate(VkCommandBuffer& commandBuffer, const float& deltaTime) override;
	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties) override;
	virtual void Destroy() override;

	MeshPropertiesBuffer* GetMeshPropertiesBuffer() { return &PropertiesBuffer; }
};

//class DLL_EXPORT DLLMesh2D : public Mesh2D
//{
//private:
//public:
//	DLLMesh2D();
//	virtual ~DLLMesh2D();
//};