#pragma once
#include "TypeDef.h"
#include "Sprite.h"
#include "Mesh2D.h"
#include "JsonPipeline.h"

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
	Vector<SpriteInstanceStruct>      SpriteInstanceList;
	SpriteInstanceBuffer			SpriteBuffer;
	SharedPtr<Mesh2D>		        SpriteLayerMesh;
	SharedPtr<JsonPipeline>			SpriteRenderPipeline;

	void SortSpritesByLayer(std::vector<WeakPtr<Sprite>>& sprites);

public:
	String					        Name;

	SpriteBatchLayer();
	SpriteBatchLayer(Vector<SharedPtr<GameObject>>& gameObjectList, SharedPtr<JsonPipeline> spriteRenderPipeline);
	virtual ~SpriteBatchLayer();

	void Update(float deltaTime);
	void Draw(VkCommandBuffer& commandBuffer, SceneDataBuffer& sceneProperties);
	void Destroy();

	void AddSprite(SharedPtr<Sprite> sprite);
	void RemoveSprite(SharedPtr<Sprite> sprite);

	SharedPtr<Mesh2D> GetSpriteLayerMesh() { return SpriteLayerMesh; }
};

