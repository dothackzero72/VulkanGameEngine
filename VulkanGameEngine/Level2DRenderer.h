#pragma once
#include "JsonRenderPass.h"
#include "SpriteBatchLayer.h"
#include "DepthTexture.h"
#include "RenderedTexture.h"

class Level2DRenderer : public JsonRenderPass
{
private:
	Vector<SpriteMesh> GetMeshFromGameObjects();

public:
	Level2DRenderer();
	Level2DRenderer(const String& JsonPath, ivec2 RenderPassResolution);
	 ~Level2DRenderer();

	void StartLevelRenderer();
	 void Update(const float& deltaTime);
	VkCommandBuffer DrawSprites(Vector<SpriteBatchLayer> meshList, SceneDataBuffer& sceneDataBuffer);
	void UpdateBufferIndex();
	 void Destroy();
};