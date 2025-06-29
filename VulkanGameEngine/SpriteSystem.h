#pragma once
#include "ECSid.h"
#include "Sprite.h"
#include <VRAM.h>
#include "SpriteBatchLayer.h"

class SpriteSystem
{
private:
	Vector<Sprite>												  SpriteList;
	Vector<SpriteInstanceStruct>								  SpriteInstanceList;
	UnorderedMap<GameObjectID, size_t>							  SpriteIdToListIndexMap;
	UnorderedMap<UM_SpriteBatchID, int>							  SpriteInstanceBufferIdMap;

	UnorderedMap<RenderPassGuid, SpriteVram>                      SpriteVramMap;
	UnorderedMap<UM_AnimationListID, Animation2D>				  SpriteAnimationMap;
	UnorderedMap<VkGuid, Vector<AnimationFrames>>			      SpriteAnimationFrameListMap;

	UnorderedMap<UM_SpriteBatchID, Vector<SpriteInstanceStruct>>  SpriteInstanceListMap;
	UnorderedMap<UM_SpriteBatchID, Vector<GameObjectID>>          SpriteBatchObjectListMap;

public:

	SpriteSystem();
	~SpriteSystem();

	UnorderedMap<RenderPassGuid, Vector<SpriteBatchLayer>>        SpriteBatchLayerMap;

	void AddSprite(GameObjectID gameObjectId, VkGuid& spriteVramId);
	void AddSpriteBatchLayer(RenderPassGuid& renderPassId);
	void AddSpriteInstanceBufferId(UM_SpriteBatchID spriteInstanceBufferId, int BufferId);
	void AddSpriteInstanceLayerList(UM_SpriteBatchID spriteBatchId, Vector<SpriteInstanceStruct>& spriteInstanceList);
	void AddSpriteBatchObjectList(UM_SpriteBatchID spriteBatchId, GameObjectID spriteBatchObject);
	SpriteBatchLayer AddSpriteLayer(VkGuid& renderPassId);
	void Update(const float& deltaTime);

	Sprite* FindSprite(GameObjectID gameObjectId);
	const SpriteVram& FindVramSprite(RenderPassGuid guid);
	const Animation2D& FindSpriteAnimation(const UM_AnimationListID& animationId);
	const Vector<AnimationFrames>& FindSpriteAnimationFrames(const VkGuid& vramSpriteId);
	const SpriteInstanceStruct* FindSpriteInstance(GameObjectID gameObjectId);
	const int FindSpriteInstanceBufferId(UM_SpriteBatchID spriteInstanceBufferId);
	 Vector<SpriteInstanceStruct>& FindSpriteInstanceList(UM_SpriteBatchID spriteAnimation);
	const Vector<GameObjectID>& FindSpriteBatchObjectListMap(UM_SpriteBatchID spriteBatchObjectListId);

	Vector<SpriteBatchLayer>& FindSpriteBatchLayer(RenderPassGuid& guid);
	size_t FindSpriteIndex(GameObjectID gameObjectId);

	VkGuid LoadSpriteVRAM(const String& spriteVramPath);

	const Vector<Sprite>& SpriteListRef() { return SpriteList; }
};
extern SpriteSystem spriteSystem;
