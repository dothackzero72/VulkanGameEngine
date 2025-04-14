#pragma once
#include "Typedef.h"
#include "Material.h"


class SpriteSheet
{
public:
	uint32				SpriteMaterialID;
	ivec2				SpritePixelSize;
	ivec2				SpriteCells;
	vec2				SpriteUVSize;
	uint				SpriteLayer;
	vec2				SpriteScale;

	SpriteSheet();
	SpriteSheet(uint32 SpriteMaterialID, ivec2& spritePixelSize, uint spriteLayer);
	SpriteSheet(uint32 SpriteMaterialID, ivec2& spritePixelSize, uint spriteLayer, vec2 spriteScale);
};

