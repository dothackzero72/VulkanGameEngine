#pragma once
#include "Sprite.h"
#include "SpriteBatchLayer.h"
#include "Level2D.h"
#include "LevelLayout.h"
#include "Animation2D.h"
#include "RenderSystem.h"
#include "SpriteVRAM.h"
#include "OrthographicCamera2D.h"

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
	UnorderedMap<RenderPassGuid, LevelTileSet>                    LevelTileSetList;

	UnorderedMap<GameObjectID, Sprite>							  SpriteList;
	UnorderedMap<RenderPassGuid, SpriteVram>                      VramSpriteList;
	UnorderedMap<UM_SpriteBatchID, Vector<SpriteInstanceStruct>>  SpriteInstanceList;
	UnorderedMap<RenderPassGuid, Vector<SpriteBatchLayer>>        SpriteBatchLayerList;
	UnorderedMap<UM_SpriteBatchID, int>							  SpriteInstanceBufferList;
	UnorderedMap<UM_SpriteBatchID, Vector<GameObjectID>>          SpriteBatchLayerObjectList;

	UnorderedMap<UM_AnimationListID, Animation2D>				  AnimationList;
	UnorderedMap<VkGuid, Vector<Vector<ivec2>>>					  AnimationFrameList;

	LevelSystem();
	~LevelSystem();
	void Update(const float& deltaTime);
	void Draw(Vector<VkCommandBuffer>& commandBufferList, const float& deltaTime);

	void   LoadLevel(const String& levelPath);
};
extern LevelSystem levelSystem;

