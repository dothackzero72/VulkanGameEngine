#pragma once
#include "JsonRenderPass.h"
#include "SpriteBatchLayer.h"
#include "Level2DRenderer.h"

class Level2DRenderer : public JsonRenderPass2
{
private:


	Vector<SpriteMesh> GetMeshFromGameObjects();

public:
	uint RenderPassId = 0;

	Level2DRenderer();
	Level2DRenderer(const String& JsonPath, ivec2 RenderPassResolution);
	virtual ~Level2DRenderer();

	void StartLevelRenderer();
	virtual void Update(const float& deltaTime) override;
	VkCommandBuffer DrawSprites(Vector<SpriteBatchLayer> meshList, SceneDataBuffer& sceneDataBuffer);
	void UpdateBufferIndex();
	virtual void Destroy() override;
};