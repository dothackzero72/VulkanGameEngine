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
	enum SpriteAnimationEnum
	{
		kStanding,
		kWalking
	};

public:

	uint32 CurrentAnimationID = 0;

	uint32 ParentGameObjectID;

	uint32 SpritesheetID;
	uint SpriteMaterialID;



	uint CurrentFrame = 0;

	bool SpriteAlive = true;

	vec2 SpritePosition = vec2(0.0f);
	uint SpriteLayer = 0;
	vec2 SpriteRotation = vec2(0.0f);
	vec2 SpriteScale = vec2(1.0f);
	vec2 SpriteSize = vec2(50.0f);
	vec4 SpriteColor = vec4(0.0f, 0.0f, 0.0f, 1.0f);

	SharedPtr<SpriteInstanceStruct> SpriteInstance;
	UnorderedMap<SpriteAnimationEnum, Animation2D>  AnimationList;
	Animation2D CurrentSpriteAnimation;

	Sprite();
	Sprite(uint32 id, uint32 spriteSheetID);
	virtual ~Sprite();

	virtual void Input(const float& deltaTime);
	virtual void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
	virtual void Destroy();

	SharedPtr<SpriteInstanceStruct> GetSpriteInstance() { return SpriteInstance; }

	Sprite& operator=(const Sprite& other)
	{
		if (this != &other)
		{
			CurrentAnimationID = other.CurrentAnimationID;
			ParentGameObjectID = other.ParentGameObjectID;
			SpritesheetID = other.SpritesheetID;
			SpriteMaterialID = other.SpriteMaterialID;
			CurrentFrame = other.CurrentFrame;
			SpriteAlive = other.SpriteAlive;
			SpritePosition = other.SpritePosition;
			SpriteLayer = other.SpriteLayer;
			SpriteRotation = other.SpriteRotation;
			SpriteScale = other.SpriteScale;
			SpriteSize = other.SpriteSize;
			SpriteColor = other.SpriteColor;
			SpriteInstance = other.SpriteInstance;
			AnimationList = other.AnimationList;
			CurrentSpriteAnimation = other.CurrentSpriteAnimation;
		}
		return *this; 
	}
};

