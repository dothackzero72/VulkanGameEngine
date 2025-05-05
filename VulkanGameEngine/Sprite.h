#pragma once
#include "TypeDef.h"
#include "VkGuid.h"
#include "ECGid.h"
#include "vertex.h"

class Sprite
{
private:
	static uint NextSpriteID;

	uint CurrentAnimationID = 0;
	uint CurrentFrame = 0;
	VkGuid SpriteVramId;
	float CurrentFrameTime = 0.0f;
	bool SpriteAlive = true;

public:

	enum SpriteAnimationEnum
	{
		kStanding,
		kWalking
	};

	GameObjectID GameObjectId;
	uint SpriteID = 0;

	ivec2 FlipSprite = vec2(0);
	vec2 LastSpritePosition = vec2(0.0f);
	vec2 LastSpriteRotation = vec2(0.0f);
	vec2 LastSpriteScale = vec2(1.0f);
	vec2 SpritePosition = vec2(0.0f);
	vec2 SpriteRotation = vec2(0.0f);
	vec2 SpriteScale = vec2(1.0f);

	Sprite();
	Sprite(GameObjectID gameObjectId, VkGuid& spriteVramId);
	~Sprite();

	SpriteInstanceStruct Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
	void SetSpriteAnimation(SpriteAnimationEnum spriteAnimation);
};

