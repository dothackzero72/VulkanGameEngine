//#pragma once
//#include "GameObjectComponent.h"
//#include "Mesh2D.h"
//
//class RenderMesh2DComponent : public GameObjectComponent
//{
//private:
//	SharedPtr<Mesh2D> mesh;
//	RenderMesh2DComponent(SharedPtr<GameObject> parentGameObjectPtr, String name, uint32 meshBufferIndex);
//
//public:
//	RenderMesh2DComponent();
//	virtual ~RenderMesh2DComponent() override;
//
//	static SharedPtr<RenderMesh2DComponent> CreateRenderMesh2DComponent(SharedPtr<GameObject> parentGameObjectPtr, String name, uint32 meshBufferIndex);
//
//	virtual void Input(float deltaTime) override;
//	virtual void Update(float deltaTime) override;
//	virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime) override;
//	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties) override;
//	virtual void Destroy() override;
//	virtual SharedPtr<GameObjectComponent> Clone() const override;
//	virtual size_t GetMemorySize() const override;
//
//	void GetMeshPropertiesBuffer(std::vector<VkDescriptorBufferInfo>& meshBufferList) { return mesh->GetMeshPropertiesBuffer(meshBufferList); }
//	const SharedPtr<Mesh2D> GetMesh2D() { return mesh; }
//};
