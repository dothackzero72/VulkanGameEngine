#pragma once
#include "AssetManager.h"
#include "RenderSystem.h"

class GameSystem
{
private:

	void DestroyDeadGameObjects();

public:
	GameSystem();
	~GameSystem();

	void Update(const float& deltaTime);
};

