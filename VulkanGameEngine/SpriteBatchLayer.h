#pragma once
#include "TypeDef.h"
#include "Sprite.h"
#include <Mesh.h>
#include <VulkanPipeline.h>
#include "ECSid.h"

class SpriteBatchLayer
{
private:
	static uint32 NextSpriteBatchLayerID;

public:
	VkGuid RenderPassId;
	uint SpriteBatchLayerID = 0;
	uint SpriteLayerMeshId = 0;

	SpriteBatchLayer();
	SpriteBatchLayer(VkGuid& renderPassId);
	~SpriteBatchLayer();

	void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
};

