#pragma once
#include "TypeDef.h"
#include "Sprite.h"
#include "Mesh2D.h"

struct SpriteInstanceStruct
{
	vec2 SpriteSize = vec2(0.0f);
	vec2 UV = vec2(0.0f);
	vec2 UVOffset = vec2(0.0f);
	ivec2 FlipSprite = ivec2(0);
	vec4 Color = vec4(0.0f);
	uint MaterialID = 0;
	mat4 InstanceTransform = mat4(1.0f);

	SpriteInstanceStruct()
	{

	}

	SpriteInstanceStruct(vec2 spriteSize, vec2 uv, vec2 uvOffset, vec2 flipSprite, vec4 color, uint materialID, mat4 instanceTransform)
	{
		SpriteSize = spriteSize;
		UV = uv;
		UVOffset = uvOffset;
		FlipSprite = flipSprite;
		Color = color;
		MaterialID = materialID;
		InstanceTransform = instanceTransform;
	}
};

class SpriteBatchLayer
{
private:
	const List<Vertex2D> SpriteVertexList =
	{
		Vertex2D(vec2(0.0f, 0.0)),
		Vertex2D(vec2(0.0f, 0.0)),
		Vertex2D(vec2(0.0f, 0.0)),
		Vertex2D(vec2(0.0f, 0.0)),
	};

	const List<uint32> SpriteIndexList =
	{
	  0, 3, 1,
	  1, 3, 2
	};

	uint32                     MaxSpritesPerSheet;
	uint32                     SpriteLayer;
	SharedPtr<Material>		   material;
	SharedPtr<Material>		   Material2;
	List<SharedPtr<Sprite>>    SpriteDrawList;
	List<SpriteInstanceStruct> SpriteInstanceInfo;
	SharedPtr<Mesh2D>		   SpriteLayerMesh;

	List<Vertex2D>             VertexList;
	List<uint32>               IndexList;

	void AddSprite(List<SharedPtr<Sprite>>& spriteVertexList);

public:
	String					   Name;

	SpriteBatchLayer();
	virtual ~SpriteBatchLayer();

	void BuildSpriteLayer(List<SharedPtr<Sprite>>& spriteDrawList);
	void Update(float deltaTime);
	void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
	void Destroy();

	SharedPtr<Mesh2D> GetSpriteLayerMesh() { return SpriteLayerMesh; }
};

