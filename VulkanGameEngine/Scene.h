#pragma once
#include "InterfaceRenderPass.h"
#include "Vertex.h"
#include "SceneDataBuffer.h"
#include "FrameBufferRenderPass.h"
#include "OrthographicCamera2D.h"
#include "FrameTimer.h"
#include "JsonRenderPass.h"
#include "GameObject.h"
#include "RenderMesh2DComponent.h"
#include "Level2DRenderer.h"

class Scene
{
private:
	std::vector<Vertex2D> SpriteVertexList;

	FrameTimer timer;
	SceneDataBuffer				  sceneProperties;
	SharedPtr<OrthographicCamera2D> orthographicCamera;
	SharedPtr<FrameBufferRenderPass>		  frameRenderPass;
	SharedPtr<Level2DRenderer>    levelRenderer;
	Vector<SharedPtr<GameObject>>   GameObjectList;
	Vector<SharedPtr<Sprite>>		  SpriteList;
	Vector<SharedPtr<Texture>>      TextureList;

	std::vector<VkCommandBuffer> CommandBufferSubmitList;
public:
	void StartUp();
	void Input(float deltaTime);
	void Update(const float& deltaTime);
	void ImGuiUpdate(const float& deltaTime);
	void BuildRenderPasses();
	void UpdateRenderPasses();
	void Draw();
	void Destroy();
};
