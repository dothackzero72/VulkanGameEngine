#pragma once
#include "TypeDef.h"
#include "SpriteSheet.h"
#include "Animation2D.h"
#include "SceneDataBuffer.h"
#include "Vertex.h"

class Sprite
{
	friend class SpriteSheet;
private:
	uint32 CurrentAnimationID = 0;

	SharedPtr<SpriteSheet> SpriteSheetPtr;
	List<SharedPtr<Animation2D>> AnimationList;
	ivec2 SpriteSize;

	vec2 Position;
	vec2 UV;
	vec4 Color;

public:

	List<Vertex2D> VertexList;
	List<uint32> IndexList;

	Sprite();
	Sprite(SharedPtr<Material> material, vec2 spriteSize);
	virtual ~Sprite();

	virtual void Input(float deltaTime);
	virtual void Update(float deltaTime);
	virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime);
	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
	virtual void Destroy();
};

