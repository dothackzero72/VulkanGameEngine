#pragma once
#include <cstdint>
#include <queue>
#include <TypeDef.h>
#include "VulkanRenderer.h"
#include "Mesh2D.h"
#include "SpriteComponent.h"
#include "JsonPipeline.h"

typedef uint32 GameObjectID;
typedef uint32 TextureID;
typedef uint32 MaterialID;

class Sprite;
class AssetManager
{
private:
	std::queue<uint32> FreeIds;
	uint32 NextId = 0;

public:
	Vector<GameObjectID> GameObjectList;
	UnorderedMap<GameObjectID, Transform2DComponent> TransformComponentList;
	//UnorderedMap<GameObjectID, SpriteComponent> SpriteComponentList;
	UnorderedMap<GameObjectID, Sprite> SpriteList;
	UnorderedMap<TextureID, Texture> TextureList;
	UnorderedMap<MaterialID, Material> MaterialList;
	//UnorderedMap<GameObjectID, Mesh2D> MeshList;
	UnorderedMap<GameObjectID, Animation2D> Animation2DList;
	UnorderedMap<GameObjectID, SpriteSheet> SpriteSheetList;

	AssetManager();
	~AssetManager();

	void Input(const float& deltaTime);
	void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
	void Destroy();

	void CreateEntity();
	void DestroyEntity(uint32_t id);
};
extern AssetManager assetManager;