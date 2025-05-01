#pragma once
#include "InterfaceRenderPass.h"
#include "Vertex.h"
#include "SceneDataBuffer.h"
#include "OrthographicCamera2D.h"
#include "JsonRenderPass.h"
#include "GameObject.h"

class Scene
{
private:
	std::vector<Vertex2D> SpriteVertexList;

	SceneDataBuffer				  sceneProperties;
	SharedPtr<OrthographicCamera2D> orthographicCamera;
	RenderPassID renderPass2DId;
	RenderPassID frameBufferId;
	std::vector<VkCommandBuffer> CommandBufferSubmitList;
public:
	void StartUp();
	void Input(float deltaTime);
	void Update(const float& deltaTime);
	void ImGuiUpdate(const float& deltaTime);
	void BuildRenderPasses();
	void UpdateRenderPasses();
	void Draw(const float& deltaTime);
	void Destroy();
};
