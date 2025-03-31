#include "GameObject.h"
#include <Macro.h>
#include <iostream>
#include "MemoryManager.h"
#include "RenderMesh2DComponent.h"
#include "Transform2DComponent.h"
#include "InputComponent.h"
#include "Sprite.h"

uint32 GameObject::NextGameObjectId = 0;

GameObject::GameObject()
{
}

GameObject::GameObject(const String& name)
{
	GameObjectId = ++NextGameObjectId;
	Name = name;

	//CSclass = std::make_shared<Coral::Type>(MemoryManager::GetECSassemblyModule()->GetType(NameSpace));
//	CSobject = std::make_shared<Coral::ManagedObject>(CSclass->CreateInstance());
}

GameObject::GameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentList)
{
	GameObjectId = ++NextGameObjectId;
	Name = name;
	//String asdf = "adsfasd";
	//for (auto component : gameObjectComponentList)
	//{

	//	switch (component)
	//	{
	//	case kTransform2DComponent: AddComponent(std::make_shared<Transform2DComponent>(Transform2DComponent(this, asdf))); break;
	//	case kInputComponent: AddComponent(std::make_shared<InputComponent>(InputComponent(this, asdf))); break;
	//	}
	//}

//	CSclass = std::make_shared<Coral::Type>(MemoryManager::GetECSassemblyModule()->GetType(NameSpace));
//	CSobject = std::make_shared<Coral::ManagedObject>(CSclass->CreateInstance());
}

GameObject::GameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentList, SpriteSheet& spriteSheet)
{
	GameObjectId = ++NextGameObjectId;
	Name = name;
	//String asdf = "adsfasd";
	//for (auto component : gameObjectComponentList)
	//{
	//	switch (component)
	//	{
	//	case kTransform2DComponent: AddComponent(std::make_shared<Transform2DComponent>(Transform2DComponent(this, asdf))); break;
	//	case kInputComponent: AddComponent(std::make_shared<InputComponent>(InputComponent(this, asdf))); break;
	//	}
	//}

	//AddComponent(std::make_shared<SpriteComponent>(SpriteComponent(asdf, this, spriteSheet)));
}

GameObject::GameObject(const String& name, const Vector<GameObjectComponent>& gameObjectComponentList, SpriteSheet& spriteSheet)
{
	GameObjectId = ++NextGameObjectId;
	Name = name;
	//String asdf = "adsfasd";
	//for (auto component : gameObjectComponentList)
	//{
	//	switch (component)
	//	{
	//	case kTransform2DComponent: AddComponent(std::make_shared<Transform2DComponent>(Transform2DComponent(this, asdf))); break;
	//	case kInputComponent: AddComponent(std::make_shared<InputComponent>(InputComponent(this, asdf))); break;
	//	}
	//}

	//AddComponent(std::make_shared<SpriteComponent>(SpriteComponent(asdf, this, spriteSheet)));
}

void GameObject::Input(const float& deltaTime)
{
	for (SharedPtr<GameObjectComponent> component : GameObjectComponentList)
	{
		component->Input(deltaTime);
	}
}

void GameObject::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
	for (SharedPtr<GameObjectComponent> component : GameObjectComponentList)
	{
		component->Update(commandBuffer, deltaTime);
	}
}

void GameObject::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, Vector<VkDescriptorSet>& descriptorSetList)
{
	for (SharedPtr<GameObjectComponent> component : GameObjectComponentList)
	{
		component->Draw(commandBuffer, pipeline, shaderPipelineLayout, descriptorSetList);
	}
}

void GameObject::Destroy()
{
	for (auto& component : GameObjectComponentList)
	{
		component->Destroy();
		component.reset();
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
			return component->ComponentType == type;
		});

	if (component != GameObjectComponentList.end())
	{
		return *component;
	}

	return nullptr;
}