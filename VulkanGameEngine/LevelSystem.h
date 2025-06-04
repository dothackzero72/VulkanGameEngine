#pragma once
#include "Sprite.h"
#include "SpriteBatchLayer.h"
#include "Level2D.h"
#include "LevelLayout.h"
#include "Animation2D.h"
#include "RenderSystem.h"
#include "SpriteVRAM.h"
#include "OrthographicCamera2D.h"

typedef Vector<vec2> AnimationFrames;

class MeshSystem;
class LevelSystem
{
private:
	RenderPassGuid						levelRenderPass2DId;
	RenderPassGuid						spriteRenderPass2DId;
	RenderPassGuid   					frameBufferId;

	VkGuid LoadSpriteVRAM(const String& spritePath);
	VkGuid LoadTileSetVRAM(const String& tileSetPath);
	VkGuid LoadLevelLayout(const String& levelLayoutPath);

	void DestroyDeadGameObjects();

public:
	SceneDataBuffer												  SceneProperties;
	SharedPtr<OrthographicCamera2D>								  OrthographicCamera;

	Level2D														  Level;
	LevelLayout                                                   levelLayout;

	UnorderedMap<GameObjectID, Sprite>							  SpriteMap;
	UnorderedMap<RenderPassGuid, LevelTileSet>                    LevelTileSetMap;
	UnorderedMap<RenderPassGuid, SpriteVram>                      VramSpriteMap;
	UnorderedMap<RenderPassGuid, Vector<SpriteBatchLayer>>        SpriteBatchLayerListMap;
	UnorderedMap<UM_SpriteBatchID, Vector<SpriteInstanceStruct>>  SpriteInstanceListMap;
	UnorderedMap<UM_SpriteBatchID, int>							  SpriteInstanceBufferMap;
	UnorderedMap<UM_SpriteBatchID, Vector<GameObjectID>>          SpriteBatchLayerObjectListMap;

	UnorderedMap<UM_AnimationListID, Animation2D>				  AnimationMap;
	UnorderedMap<VkGuid, Vector<AnimationFrames>>			      AnimationFrameListMap;

	LevelSystem();
	~LevelSystem();
	void Update(const float& deltaTime);
	void Draw(Vector<VkCommandBuffer>& commandBufferList, const float& deltaTime);

	void LoadLevel(const String& levelPath);
	void DestoryLevel();
};
extern LevelSystem levelSystem;

