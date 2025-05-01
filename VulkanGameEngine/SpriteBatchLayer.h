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
	uint SpriteLayerMeshId = 0;

	SpriteBatchLayer();
	SpriteBatchLayer(uint32 renderPassId);
	~SpriteBatchLayer();

	void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
	void Destroy();

	void AddSprite(uint gameObjectID);
	void RemoveSprite(uint gameObjectID);
};

