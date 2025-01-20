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
	SharedPtr<Material> MaterialPtr;
	List<SharedPtr<Animation2D>> AnimationList;
	ivec2 SpriteSize;
	vec2  SpritePosition;
	vec4  SpriteColor;

public:

	List<Vertex2D> VertexList;

	Sprite();
	Sprite(vec2 spritePosition, vec2 spriteSize, vec4 spriteColor, SharedPtr<Material> material);
	virtual ~Sprite();

	virtual void Input(float deltaTime);
	virtual void Update(float deltaTime);
	virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime);
	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
	virtual void Destroy();
};

