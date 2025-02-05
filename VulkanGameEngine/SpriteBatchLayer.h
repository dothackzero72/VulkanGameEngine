#pragma once
#include "TypeDef.h"
#include "Sprite.h"
#include "Mesh2D.h"

class SpriteBatchLayer
{
private:
	Vector<Vertex2D> SpriteVertexList =
	{
		Vertex2D(vec2(0.0f, 1.0), vec2(0.0f, 0.0f)),
		Vertex2D(vec2(1.0f, 0.0), vec2(1.0f, 0.0f)),
		Vertex2D(vec2(1.0f, 0.0), vec2(1.0f, 1.0f)),
		Vertex2D(vec2(0.0f, 0.0), vec2(0.0f, 1.0f)),
	};

	 Vector<uint32> SpriteIndexList =
	{
	  0, 3, 1,
	  1, 3, 2
	};

	uint32                          MaxSpritesPerSheet;
	uint32                          SpriteLayerIndex;

	Vector<SharedPtr<Sprite>>         SpriteList;
	Vector<SpriteInstanceStruct>      SpriteInstanceList;
	SpriteInstanceBuffer			SpriteBuffer;
	SharedPtr<Mesh2D>		        SpriteLayerMesh;
	SharedPtr<JsonPipeline>			SpriteRenderPipeline;

public:
	String					        Name;

	SpriteBatchLayer();
	SpriteBatchLayer(SharedPtr<JsonPipeline> spriteRenderPipeline, Vector<SharedPtr<Sprite>> spriteList);
	virtual ~SpriteBatchLayer();

	void BuildSpriteLayer(Vector<SharedPtr<Sprite>>& spriteDrawList);
	void Update(float deltaTime);
	void Draw(VkCommandBuffer& commandBuffer, SceneDataBuffer& sceneProperties);
	void Destroy();

	void AddSprite(SharedPtr<Sprite> sprite);
	void RemoveSprite(SharedPtr<Sprite> sprite);

	SharedPtr<Mesh2D> GetSpriteLayerMesh() { return SpriteLayerMesh; }
	void SortSpritesByLayer(std::vector<SharedPtr<Sprite>>& sprites)
	{
		std::sort(sprites.begin(), sprites.end(), [](const SharedPtr<Sprite>& a, const SharedPtr<Sprite>& b) {
			return a->SpriteLayer > b->SpriteLayer;
			});
	}
};

