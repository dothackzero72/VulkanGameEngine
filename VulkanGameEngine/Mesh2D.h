#pragma once
#include "Mesh.h"

class Mesh2D : public Mesh<Vertex2D>
{
private:

protected:

public:
	Mesh2D();
	Mesh2D(Vector<Vertex2D>& vertexList, Vector<uint32>& indexList, SharedPtr<Material> material);
	virtual ~Mesh2D();

	virtual void Update(VkCommandBuffer& commandBuffer, const float& deltaTime) override;
	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneDataBuffer) override;
	virtual void InstanceDraw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, VulkanBuffer<SpriteInstanceStruct>& InstanceBuffer);
	virtual void Destroy() override;
};

