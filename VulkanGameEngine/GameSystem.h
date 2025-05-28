#pragma once
#include "AssetManager.h"
#include "RenderSystem.h"
#include "InputSystem.h"
#include "OrthographicCamera2D.h"
#include "Level2D.h"

class GameSystem
{
private:
	Vector<VkCommandBuffer>				CommandBufferSubmitList;

	void DestroyDeadGameObjects();

public:
	SceneDataBuffer						SceneProperties;
	Level2D								Level;
	SharedPtr<OrthographicCamera2D>		OrthographicCamera;

	VkGuid								TileSetId;
	RenderPassGuid						levelRenderPass2DId;
	RenderPassGuid						spriteRenderPass2DId;
	RenderPassGuid   					frameBufferId;

	GameSystem();
	~GameSystem();

	void LoadLevel(const String& levelPath);

	void StartUp();
	void Input(const float& deltaTime);
	void Update(const float& deltaTime);
	void DebugUpdate(const float& deltaTime);
	void Draw(const float& deltaTime);
	void Destroy();
};
extern GameSystem gameSystem;
