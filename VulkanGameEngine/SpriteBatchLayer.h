#pragma once
#include "TypeDef.h"
#include "Sprite.h"
#include "Mesh2D.h"

class SpriteBatchLayer
{
private:
	 List<Vertex2D> SpriteVertexList =
	{
		Vertex2D(vec2(0.0f, 0.0), vec2(0.0f, 0.0f)),
		Vertex2D(vec2(0.0f, 0.0), vec2(1.0f, 0.0f)),
		Vertex2D(vec2(0.0f, 0.0), vec2(1.0f, 1.0f)),
		Vertex2D(vec2(0.0f, 0.0), vec2(0.0f, 1.0f)),
	};

	 List<uint32> SpriteIndexList =
	{
	  0, 3, 1,
	  1, 3, 2
	};

	uint32                          MaxSpritesPerSheet;
	uint32                          SpriteLayerIndex;

	List<SharedPtr<Sprite>>         SpriteList;
	List<SpriteInstanceStruct>      SpriteInstanceList;
	SharedPtr<SpriteInstanceBuffer> SpriteBuffer;
	SharedPtr<Mesh2D>		        SpriteLayerMesh;


public:
	String					        Name;

	SpriteBatchLayer();
	virtual ~SpriteBatchLayer();
	void AddSprite(SharedPtr<Sprite> sprite);
	void BuildSpriteLayer(List<SharedPtr<Sprite>>& spriteDrawList);
	void Update(float deltaTime);
	void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
	void Destroy();

	SharedPtr<Mesh2D> GetSpriteLayerMesh() { return SpriteLayerMesh; }
};

