#pragma once
#include "Typedef.h"

class GameObjectComponent
{
private:
protected:
	String Name;
	size_t MemorySize = 0;
public:

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

	virtual std::shared_ptr<GameObjectComponent> Clone()
	{
		return std::make_shared<GameObjectComponent>(*this);
	}

	virtual size_t GetMemorySize() const
	{
		return sizeof(*this);
	}
};

