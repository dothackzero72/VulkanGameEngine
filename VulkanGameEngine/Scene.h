#pragma once
#include "InterfaceRenderPass.h"
#include "Vertex.h"
#include "SceneDataBuffer.h"
#include "FrameBufferRenderPass.h"
#include "RenderPass2D.h"
#include "OrthographicCamera.h"
#include "FrameTimer.h"
#include "JsonRenderPass.h"
#include "GameObject.h"
#include "RenderMesh2DComponent.h"

class Scene
{
private:
	std::vector<Vertex2D> SpriteVertexList;

	FrameTimer timer;
	SceneDataBuffer sceneProperties;
	List<std::shared_ptr<Texture>> texture;
	std::shared_ptr<Texture> texture2;
	std::shared_ptr<OrthographicCamera> orthographicCamera;
	FrameBufferRenderPass frameRenderPass;
	//RenderPass2D		  renderPass2D;
	std::shared_ptr<JsonRenderPass>        renderPass2D;

	List<std::shared_ptr<GameObject>> gameObjectList;
	List<std::shared_ptr<Texture>> TextureList;

public:
	void StartUp();
	void Update(const float& deltaTime);
	void ImGuiUpdate(const float& deltaTime);
	void BuildRenderPasses();
	void UpdateRenderPasses();
	void Draw();
	//void BakeCubeTextureAtlus(const String& FilePath, std::shared_ptr<BakedTexture> texture);
	void Destroy();
};
