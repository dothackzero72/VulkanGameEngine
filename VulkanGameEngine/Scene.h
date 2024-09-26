#pragma once
#include "InterfaceRenderPass.h"
#include "Vertex.h"
#include "SceneDataBuffer.h"
#include "FrameBufferRenderPass.h"
#include "RenderPass2D.h"
#include "OrthographicCamera.h"
#include "FrameTimer.h"


class Scene
{
	private:
		std::vector<Vertex2D> SpriteVertexList;

		FrameTimer timer;
		SceneDataBuffer sceneProperties;
		std::shared_ptr<BakedTexture> texture;
		std::shared_ptr<Mesh2D> mesh;
		std::shared_ptr<OrthographicCamera> orthographicCamera;
		FrameBufferRenderPass frameRenderPass;
		RenderPass2D		  renderPass2D;
	
	public:
		void StartUp();
		void Update(const float& deltaTime);
		void ImGuiUpdate(const float& deltaTime);
		void BuildRenderPasses();
		void UpdateRenderPasses();
		void Draw();
		void BakeCubeTextureAtlus(const std::string& FilePath, std::shared_ptr<BakedTexture> texture);
		void Destroy();
};

