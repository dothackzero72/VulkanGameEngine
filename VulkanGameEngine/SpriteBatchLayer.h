#pragma once
#include "TypeDef.h"
#include "Sprite.h"
#include "Mesh.h"
#include "JsonPipeline.h"
#include "ECGid.h"

class SpriteBatchLayer
{
private:
	static uint32 NextSpriteBatchLayerID;
	void SortSpritesByLayer();

public:
	RenderPassID RenderPassId;
	uint SpriteBatchLayerID = 0;
	uint SpriteLayerMeshId = 0;

	SpriteBatchLayer();
	SpriteBatchLayer(RenderPassID renderPassId);
	~SpriteBatchLayer();

	void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
	void Destroy();

	void AddSprite(uint gameObjectID);
	void RemoveSprite(uint gameObjectID);
};

