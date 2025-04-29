#pragma once
#include "TypeDef.h"
#include "Sprite.h"
#include "Mesh.h"
#include "JsonPipeline.h"

class SpriteBatchLayer
{
private:
	static uint32 NextSpriteBatchLayerID;
	void SortSpritesByLayer();

public:
	uint RenderPassId = 0;
	uint SpriteBatchLayerID = 0;
	uint MaxSpritesPerSheet;
	uint SpriteLayerIndex;
	uint SpriteLayerMeshId;

	SpriteBatchLayer();
	SpriteBatchLayer(uint32 renderPassId, Vector<GameObject>& gameObjectList, JsonPipeline spriteRenderPipeline);
	~SpriteBatchLayer();

	void LoadSprites();
	void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
	void Destroy();

	void AddSprite(uint gameObjectID);
	void RemoveSprite(uint gameObjectID);
};

