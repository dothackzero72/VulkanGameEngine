#pragma once
#include "Typedef.h"
#include "Material.h"
#include "Sprite.h"

class Sprite;
class SpriteSheet
{
	friend class Sprite;
private:
	vec2				SpriteSize;
	ivec2				SpriteCellCount;
	ivec2				SpriteSizeInPixels;
	ivec2				SpritePosition;
	vec2				SpriteUVSize;
	vec2				CurrentSpriteUV;

	float LeftSideUV;
	float RightSideUV;
	float TopSideUV;
	float BottomSideUV;

	bool				DrawSprite = true;
	bool				Animated;
	uint32				CurrentFrame;
	float				CurrentFrameTime;
	float				FrameHoldTime;
	std::vector<ivec2>  AnimationFrameOffsets;

	SharedPtr<Material> SpriteMaterial;

public:
	SpriteSheet();
	SpriteSheet(SharedPtr<Material> material, vec2 spriteSize, ivec2 tileSizeInPixels, ivec2 SpritePosition, std::vector<ivec2>& AnimationFrameOffsets, float FrameTime);
	virtual ~SpriteSheet();

	void Update(float deltaTime);
};

