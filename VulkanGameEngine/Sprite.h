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

	mat4 SpriteMatrix = mat4(1.0f);
	vec2 SpritePosition = vec2(0.0f);
	vec2 SpriteRotation = vec2(0.0f);
	vec2 SpriteScale = vec2(0.0f);



	//vec2 SpriteSize = vec2(0.0f);
	//vec2 UV = vec2(0.0f);
	//vec2 UVOffset = vec2(0.0f);
	//ivec2 FlipSprite = ivec2(0);
	//vec4 Color = vec4(0.0f);
	//uint MaterialID = 0;
	//mat4 InstanceTransform = mat4(1.0f);
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

