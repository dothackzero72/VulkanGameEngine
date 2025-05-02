#pragma once
#include "AssetManager.h"
#include "RenderSystem.h"
#include "InputSystem.h"
#include "InterfaceRenderPass.h"
#include "OrthographicCamera2D.h"


class GameSystem
{
private:
	SceneDataBuffer						SceneProperties;
	SharedPtr<OrthographicCamera2D>		OrthographicCamera;
	Vector<VkCommandBuffer>				CommandBufferSubmitList;

	RenderPassID renderPass2DId;
	RenderPassID frameBufferId;

	void DestroyDeadGameObjects();

public:
	GameSystem();
	~GameSystem();

	void LoadLevel();

	void Input(const float& deltaTime);
	void Update(const float& deltaTime);
	void DebugUpdate(const float& deltaTime);
	void Draw(const float& deltaTime);
};
extern GameSystem gameSystem;
