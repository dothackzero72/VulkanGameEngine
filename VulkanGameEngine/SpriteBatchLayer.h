#pragma once
#include "TypeDef.h"
#include "Sprite.h"
#include "Mesh2D.h"

class SpriteBatchLayer
{
private:
	uint32                    MaxSpritesPerSheet;
	uint32                    SpriteLayer;
	SharedPtr<Material>		  material;
	SharedPtr<Material>		  Material2;
	List<SharedPtr<Sprite>>   SpriteDrawList;
	SharedPtr<Mesh2D>		  SpriteLayerMesh;

	List<Vertex2D>            VertexList;
	List<uint32>              IndexList;

	void AddSprite(List<SharedPtr<Sprite>>& spriteVertexList);

public:
	String					  Name;

	SpriteBatchLayer();
	virtual ~SpriteBatchLayer();

	void BuildSpriteLayer(List<SharedPtr<Sprite>>& spriteDrawList);
	void Update(float deltaTime);
	void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
	void Destroy();

	SharedPtr<Mesh2D> GetSpriteLayerMesh() { return SpriteLayerMesh; }
};

