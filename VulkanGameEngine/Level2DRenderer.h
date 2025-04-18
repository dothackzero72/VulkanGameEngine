#pragma once
#include "JsonRenderPass.h"
#include "SpriteBatchLayer.h"

class SpriteBatchLayer;
class Level2DRenderer : public JsonRenderPass
{
private:


	void AddGameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentTypeList, SpriteSheet& spriteSheet, vec2 objectPosition);
	void RemoveGameObject(SharedPtr<GameObject> gameObject);
	void DestroyDeadGameObjects();

	Vector<SharedPtr<Mesh<Vertex2D>>> GetMeshFromGameObjects();

public:
	static SharedPtr<Level2DRenderer>   LevelRenderer;
	Vector<SharedPtr<SpriteBatchLayer>> SpriteLayerList;
	Vector<SharedPtr<GameObject>>		GameObjectList;
	Vector<SharedPtr<Texture>>		    TextureList;

	Level2DRenderer();
	Level2DRenderer(const String& JsonPath, ivec2 RenderPassResolution);
	virtual ~Level2DRenderer();

	void StartLevelRenderer();
	virtual void Input(const float& deltaTime);
	virtual void Update(const float& deltaTime) override;
	VkCommandBuffer DrawSprites(Vector<SharedPtr<SpriteBatchLayer>> meshList, SceneDataBuffer& sceneDataBuffer);
	void UpdateBufferIndex();
	virtual void Destroy() override;

	SharedPtr<GameObject> SearchGameObjectsById(uint32 id);
};