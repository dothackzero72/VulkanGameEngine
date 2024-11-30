#include "GameObject.h"
#include <Macro.h>
#include <iostream>
#include "MemoryManager.h"
#include "RenderMesh2DComponent.h"
#include "Transform2DComponent.h"
#include "TestScriptComponent.h"

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

std::shared_ptr<GameObject> GameObject::CreateGameObject(String name)
{
	std::shared_ptr<GameObject> gameObject = MemoryManager::AllocateNewGameObject();
	new (gameObject.get()) GameObject(name);
	return gameObject;
}

std::shared_ptr<GameObject> GameObject::CreateGameObject(String name, List<ComponentTypeEnum> gameObjectComponentList)
{
	std::shared_ptr<GameObject> gameObject = MemoryManager::AllocateNewGameObject();
	new (gameObject.get()) GameObject(name);

	for (auto component : gameObjectComponentList)
	{
		String asdf = "adsfasd";
		switch (component)
		{
			case kRenderMesh2DComponent: gameObject->AddComponent(RenderMesh2DComponent::CreateRenderMesh2DComponent(gameObject, "Mesh Renderer", static_cast<uint32>(MemoryManager::GetRenderMesh2DComponentList().size()))); break;
			case kTransform2DComponent: gameObject->AddComponent(std::make_shared<Transform2DComponent>(Transform2DComponent(gameObject, asdf)));
			case kTestScriptComponent: gameObject->AddComponent(std::make_shared<TestScriptComponent>(TestScriptComponent(gameObject, asdf)));
		}
	}
	
	
	return gameObject;
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
		component->BufferUpdate(commandBuffer, deltaTime);
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

List<std::shared_ptr<GameObjectComponent>> GameObject::GetGameObjectComponentList()
{
	return GameObjectComponentList;
}

std::shared_ptr<GameObjectComponent> GameObject::GetComponentByName(const std::string& name)
{
	auto component = std::find_if(GameObjectComponentList.begin(), GameObjectComponentList.end(),
		[&name](const std::shared_ptr<GameObjectComponent>& component) {
			return component->Name == name;
		});

	if (component != GameObjectComponentList.end())
	{
		return *component;
	}

	return nullptr;
}

std::shared_ptr<GameObjectComponent> GameObject::GetComponentByComponentType(ComponentTypeEnum type)
{
	auto component = std::find_if(GameObjectComponentList.begin(), GameObjectComponentList.end(),
		[&type](const std::shared_ptr<GameObjectComponent>& component)
		{
			return component->ComponentType == type;
		});

	if (component != GameObjectComponentList.end())
	{
		return *component;
	}

	return nullptr;
}