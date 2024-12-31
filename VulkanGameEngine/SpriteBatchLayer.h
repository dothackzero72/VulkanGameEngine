#pragma once
#include "TypeDef.h"
#include "Sprite.h"
#include "Mesh2D.h"

class SpriteBatchLayer
{
private:
	uint32                    MaxSpritesPerSheet;
	uint32                    SpriteLayer;
	List<SharedPtr<Sprite>>   SpriteDrawList;
	SharedPtr<Mesh2D>		  SpriteLayerMesh;

	List<Vertex2D>            VertexList;
	List<uint32>              IndexList;

public:
	String					  Name;

	SpriteBatchLayer();
	virtual ~SpriteBatchLayer();

	void BuildSpriteLayer(List<SharedPtr<Sprite>>& spriteDrawList);
	void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
	void Destroy();

	SharedPtr<Mesh2D> GetSpriteLayerMesh() { return SpriteLayerMesh; }
};

