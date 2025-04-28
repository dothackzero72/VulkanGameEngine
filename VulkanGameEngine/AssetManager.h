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

public:
	UnorderedMap<UM_GameObjectID, GameObject> GameObjectList;
	UnorderedMap<UM_GameObjectID, Transform2DComponent> TransformComponentList;
	UnorderedMap<UM_GameObjectID, Sprite> SpriteList;
	UnorderedMap<UM_GameObjectID, SpriteMesh> MeshList;

	UnorderedMap<VkGuid, Texture> TextureList;
	UnorderedMap<VkGuid, Material> MaterialList;
	UnorderedMap<VkGuid, SpriteVram> VramSpriteList;

	UnorderedMap<uint, Animation2D> AnimationList;
	UnorderedMap<uint, Vector<ivec2>> AnimationFrameList;

	AssetManager();
	~AssetManager();

	void Input(const float& deltaTime);
	void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);

	void CreateEntity();
	void DestroyEntity(uint32_t id);

	VkGuid AddSpriteVRAM(const String& spritePath);
	VkGuid LoadTexture(const String& texturePath);
	VkGuid LoadMaterial(const String& materialPath);

	void DestroyGameObject(UM_GameObjectID id);
	void DestroyGameObjects();
	void DestoryTextures();
	void DestoryMaterials();
	void DestoryVRAMSprites();
	void Destroy();
};
extern AssetManager assetManager;