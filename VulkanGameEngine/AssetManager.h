#pragma once
#include <vulkan/vulkan_core.h>
#include <queue>
#include "ECSid.h"
#include <VkGuid.h>
#include "Mesh.h"
#include "SceneDataBuffer.h"
#include "GameObject.h"
#include "Transform2DComponent.h"
#include "InputComponent.h"
#include "Animation2D.h"
#include "Sprite.h"
#include "SpriteVRAM.h"
#include "SpriteBatchLayer.h"
#include "LevelTileSet.h"
#include "LevelLayout.h"
#include "Material.h"

typedef uint32 UM_TextureID;
typedef uint32 UM_MaterialID;
typedef uint32 UM_RenderPassID;
typedef uint32 UM_PipelineID;
typedef uint32 UM_SpriteSheetID;
typedef uint32 UM_SpriteVRAMID;
typedef uint32 UM_AnimationFrameId;
typedef uint32 UM_AnimationListID;
typedef VkGuid LevelGuid;
typedef VkGuid RenderPassGuid;

class AssetManager
{
private:
	std::queue<uint32> FreeIds;
	uint32 NextId = 0;

public:

	    Vector<Vertex2D> SpriteVertexList =
    {
        Vertex2D(vec2(0.0f, 1.0f), vec2(0.0f, 0.0f)),
        Vertex2D(vec2(1.0f, 1.0f), vec2(1.0f, 0.0f)),
        Vertex2D(vec2(1.0f, 0.0f), vec2(1.0f, 1.0f)),
        Vertex2D(vec2(0.0f, 0.0f), vec2(0.0f, 1.0f)), 
    };

    Vector<uint32> SpriteIndexList =
    {
        0, 3, 1,
        1, 3, 2
    };

	UnorderedMap<GameObjectID, GameObject> GameObjectList;
	UnorderedMap<GameObjectID, Transform2DComponent> TransformComponentList;
	UnorderedMap<GameObjectID, InputComponent> InputComponentList;
	UnorderedMap<GameObjectID, Sprite> SpriteList;



	LevelLayout                                                   levelLayout;


	UnorderedMap<RenderPassGuid, Material>                        MaterialList;
	UnorderedMap<RenderPassGuid, SpriteVram>                      VramSpriteList;
	UnorderedMap<RenderPassGuid, LevelTileSet>                    LevelTileSetList;

	UnorderedMap<RenderPassGuid, Vector<SpriteBatchLayer>>        SpriteBatchLayerList;

	UnorderedMap<UM_SpriteBatchID, int>          SpriteInstanceBufferList;
	UnorderedMap<UM_SpriteBatchID, Vector<GameObjectID>>          SpriteBatchLayerObjectList;
	UnorderedMap<UM_SpriteBatchID, Vector<SpriteInstanceStruct>>  SpriteInstanceList;

	UnorderedMap<UM_AnimationListID, Animation2D> AnimationList;
	UnorderedMap<VkGuid, Vector<Vector<ivec2>>> AnimationFrameList;

	AssetManager();
	~AssetManager();

	void Input(const float& deltaTime);
	void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);

	//CreateGameObject();
	void CreateGameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentTypeList, VkGuid vramId, vec2 objectPosition);
	void CreateGameObject(const String& gameObjectPath, const vec2& gameObjectPosition);
	void AddTransformComponent(const nlohmann::json& json, GameObjectID id, const vec2& gameObjectPosition);
	void AddInputComponent(const nlohmann::json& json, GameObjectID id);
	void AddSpriteComponent(const nlohmann::json& json, GameObjectID id);
	void DestroyEntity(RenderPassID id);

	VkGuid AddSpriteVRAM(const String& spritePath);
	VkGuid AddTileSetVRAM(const String& tileSetPath);
	VkGuid LoadMaterial(const String& materialPath);

	void DestroyGameObject(GameObjectID id);
	void DestroyGameObjects();
	void DestoryTextures();
	void DestoryMaterials();
	void DestoryVRAMSprites();
	void Destroy();
};
extern AssetManager assetManager;