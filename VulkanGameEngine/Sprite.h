#pragma once
#include <vulkan/vulkan.h>
#include "TypeDef.h"
#include <VkGuid.h>
#include "ECSid.h"

struct SpriteInstanceStruct
{
    vec2  SpritePosition;
    vec4  UVOffset;
    vec2  SpriteSize;
    ivec2 FlipSprite;
    vec4  Color;
    mat4  InstanceTransform;
    uint  MaterialID;

    SpriteInstanceStruct()
    {
        SpritePosition = vec2(0.0f);
        UVOffset = vec4(0.0f);
        SpriteSize = vec2(0.0f);
        FlipSprite = ivec2(0);
        Color = vec4(0.0f);
        MaterialID = 0;
        InstanceTransform = mat4(1.0f);
    }

    SpriteInstanceStruct(vec2 spritePosition, vec4 uv, vec2 spriteSize, ivec2 flipSprite, vec4 color, uint materialID, mat4 instanceTransform, uint spriteLayer)
    {
        SpritePosition = spritePosition;
        UVOffset = uv;
        SpriteSize = spriteSize;
        FlipSprite = flipSprite;
        Color = color;
        MaterialID = materialID;
        InstanceTransform = instanceTransform;
    }
};

struct SpriteInstanceVertex2D
{
    vec2 SpritePosition;
    vec4 UVOffset;
    vec2 SpriteSize;
    ivec2 FlipSprite;
    vec4 Color;
    mat4 InstanceTransform;
    uint MaterialID;

    SpriteInstanceVertex2D()
    {
        SpritePosition = vec2(0.0f);
        UVOffset = vec4(0.0f);
        SpriteSize = vec2(0.0f);
        FlipSprite = ivec2(0);
        Color = vec4(0.0f);
        MaterialID = 0;
        InstanceTransform = mat4(1.0f);
    }

    SpriteInstanceVertex2D(vec2 spritePosition, vec4 uv, vec2 spriteSize, ivec2 flipSprite, vec4 color, uint materialID, mat4 instanceTransform, uint spriteLayer)
    {
        SpritePosition = spritePosition;
        UVOffset = uv;
        SpriteSize = spriteSize;
        FlipSprite = flipSprite;
        Color = color;
        MaterialID = materialID;
        InstanceTransform = instanceTransform;
    }
};

class Sprite
{
private:
	static uint NextSpriteID;

public:

	enum SpriteAnimationEnum
	{
		kStanding,
		kWalking
	};

	GameObjectID GameObjectId;
	uint SpriteID = 0;
    uint CurrentAnimationID = 0;
    uint CurrentFrame = 0;
    VkGuid SpriteVramId;
    float CurrentFrameTime = 0.0f;
    bool SpriteAlive = true;
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

	SpriteInstanceStruct Update(const float& deltaTime);
	void SetSpriteAnimation(SpriteAnimationEnum spriteAnimation);
};

