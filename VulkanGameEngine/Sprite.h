#pragma once
#include "TypeDef.h"
#include "Animation2D.h"
#include "SceneDataBuffer.h"
#include "Vertex.h"
#include "GameObject.h"
#include "Transform2DComponent.h"
#include "VkGuid.h"

class AssetManager;
class Sprite
{
private:
	static uint NextSpriteID;
	enum SpriteAnimationEnum
	{
		kStanding,
		kWalking
	};

public:

	GameObjectID GameObjectId;
	uint SpriteID = 0;
	VkGuid SpriteVramId;
	uint CurrentAnimationID = 0;
	uint CurrentFrame = 0;
	float CurrentFrameTime = 0.0f;

	vec2 LastSpritePosition = vec2(0.0f);
	vec2 LastSpriteRotation = vec2(0.0f);
	vec2 LastSpriteScale = vec2(1.0f);
	vec2 SpritePosition = vec2(0.0f);
	vec2 SpriteRotation = vec2(0.0f);
	vec2 SpriteScale = vec2(1.0f);

	bool SpriteAlive = true;

	Sprite();
	Sprite(GameObjectID gameObjectId, VkGuid& spriteVramId);
	~Sprite();

	SpriteInstanceStruct Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
};

