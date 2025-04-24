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
	Vector<Vertex2D> SpriteVertexList =
	{
		Vertex2D(vec2(0.0f, 1.0f), vec2(0.0f, 0.0f)),
		Vertex2D(vec2(1.0f, 0.0f), vec2(1.0f, 0.0f)),
		Vertex2D(vec2(1.0f, 0.0f), vec2(1.0f, 1.0f)),
		Vertex2D(vec2(0.0f, 0.0f), vec2(0.0f, 1.0f)),
	};

	Vector<uint32> SpriteIndexList =
	{
	  0, 3, 1,
	  1, 3, 2
	};

	String					        Name;
	uint32							SpriteBatchLayerID = 0;
	uint32                          MaxSpritesPerSheet;
	uint32                          SpriteLayerIndex;

	Vector<UM_GameObjectID>         GameObjectIDList;
	SharedPtr<SpriteMesh>		    SpriteLayerMesh;
	JsonPipeline 					SpriteRenderPipeline;

	SpriteBatchLayer();
	SpriteBatchLayer(Vector<SharedPtr<GameObject>>& gameObjectList, JsonPipeline spriteRenderPipeline);
	virtual ~SpriteBatchLayer();

	void LoadSprites();
	void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
	void Draw(VkCommandBuffer& commandBuffer, SceneDataBuffer& sceneDataBuffer);
	void Destroy();

	void AddSprite(uint gameObjectID);
	void RemoveSprite(uint gameObjectID);

	SharedPtr<SpriteMesh> GetSpriteLayerMesh() { return SpriteLayerMesh; }
};

