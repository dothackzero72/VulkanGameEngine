#pragma once
#include "AssetManager.h"
#include "RenderSystem.h"
#include "InputSystem.h"
#include "OrthographicCamera2D.h"
#include "Level2D.h"

class MeshSystem;
class GameSystem
{
private:
	Vector<VkCommandBuffer>				CommandBufferSubmitList;

	void DestroyDeadGameObjects();

public:
	VkGuid								TileSetId;

	GameSystem();
	~GameSystem();

	void StartUp();
	void Input(const float& deltaTime);
	void Update(const float& deltaTime);
	void DebugUpdate(const float& deltaTime);
	void Draw(const float& deltaTime);
	void Destroy();
};
extern GameSystem gameSystem;
