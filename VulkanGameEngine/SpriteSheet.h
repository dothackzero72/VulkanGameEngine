#pragma once
#include "Typedef.h"
#include "Material.h"

class SpriteSheet
{
public:
	SharedPtr<Material> SpriteMaterial;
	ivec2				SpritePixelSize;
	ivec2				SpriteCells;
	vec2				SpriteUVSize;
	uint				SpriteLayer;
	vec2				SpriteScale;

	SpriteSheet();
	SpriteSheet(SharedPtr<Material> spriteMaterial, ivec2& spritePixelSize, uint spriteLayer);
	SpriteSheet(SharedPtr<Material> spriteMaterial, ivec2& spritePixelSize, uint spriteLayer, vec2 spriteScale);
};

