#pragma once
#include "TypeDef.h"
#include "Sprite.h"
#include "Mesh.h"
#include "Mesh2D.h"
#include "JsonPipeline.h"
#include "Level2DRenderer.h"

class Level2DRenderer;
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

	Vector<WeakPtr<Sprite>>         SpriteList;
	Vector<SpriteInstanceStruct>    SpriteInstanceList;
	SpriteInstanceBuffer			SpriteBuffer;
	SharedPtr<Mesh2D>		        SpriteLayerMesh;

	void SortSpritesByLayer(std::vector<WeakPtr<Sprite>>& sprites);

public:
	String					        Name;
	SharedPtr<JsonPipeline>			SpriteRenderPipeline;
	SpriteBatchLayer();
	SpriteBatchLayer(Vector<SharedPtr<GameObject>>& gameObjectList, SharedPtr<JsonPipeline> spriteRenderPipeline);
	virtual ~SpriteBatchLayer();

	void LoadSprites();
	void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
	void Draw(VkCommandBuffer& commandBuffer);
	void Destroy();

	void AddSprite(SharedPtr<Sprite> sprite);
	void RemoveSprite(SharedPtr<Sprite> sprite);

	SharedPtr<Mesh2D> GetSpriteLayerMesh() { return SpriteLayerMesh; }
};

