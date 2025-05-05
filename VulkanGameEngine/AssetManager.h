#pragma once
#include <cstdint>
#include <queue>
#include "TypeDef.h"
#include "VkGuid.h"
#include "GameObject.h"
#include "Transform2DComponent.h"
#include "Mesh.h"
#include "SpriteVRAM.h"
#include "Sprite.h"
#include "ECGid.h"
#include "SceneDataBuffer.h"
#include "InputComponent.h"

typedef uint32 UM_TextureID;
typedef uint32 UM_MaterialID;
typedef uint32 UM_RenderPassID;
typedef uint32 UM_PipelineID;
typedef uint32 UM_SpriteSheetID;
typedef uint32 UM_SpriteVRAMID;
typedef uint32 UM_AnimationFrameId;
typedef uint32 UM_AnimationListID;

class AssetManager
{
private:
	std::queue<uint32> FreeIds;
	uint32 NextId = 0;

public:
	UnorderedMap<GameObjectID, GameObject> GameObjectList;
	UnorderedMap<GameObjectID, Transform2DComponent> TransformComponentList;
	UnorderedMap<GameObjectID, InputComponent> InputComponentList;
	UnorderedMap<GameObjectID, Sprite> SpriteList;
	//UnorderedMap<GameObjectID, SpriteMesh> MeshList;

	UnorderedMap<UM_AnimationListID, Animation2D> AnimationList;
	UnorderedMap<UM_AnimationFrameId, Vector<ivec2>> AnimationFrameList;

	AssetManager();
	~AssetManager();

	void Input(const float& deltaTime);
	void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);

	//CreateGameObject();
	void CreateGameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentTypeList, VkGuid vramId, vec2 objectPosition);
	void DestroyEntity(RenderPassID id);

	void DestroyGameObject(GameObjectID id);
	void DestroyGameObjects();
	void DestoryTextures();
	void DestoryMaterials();
	void DestoryVRAMSprites();
	void Destroy();
};
extern AssetManager assetManager;