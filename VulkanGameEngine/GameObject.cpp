#include "GameObject.h"
#include <Macro.h>
#include <iostream>
#include "MemoryManager.h"
#include "RenderMesh2DComponent.h"
#include "Transform2DComponent.h"
#include "InputComponent.h"
#include "Sprite.h"

GameObject::GameObject()
{
}

GameObject::GameObject(String name)
{
	Name = name;

	CSclass = std::make_shared<Coral::Type>(MemoryManager::GetECSassemblyModule()->GetType(NameSpace));
	CSobject = std::make_shared<Coral::ManagedObject>(CSclass->CreateInstance());
}

GameObject::GameObject(String name, Vector<ComponentTypeEnum> gameObjectComponentList)
{
	Name = name;
	for (auto component : gameObjectComponentList)
	{
		String asdf = "adsfasd";
		switch (component)
		{
			//case kTransform2DComponent: AddComponent(std::make_shared<Transform2DComponent>(Transform2DComponent(std::make_shared<GameObject>(this), asdf))); break;
			//case kInputComponent: AddComponent(std::make_shared<InputComponent>(InputComponent(std::make_shared<GameObject>(this), asdf))); break;
		}
	}

	CSclass = std::make_shared<Coral::Type>(MemoryManager::GetECSassemblyModule()->GetType(NameSpace));
	CSobject = std::make_shared<Coral::ManagedObject>(CSclass->CreateInstance());
}

GameObject::GameObject(String name, Vector<ComponentTypeEnum> gameObjectComponentList, SharedPtr<Material> spriteMaterial, const uint& DrawLayer)
{
	String asdf = "adsfasd";
	for (auto component : gameObjectComponentList)
	{
		switch (component)
		{
		/*	case kTransform2DComponent: AddComponent(std::make_shared<Transform2DComponent>(Transform2DComponent(std::make_shared<GameObject>(this), asdf))); break;
			case kInputComponent: AddComponent(std::make_shared<InputComponent>(InputComponent(std::make_shared<GameObject>(this), asdf))); break;*/
		}
	}
	
	//AddComponent(SpriteComponent::CreateSpriteComponent(asdf, std::make_shared<GameObject>(this), spriteMaterial, DrawLayer));
}

void GameObject::Input(float deltaTime)
{
	for (SharedPtr<GameObjectComponent> component : GameObjectComponentList)
	{
		component->Input(deltaTime);
	}
}

void GameObject::Update(float deltaTime)
{
	for (SharedPtr<GameObjectComponent> component : GameObjectComponentList)
	{
		component->Update(deltaTime);
	}
}

void GameObject::BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
{
	for (SharedPtr<GameObjectComponent> component : GameObjectComponentList)
	{
		component->BufferUpdate(commandBuffer, deltaTime);
	}
}

void GameObject::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
	for (SharedPtr<GameObjectComponent> component : GameObjectComponentList)
	{
		component->Draw(commandBuffer, pipeline, shaderPipelineLayout, descriptorSet, sceneProperties);
	}
}

void GameObject::Destroy()
{
	for (SharedPtr<GameObjectComponent> component : GameObjectComponentList)
	{
		component->Destroy();
	}
	CSclass.reset();
	CSobject.reset();
	GameObjectComponentList.clear();
}

void GameObject::AddComponent(SharedPtr<GameObjectComponent> newComponent)
{
	//auto a = newComponent.get();
	//CSobject->InvokeMethod("AddComponent", newComponent->GetCSObjectHandle(), a);
	GameObjectComponentList.emplace_back(newComponent);
}

void GameObject::RemoveComponent(size_t index)
{
	GameObjectComponentList.erase(GameObjectComponentList.begin() + index);
}

Vector<SharedPtr<GameObjectComponent>> GameObject::GetGameObjectComponentList()
{
	return GameObjectComponentList;
}

SharedPtr<GameObjectComponent> GameObject::GetComponentByName(const std::string& name)
{
	auto component = std::find_if(GameObjectComponentList.begin(), GameObjectComponentList.end(),
		[&name](const SharedPtr<GameObjectComponent>& component)
		{
			return *component->Name.get() == name;
		});

	if (component != GameObjectComponentList.end())
	{
		return *component;
	}

	return nullptr;
}

SharedPtr<GameObjectComponent> GameObject::GetComponentByComponentType(ComponentTypeEnum type)
{
	auto component = std::find_if(GameObjectComponentList.begin(), GameObjectComponentList.end(),
		[&type](const SharedPtr<GameObjectComponent>& component)
		{
			return *component->ComponentType.get() == type;
		});

	if (component != GameObjectComponentList.end())
	{
		return *component;
	}

	return nullptr;
}