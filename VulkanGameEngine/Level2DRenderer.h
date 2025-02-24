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

	Level2DRenderer(const String& JsonPath, ivec2 RenderPassResolution);
	void AddGameObject(const String& name, Vector<ComponentTypeEnum> gameObjectComponentList, SpriteSheet& spriteSheet, vec2 objectPosition);
	void AddTexture();
	void AddMaterial();
	void RemoveGameObject(SharedPtr<GameObject> gameObject);
	void DestroyDeadGameObjects();

	Vector<SharedPtr<Mesh<Vertex2D>>> GetMeshFromGameObjects();

public:
	static SharedPtr<Level2DRenderer>   LevelRenderer;
	Level2DRenderer();
	virtual ~Level2DRenderer();

	virtual void Input(const float& deltaTime);
	virtual void Update(const float& deltaTime) override;
	void UpdateBufferIndex();
	virtual VkCommandBuffer Draw(Vector<SharedPtr<GameObject>> meshList, SceneDataBuffer& sceneProperties) override;
	virtual void Destroy() override;

	void SetRendererRefForSprites(std::shared_ptr<Level2DRenderer> self);
	SharedPtr<GameObject> SeachGameObjectsById(uint32 id);

	static SharedPtr<Level2DRenderer> CreateLevel2DRenderer(const String& jsonPath, ivec2 renderPassResolution)
	{
		SharedPtr<Level2DRenderer> gameObject = std::make_shared<Level2DRenderer>(Level2DRenderer());
		LevelRenderer = gameObject;
		new (gameObject.get()) Level2DRenderer(jsonPath, renderPassResolution);

		return LevelRenderer;
	}
};

