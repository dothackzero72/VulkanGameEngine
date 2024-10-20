#pragma once
#include <vulkan/vulkan_core.h>
#include "Typedef.h"
#include "SceneDataBuffer.h"

class GameObjectComponent
{
private:
protected:

public:
	String Name;
	size_t MemorySize = 0;

	GameObjectComponent()
	{
		MemorySize = GetMemorySize();
	}

	GameObjectComponent(String name)
	{
		Name = name;
		MemorySize = GetMemorySize();
	}

	virtual ~GameObjectComponent()
	{

	}

	virtual void Update(float deltaTime)
	{
	}
	virtual void Update(VkCommandBuffer& commandBuffer, float deltaTime)
	{}
	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
	{}
	virtual void Destroy()
	{}
	virtual std::shared_ptr<GameObjectComponent> Clone() const
	{
		return nullptr;
	}
	virtual size_t GetMemorySize() const
	{
		return sizeof(*this);
	}
};

