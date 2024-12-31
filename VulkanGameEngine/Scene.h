#pragma once
#include "InterfaceRenderPass.h"
#include "Vertex.h"
#include "SceneDataBuffer.h"
#include "FrameBufferRenderPass.h"
#include "OrthographicCamera.h"
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
	SceneDataBuffer sceneProperties;
	List<SharedPtr<Texture>> texture;
	SharedPtr<Texture> texture2;
	SharedPtr<OrthographicCamera> orthographicCamera;
	FrameBufferRenderPass frameRenderPass;
	SharedPtr<Level2DRenderer>        levelRenderer;

	List<SharedPtr<GameObject>> gameObjectList;
	List<SharedPtr<Texture>> TextureList;

public:
	void StartUp();
	void Input(float deltaTime);
	void Update(const float& deltaTime);
	void ImGuiUpdate(const float& deltaTime);
	void BuildRenderPasses();
	void UpdateRenderPasses();
	void Draw();
	//void BakeCubeTextureAtlus(const String& FilePath, SharedPtr<BakedTexture> texture);
	void Destroy();
};
