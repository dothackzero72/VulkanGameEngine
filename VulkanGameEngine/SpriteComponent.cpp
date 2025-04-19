#include "SpriteComponent.h"
#include "MemoryManager.h"

SpriteComponent::SpriteComponent()
{
}

SpriteComponent::SpriteComponent(uint32 gameObjectId, const String& name, uint32 spriteSheetID) : GameObjectComponent(gameObjectId, kSpriteComponent)
{
	SpriteObj = std::make_shared<Sprite>(Sprite(gameObjectId, spriteSheetID));
}

SpriteComponent::~SpriteComponent()
{
}

void SpriteComponent::Input(const float& deltaTime)
{
	SpriteObj->Input(deltaTime);
}

void SpriteComponent::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
	SpriteObj->Update(commandBuffer, deltaTime);
}

void SpriteComponent::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, Vector<VkDescriptorSet>& descriptorSetList)
{
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
