#pragma once
#include "Mesh.h"

class Mesh2D : public Mesh
{
private:

protected:

public:
	Mesh2D();
	Mesh2D(Vector<Vertex2D>& vertexList, Vector<uint32>& indexList, SharedPtr<Material> material);
	virtual ~Mesh2D();

	virtual void Update(const float& deltaTime) override;
	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties) override;
	virtual void InstanceDraw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties, VulkanBuffer<SpriteInstanceStruct>& InstanceBuffer);
	virtual void Destroy() override;
};

