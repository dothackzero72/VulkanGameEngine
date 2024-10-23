#pragma once
#include <vulkan/vulkan_core.h>
#include "Typedef.h"
#include "SceneDataBuffer.h"

enum ComponentTypeEnum
{
	kUndefined,
	kRenderMesh2DComponent
};

class GameObjectComponent
{
private:
protected:

public:
	String Name;
	size_t MemorySize = 0;
	ComponentTypeEnum ComponentType;

	GameObjectComponent()
	{
		Name = "Unnamed";
	}

	GameObjectComponent(ComponentTypeEnum componentType)
	{
		Name = "unnamed";
		ComponentType = componentType;
	}

	GameObjectComponent(String name, ComponentTypeEnum componentType)
	{
		Name = name;
		ComponentType = componentType;
	}

	virtual ~GameObjectComponent()
	{

	}

	virtual void Update(float deltaTime)
	{
	}

	virtual void Update(VkCommandBuffer& commandBuffer, float deltaTime)
	{}

	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties) = 0;
	virtual void Destroy() = 0;
	virtual std::shared_ptr<GameObjectComponent> Clone() const = 0;
	virtual size_t GetMemorySize() const = 0;
};

