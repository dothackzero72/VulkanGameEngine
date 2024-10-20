#include "GameObject.h"
#include <Macro.h>
#include <iostream>

GameObject::GameObject()
{
}

GameObject::GameObject(String name)
{
	Name = name;
}

GameObject::GameObject(String name, List<std::shared_ptr<GameObjectComponent>> gameObjectComponentList)
{
	Name = name;
	GameObjectComponentList = gameObjectComponentList;
}

void GameObject::Update(float deltaTime)
{
	for (std::shared_ptr<GameObjectComponent> component : GameObjectComponentList)
	{
		component->Update(deltaTime);
	}
}

void GameObject::BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
{
	for (std::shared_ptr<GameObjectComponent> component : GameObjectComponentList)
	{
		component->Update(commandBuffer, deltaTime);
	}
}

void GameObject::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
	for (std::shared_ptr<GameObjectComponent> component : GameObjectComponentList)
	{
		component->Draw(commandBuffer, pipeline, shaderPipelineLayout, descriptorSet, sceneProperties);
	}
}

void GameObject::Destroy()
{
	for (std::shared_ptr<GameObjectComponent> component : GameObjectComponentList)
	{
		component->Destroy();
	}
}

void GameObject::AddComponent(std::shared_ptr<GameObjectComponent> newComponent)
{
	GameObjectComponentList.emplace_back(newComponent);
}

void GameObject::RemoveComponent(size_t index)
{
	GameObjectComponentList.erase(GameObjectComponentList.begin() + index);
}