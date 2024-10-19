#include "GameObject.h"
#include <Macro.h>

GameObject::GameObject()
{
}

GameObject::GameObject(String name)
{
	Name = name;
	GameObjectComponentList.emplace_back(std::make_shared<TestComponent>(TestComponent("test")));
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

	void* memoryBlock = malloc(ObjectComponentMemorySize);

	void* ptr = memoryBlock;
	std::vector<GameObjectComponent*> components;
    for (size_t x = 0; x < GameObjectComponentList.size(); x++)
    {
        if (GameObjectComponentList[x] == nullptr)
        {
            continue;
        }

        std::shared_ptr<GameObjectComponent> clonedComponent = GameObjectComponentList[x]->Clone();
        if (!clonedComponent)
        {
            continue;
        }
        size_t componentSize = clonedComponent->GetMemorySize();

        std::memcpy(ptr, clonedComponent.get(), componentSize);
        GameObjectComponentList[x] = std::shared_ptr<GameObjectComponent>(static_cast<GameObjectComponent*>(ptr), [](GameObjectComponent* p) { delete p; } );
        ptr = static_cast<void*>(static_cast<byte*>(ptr) + componentSize);
    }
}

void GameObject::AddComponent(std::shared_ptr<GameObjectComponent> newComponent) 
{
    if (newComponent) 
	{
        GameObjectComponentList.emplace_back(newComponent);
        AllocateComponentMemory();
    }
}

void GameObject::RemoveComponent(size_t index)
{
	GameObjectComponentList.erase(GameObjectComponentList.begin() + index);
	AllocateComponentMemory();
}