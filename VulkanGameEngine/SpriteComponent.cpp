#include "SpriteComponent.h"

SpriteComponent::SpriteComponent()
{
}

SpriteComponent::SpriteComponent(SharedPtr<GameObject> parentGameObjectPtr) : GameObjectComponent(this, parentGameObjectPtr, kSpriteComponent)
{
}

SpriteComponent::SpriteComponent(SharedPtr<GameObject> parentGameObjectPtr, String& name) : GameObjectComponent(this, parentGameObjectPtr, name, kSpriteComponent)
{
}

SpriteComponent::~SpriteComponent()
{
}

void SpriteComponent::Input(float deltaTime)
{
	sprite->Input(deltaTime);
}

void SpriteComponent::Update(float deltaTime)
{
	sprite->Update(deltaTime);
}

void SpriteComponent::BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
{
	sprite->BufferUpdate(commandBuffer, deltaTime);
}

void SpriteComponent::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
	sprite->Draw(commandBuffer, pipeline, pipelineLayout, descriptorSet, sceneProperties);
}

void SpriteComponent::Destroy()
{
	sprite->Destroy();
}

SharedPtr<GameObjectComponent> SpriteComponent::Clone() const
{
	return std::make_shared<SpriteComponent>(*this);
}

size_t SpriteComponent::GetMemorySize() const
{
	return sizeof(SpriteComponent);
}
