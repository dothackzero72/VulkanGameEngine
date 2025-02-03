#pragma once
#include "TypeDef.h"
#include "Sprite.h"
#include "Mesh2D.h"

class SpriteBatchLayer
{
private:
	 List<Vertex2D> SpriteVertexList =
	{
		Vertex2D(vec2(0.0f, 1.0), vec2(0.0f, 0.0f)),
		Vertex2D(vec2(1.0f, 0.0), vec2(1.0f, 0.0f)),
		Vertex2D(vec2(1.0f, 0.0), vec2(1.0f, 1.0f)),
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
	SpriteInstanceBuffer			SpriteBuffer;
	SharedPtr<Mesh2D>		        SpriteLayerMesh;
	SharedPtr<JsonPipeline>			SpriteRenderPipeline;

	SpriteBatchLayer(SharedPtr<JsonPipeline> spriteRenderPipeline, List<SharedPtr<Sprite>> spriteList);

public:
	String					        Name;

	SpriteBatchLayer();
	virtual ~SpriteBatchLayer();

	static SharedPtr<SpriteBatchLayer> CreateSpriteBatchLayer(SharedPtr<JsonPipeline> spriteRenderPipeline, List<SharedPtr<Sprite>>& spriteList);

	void BuildSpriteLayer(List<SharedPtr<Sprite>>& spriteDrawList);
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

