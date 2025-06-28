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
	UnorderedMap<UM_SpriteBatchID, int>							  SpriteInstanceBufferMap;
	UnorderedMap<UM_SpriteBatchID, Vector<size_t>>                SpriteBatchLayerSpriteListMap;
	UnorderedMap<RenderPassGuid, SpriteVram>                      VramSpriteMap;

	UnorderedMap<UM_AnimationListID, Animation2D>				  AnimationMap;
	UnorderedMap<VkGuid, Vector<AnimationFrames>>			      AnimationFrameListMap;

public:

	SpriteSystem();
	~SpriteSystem();

	void AddSprite(GameObjectID gameObjectId, VkGuid& spriteVramId);
	SpriteBatchLayer AddSpriteLayer(VkGuid& renderPassId);
	void Update(const float& deltaTime);

	Sprite* FindSprite(GameObjectID gameObjectId);
	const SpriteInstanceStruct* FindSpriteInstance(GameObjectID gameObjectId);
	size_t findSpriteIndex(GameObjectID gameObjectId);

	const Vector<Sprite>& SpriteListRef() { return SpriteList; }
};
extern SpriteSystem spriteSystem;
