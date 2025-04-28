#pragma once
#include "JsonRenderPass.h"
#include "SpriteBatchLayer.h"
#include "Level2DRenderer.h"

class Level2DRenderer : public JsonRenderPass
{
private:


	void AddGameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentTypeList, VkGuid vramId, vec2 objectPosition);
	void RemoveGameObject(SharedPtr<GameObject> gameObject);
	void DestroyDeadGameObjects();

	Vector<SpriteMesh> GetMeshFromGameObjects();

public:
	static SharedPtr<Level2DRenderer>   LevelRenderer;
	Vector<SpriteBatchLayer> SpriteLayerList;
	Vector<Texture>						TextureList;
	Vector<GameObject>					GameObjectList;

	Level2DRenderer();
	Level2DRenderer(const String& JsonPath, ivec2 RenderPassResolution);
	virtual ~Level2DRenderer();

	void StartLevelRenderer();
	virtual void Input(const float& deltaTime);
	virtual void Update(const float& deltaTime) override;
	VkCommandBuffer DrawSprites(Vector<SpriteBatchLayer> meshList, SceneDataBuffer& sceneDataBuffer);
	void UpdateBufferIndex();
	virtual void Destroy() override;
};