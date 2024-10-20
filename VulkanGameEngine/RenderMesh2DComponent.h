#pragma once
#include "GameObjectComponent.h"
#include "Mesh2D.h"
class RenderMesh2DComponent : public GameObjectComponent
{
private:
	std::shared_ptr<Mesh2D> mesh;
public:
	RenderMesh2DComponent();
	RenderMesh2DComponent(String name);
	virtual ~RenderMesh2DComponent() override;

	virtual void Update(float deltaTime) override;
	virtual void Update(VkCommandBuffer& commandBuffer, float deltaTime) override;
	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties) override;
	virtual void Destroy() override;
	virtual std::shared_ptr<GameObjectComponent> Clone() const override;
	virtual size_t GetMemorySize() const override;

	MeshPropertiesBuffer* GetMeshPropertiesBuffer() { return mesh->GetMeshPropertiesBuffer(); }
};

