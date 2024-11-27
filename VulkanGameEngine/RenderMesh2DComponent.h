#pragma once
#include "GameObjectComponent.h"
#include "Mesh2D.h"
class RenderMesh2DComponent : public GameObjectComponent
{
private:
	std::shared_ptr<Mesh2D> mesh;
	RenderMesh2DComponent(String name, uint32 meshBufferIndex);
public:
	RenderMesh2DComponent();
	virtual ~RenderMesh2DComponent() override;

	static std::shared_ptr<RenderMesh2DComponent> CreateRenderMesh2DComponent(String name, uint32 meshBufferIndex);

	virtual void Update(float deltaTime) override;
	virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime) override;
	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties) override;
	virtual void Destroy() override;
	virtual std::shared_ptr<GameObjectComponent> Clone() const override;
	virtual size_t GetMemorySize() const override;

	MeshPropertiesBuffer* GetMeshPropertiesBuffer() { return mesh->GetMeshPropertiesBuffer(); }
	const std::shared_ptr<Mesh2D> GetMesh2D() { return mesh; }
};
