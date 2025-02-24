#pragma once
#include "GameObjectComponent.h"
#include "Sprite.h"

class SpriteComponent : public GameObjectComponent
{
private:
	SharedPtr<Sprite> SpriteObj = nullptr;

public:
	SpriteComponent();
	SpriteComponent(uint32 gameObjectId, const String& name, SpriteSheet& spriteSheet);
	virtual ~SpriteComponent() override;

	virtual void Input(const float& deltaTime) override;
	virtual void Update(VkCommandBuffer& commandBuffer, const float& deltaTime) override;
	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties) override;
	virtual void Destroy() override;
	virtual SharedPtr<GameObjectComponent> Clone() const override;
	virtual size_t GetMemorySize() const override;

	SharedPtr<Sprite> GetSprite() { return SpriteObj; }
};

