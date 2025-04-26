#pragma once
#include <cstdint>
#include <queue>
#include <TypeDef.h>
#include <VkGuid.h>
#include "GameObject.h"
#include "Transform2DComponent.h"
#include "Mesh.h"
#include "SpriteVRAM.h"
#include "Sprite.h"

typedef uint32 UM_GameObjectID;
typedef uint32 UM_TextureID;
typedef uint32 UM_MaterialID;
typedef uint32 UM_RenderPassID;
typedef uint32 UM_PipelineID;
typedef uint32 UM_SpriteSheetID;
typedef uint32 UM_SpriteVRAMID;

class AssetManager
{
private:
	std::queue<uint32> FreeIds;
	uint32 NextId = 0;

	GUID GetGUID(nlohmann::json& json);

public:
	UnorderedMap<UM_GameObjectID, GameObject> GameObjectList;
	UnorderedMap<UM_GameObjectID, Transform2DComponent> TransformComponentList;
	UnorderedMap<UM_GameObjectID, Sprite> SpriteList;
	UnorderedMap<UM_GameObjectID, SpriteMesh> MeshList;

	UnorderedMap<UM_TextureID, Texture> TextureList;
	UnorderedMap<UM_MaterialID, Material> MaterialList;
	UnorderedMap<UM_SpriteVRAMID, SpriteVRAM> VRAMSpriteList;

	AssetManager();
	~AssetManager();

	void Input(const float& deltaTime);
	void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);

	void CreateEntity();
	void DestroyEntity(uint32_t id);

	UM_SpriteVRAMID AddSpriteVRAM(const String& spritePath);
	UM_TextureID LoadTexture(const String& texturePath);
	UM_MaterialID LoadMaterial(const String& materialPath);

	void DestroyGameObject(UM_GameObjectID id);
	void DestroyGameObjects();
	void DestoryTextures();
	void DestoryMaterials();
	void DestoryVRAMSprites();
	void Destroy();
};
extern AssetManager assetManager;