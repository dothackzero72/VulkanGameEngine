#include "RenderMesh2DComponent.h"
#include "MemoryManager.h"

RenderMesh2DComponent::RenderMesh2DComponent() : GameObjectComponent()
{
}

RenderMesh2DComponent::RenderMesh2DComponent(String name, uint32 meshBufferIndex) : GameObjectComponent(name, ComponentTypeEnum::kRenderMesh2DComponent)
{
	std::vector<Vertex2D> SpriteVertexList =
	{
	  { {0.0f, 0.5f},  {0.0f, 0.0f}, {1.0f, 0.0f, 0.0f, 1.0f} },
	  { {0.5f, 0.5f},  {1.0f, 0.0f}, {0.0f, 1.0f, 0.0f, 1.0f} },
	  { {0.5f, 0.0f},  {1.0f, 1.0f}, {0.0f, 0.0f, 1.0f, 1.0f} },
	  { {0.0f, 0.0f},  {0.0f, 1.0f}, {1.0f, 1.0f, 0.0f, 1.0f} }
	};
	std::vector<uint32> SpriteIndexList =
	{
	  0, 1, 3,
	  1, 2, 3
	};
	mesh = std::make_shared<Mesh2D>(Mesh2D(SpriteVertexList, SpriteIndexList, meshBufferIndex));
}

RenderMesh2DComponent::~RenderMesh2DComponent()
{
}

std::shared_ptr<RenderMesh2DComponent> RenderMesh2DComponent::CreateRenderMesh2DComponent(String name, uint32 meshBufferIndex)
{
	std::shared_ptr<RenderMesh2DComponent> gameObject = MemoryManager::AllocateRenderMesh2DComponent();
	new (gameObject.get()) RenderMesh2DComponent(name, meshBufferIndex);
	return gameObject;
}

void RenderMesh2DComponent::Update(float deltaTime)
{
	mesh->Update(deltaTime);
}

void RenderMesh2DComponent::BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
{
	mesh->BufferUpdate(commandBuffer, deltaTime);
	mesh->Update(deltaTime);
}

void RenderMesh2DComponent::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
	mesh->Draw(commandBuffer, pipeline, shaderPipelineLayout, descriptorSet, sceneProperties);
}

void RenderMesh2DComponent::Destroy()
{
	mesh->Destroy();
}

std::shared_ptr<GameObjectComponent> RenderMesh2DComponent::Clone() const
{
	return std::make_shared<RenderMesh2DComponent>(*this);
}

size_t RenderMesh2DComponent::GetMemorySize() const
{
	return sizeof(RenderMesh2DComponent);
}