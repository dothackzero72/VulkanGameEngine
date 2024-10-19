#pragma once
extern "C"
{
#include <CGameObject.h>
}

#include "Typedef.h"
#include "GameObjectComponent.h"
#include "TestComponent.h"

class GameObject
{
private:
	String Name;
	size_t ObjectComponentMemorySize = 0;
	List<std::shared_ptr<GameObjectComponent>> GameObjectComponentList = List<std::shared_ptr<GameObjectComponent>>();

	void AllocateComponentMemory();
public:
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

	void AddComponent(std::shared_ptr<GameObjectComponent> newComponent);
	void RemoveComponent(size_t index);
};

