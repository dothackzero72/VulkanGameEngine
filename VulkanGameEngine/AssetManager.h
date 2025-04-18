#pragma once
#include <cstdint>
#include <queue>
#include <TypeDef.h>
#include "VulkanRenderer.h"
#include "Mesh2D.h"
#include "SpriteComponent.h"
#include "JsonPipeline.h"

typedef uint32 UM_GameObjectID;
typedef uint32 UM_TextureID;
typedef uint32 UM_MaterialID;
typedef uint32 UM_RenderPassID;
typedef uint32 UM_PipelineID;

class Sprite;
class AssetManager
{
private:
	std::queue<uint32> FreeIds;
	uint32 NextId = 0;

public:
	Vector<UM_GameObjectID> GameObjectList;
	UnorderedMap<UM_GameObjectID, Transform2DComponent> TransformComponentList;
	//UnorderedMap<UM_GameObjectID, SpriteComponent> SpriteComponentList;
	UnorderedMap<UM_GameObjectID, Sprite> SpriteList;
	UnorderedMap<UM_TextureID, Texture> TextureList;
	UnorderedMap<UM_MaterialID, Material> MaterialList;
	//UnorderedMap<UM_GameObjectID, Mesh2D> MeshList;
	UnorderedMap<UM_GameObjectID, Animation2D> Animation2DList;
	UnorderedMap<UM_GameObjectID, SpriteSheet> SpriteSheetList;

	AssetManager();
	~AssetManager();

	void Input(const float& deltaTime);
	void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
	void Destroy();

	void CreateEntity();
	void DestroyEntity(uint32_t id);
};
extern AssetManager assetManager;