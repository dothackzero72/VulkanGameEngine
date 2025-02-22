#include "SpriteComponent.h"
#include "MemoryManager.h"

SpriteComponent::SpriteComponent()
{
}

SpriteComponent::SpriteComponent(String& name, SharedPtr<GameObject> parentGameObjectPtr, SpriteSheet& spriteSheet) : GameObjectComponent(this, parentGameObjectPtr, kSpriteComponent)
{
	SpriteObj = std::make_shared<Sprite>(Sprite(parentGameObjectPtr, spriteSheet));
}

SpriteComponent::~SpriteComponent()
{
}

void SpriteComponent::Input(float deltaTime)
{
	SpriteObj->Input(deltaTime);
}

void SpriteComponent::Update(float deltaTime)
{
	SpriteObj->Update(deltaTime);
}

void SpriteComponent::BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
{
	SpriteObj->BufferUpdate(commandBuffer, deltaTime);
}

void SpriteComponent::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
	SpriteObj->Draw(commandBuffer, pipeline, pipelineLayout, descriptorSet, sceneProperties);
}

void SpriteComponent::Destroy()
{
	SpriteObj->Destroy();
}

SharedPtr<GameObjectComponent> SpriteComponent::Clone() const
{
	return std::make_shared<SpriteComponent>(*this);
}

size_t SpriteComponent::GetMemorySize() const
{
	return sizeof(SpriteComponent);
}
