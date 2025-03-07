#pragma once
#include "JsonRenderPass.h"
#include "SpriteBatchLayer.h"

class SpriteBatchLayer;
class Level2DRenderer : public JsonRenderPass
{
private:
	Vector<SharedPtr<SpriteBatchLayer>> SpriteLayerList;
	Vector<SharedPtr<GameObject>>		GameObjectList;
	Vector<SharedPtr<Texture>>		    TextureList;
	Vector<SharedPtr<Material>>		    MaterialList;

	void AddGameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentTypeList, SpriteSheet& spriteSheet, vec2 objectPosition);
	void AddTexture();
	void AddMaterial();
	void RemoveGameObject(SharedPtr<GameObject> gameObject);
	void DestroyDeadGameObjects();

	Vector<SharedPtr<Mesh<Vertex2D>>> GetMeshFromGameObjects();

public:
	static SharedPtr<Level2DRenderer>   LevelRenderer;
	Level2DRenderer();
	Level2DRenderer(const String& JsonPath, ivec2 RenderPassResolution);
	virtual ~Level2DRenderer();

	void StartLevelRenderer();
	virtual void Input(const float& deltaTime);
	virtual void Update(const float& deltaTime) override;
	void UpdateBufferIndex();
	virtual VkCommandBuffer Draw(Vector<SharedPtr<GameObject>> meshList) override;
	virtual void Destroy() override;

	void SetRendererRefForSprites(std::shared_ptr<Level2DRenderer> self);
	SharedPtr<GameObject> SearchGameObjectsById(uint32 id);
};

