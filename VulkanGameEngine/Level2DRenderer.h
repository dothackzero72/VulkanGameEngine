#pragma once
#include "JsonRenderPass.h"
#include "SpriteBatchLayer.h"

class Level2DRenderer : public JsonRenderPass
{
private:
	Vector<SharedPtr<SpriteBatchLayer>> SpriteLayerRenderList;
	Vector<SharedPtr<GameObject>>		  GameObjectList;
	Vector<SharedPtr<Sprite>>			  SpriteList;
	Vector<SharedPtr<Texture>>		  TextureList;
	Vector<SharedPtr<Material>>		  MaterialList;

public:
	Level2DRenderer();
	Level2DRenderer(String JsonPath, ivec2 RenderPassResolution);
	virtual ~Level2DRenderer();

	virtual void Input(const float& deltaTime);
	virtual void Update(const float& deltaTime) override;
	virtual VkCommandBuffer Draw(Vector<SharedPtr<GameObject>> meshList, SceneDataBuffer& sceneProperties) override;
	virtual void Destroy() override;
};

