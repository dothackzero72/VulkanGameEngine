#pragma once
#include "Typedef.h"
#include "Material.h"
#include "Sprite.h"

class Sprite;
class SpriteSheet
{
private:
	SharedPtr<Material> SpriteMaterial;

	ivec2				SpritePixelSize;
	ivec2				SpriteCells;
	vec2				SpriteUVSize;
	uint				SpriteLayer;

public:
	SpriteSheet();
	SpriteSheet(SharedPtr<Material> spriteMaterial, vec2& spritePixelSize, uint spriteLayer);
	virtual ~SpriteSheet();

	SharedPtr<Material> GetSpriteMaterial() { return SpriteMaterial; }
	ivec2 GetSpritePixelSize() { return SpritePixelSize; }
	ivec2 GetSpriteCells() { return SpriteCells; }
	vec2 GetSpriteUVSize() { return SpriteUVSize; }
	uint GetSpriteLayer() { return SpriteLayer; }
};

