#pragma once
#include "Mesh.h"

class Mesh2D : public Mesh
{
private:
protected:
public:
	Mesh2D();
	Mesh2D(List<Vertex2D>& vertexList, List<uint32>& indexList, uint32 MeshBufferIndex);
	virtual ~Mesh2D();
	virtual void Update(const float& deltaTime) override;
	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties) override;
	virtual void Destroy() override;
};

