#pragma once
#include "TypeDef.h"
#include "SpriteSheet.h"
#include "Animation2D.h"
#include "SceneDataBuffer.h"
#include "Vertex.h"
#include "GameObject.h"
#include "Transform2DComponent.h"
#include "AssetManager.h"

class AssetManager;
class Sprite
{
	friend class SpriteSheet;
private:
	static uint NextSpriteID;
	enum SpriteAnimationEnum
	{
		kStanding,
		kWalking
	};

public:
	uint ParentGameObjectID = 0;
	uint SpriteID = 0;
	uint SpriteInstance = 0;
	uint SpriteVRAMID = 0;
	uint CurrentAnimationID = 0;
	uint CurrentFrame = 0;

	vec2 SpritePosition = vec2(0.0f);
	vec2 SpriteRotation = vec2(0.0f);
	vec2 SpriteScale = vec2(1.0f);

	Animation2D CurrentSpriteAnimation;
	bool SpriteAlive = true;

	Sprite();
	Sprite(uint32 id, uint32 spriteSheetID);
	~Sprite();

	void Input(const float& deltaTime);
	SpriteInstanceStruct Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
	void Destroy();

	Sprite& operator=(const Sprite& other)
	{
		if (this != &other)
		{
			CurrentAnimationID = other.CurrentAnimationID;
			ParentGameObjectID = other.ParentGameObjectID;
			CurrentFrame = other.CurrentFrame;
			SpriteAlive = other.SpriteAlive;
			SpritePosition = other.SpritePosition;
			SpriteRotation = other.SpriteRotation;
			SpriteScale = other.SpriteScale;
			CurrentSpriteAnimation = other.CurrentSpriteAnimation;
		}
		return *this; 
	}
};

