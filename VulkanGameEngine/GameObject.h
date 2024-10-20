#pragma once
extern "C"
{
#include <CGameObject.h>
}

#include "Typedef.h"
#include "GameObjectComponent.h"

class GameObject
{
private:
	String Name;
	size_t ObjectComponentMemorySize = 0;


public:
	List<std::shared_ptr<GameObjectComponent>> GameObjectComponentList = List<std::shared_ptr<GameObjectComponent>>();

	GameObject();
	GameObject(String name);
	GameObject(String name, List<std::shared_ptr<GameObjectComponent>> gameObjectComponentList);

	template <class T>
	T GetGameObjectComponentByName(String name)
	{

	}

	template <class T>
	List<T> GetGameObjectComponentsByObjectType(T objectType)
	{

	}

	virtual void Update(float deltaTime);
	virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime);
	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
	virtual void Destroy();
	void AddComponent(std::shared_ptr<GameObjectComponent> newComponent);
	void RemoveComponent(size_t index);


};

