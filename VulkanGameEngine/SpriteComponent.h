#pragma once
#include "GameObjectComponent.h"
#include "Sprite.h"

class SpriteComponent : public GameObjectComponent
{
private:
	SharedPtr<Sprite> SpriteObj = nullptr;

public:
	SpriteComponent();
	SpriteComponent(String& name, SharedPtr<GameObject> parentGameObjectPtr, SpriteSheet& spriteSheet);
	virtual ~SpriteComponent() override;

	virtual void Input(float deltaTime) override;
	virtual void Update(float deltaTime) override;
	virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime) override;
	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties) override;
	virtual void Destroy() override;
	virtual SharedPtr<GameObjectComponent> Clone() const override;
	virtual size_t GetMemorySize() const override;
};

