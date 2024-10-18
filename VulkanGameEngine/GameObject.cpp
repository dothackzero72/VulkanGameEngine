#include "GameObject.h"

GameObject::GameObject()
{
	GameObjectComponentList.emplace_back(std::make_shared<TestComponent>(TestComponent("test")));
	GameObjectComponentList.emplace_back(std::make_shared<TestComponent>(TestComponent("test2")));
	GameObjectComponentList.emplace_back(std::make_shared<TestComponent>(TestComponent("test3")));
	AllocateComponentMemory();
}

GameObject::GameObject(String name)
{
	Name = name;
	//auto a = List<std::unique_ptr<TestComponent>>();
	GameObjectComponentList.emplace_back(std::make_shared<TestComponent>(TestComponent("test")));
	GameObjectComponentList.emplace_back(std::make_shared<TestComponent>(TestComponent("test2")));
	GameObjectComponentList.emplace_back(std::make_shared<TestComponent>(TestComponent("test3")));
	AllocateComponentMemory();
}

GameObject::GameObject(String name, List<std::shared_ptr<GameObjectComponent>> gameObjectComponentList)
{
	Name = name;
	GameObjectComponentList = gameObjectComponentList;
	AllocateComponentMemory();
}

void GameObject::AllocateComponentMemory()
{
	size_t totalComponentMemorySize = 0;
	for (std::shared_ptr<GameObjectComponent> component : GameObjectComponentList)
	{
		totalComponentMemorySize += component->GetMemorySize();
	}
	ObjectComponentMemorySize = totalComponentMemorySize;


}