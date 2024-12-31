#pragma once
#include "JsonRenderPass.h"
#include "SpriteBatchLayer.h"

class Level2DRenderer : public JsonRenderPass
{
private:
	List<SharedPtr<SpriteBatchLayer>> SpriteLayerRenderList;

public:
	Level2DRenderer();
	Level2DRenderer(String JsonPath, ivec2 RenderPassResolution);
	virtual ~Level2DRenderer();

	virtual VkCommandBuffer Draw(List<SharedPtr<GameObject>> meshList, SceneDataBuffer& sceneProperties) override;
	virtual void Destroy() override;
};

