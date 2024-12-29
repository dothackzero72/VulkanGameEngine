#pragma once
#include "SceneDataBuffer.h"
#include "Sprite.h"
#include "Vertex.h"
#include "Mesh2D.h"

class SpriteBatchLayerhRenderer
{
private:
	uint32 MaxSpritesPerSheet;
	uint32 SpriteBatchLayer;
	List<SharedPtr<Sprite>> SpriteDrawList;

	List<Vertex2D> VertexList;
	List<uint32>   IndexList;
	Mesh2D		   Mesh2D;

public:
	SpriteBatchLayerhRenderer();
	virtual ~SpriteBatchLayerhRenderer();

	void BuildSpriteLayer();
	void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
};

