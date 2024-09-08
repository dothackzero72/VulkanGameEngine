#pragma once
#include "InterfaceRenderPass.h"
#include "Vertex.h"
#include "SceneDataBuffer.h"
#include "FrameBufferRenderPass.h"
#include "RenderPass2D.h"
#include "OrthographicCamera.h"


class Scene
{
	private:
		std::vector<Vertex2D> SpriteVertexList;

		//Timer timer;
		SceneDataBuffer sceneProperties;
		std::shared_ptr<Texture> texture;
		std::shared_ptr<Mesh2D> mesh;
		std::shared_ptr<OrthographicCamera> orthographicCamera;
		FrameBufferRenderPass frameRenderPass;
		RenderPass2D		  renderPass2D;
	
	public:
		void StartUp();
		void Update();
		void ImGuiUpdate();
		void BuildRenderPasses();
		void UpdateRenderPasses();
		void Draw();
		void Destroy();
};

