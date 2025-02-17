#include "SpriteSheet.h"

SpriteSheet::SpriteSheet()
{
}

SpriteSheet::SpriteSheet(SharedPtr<Material> spriteMaterial, ivec2& spritePixelSize, uint spriteLayer)
{
	SpriteMaterial = spriteMaterial;
	SpritePixelSize = spritePixelSize;
	SpriteCells = ivec2(spriteMaterial->GetAlbedoMap()->Width / spritePixelSize.x, spriteMaterial->GetAlbedoMap()->Height / spritePixelSize.y);
	SpriteUVSize = vec2(1.0f / (float)SpriteCells.x, 1.0f / (float)SpriteCells.y);
	SpriteLayer = spriteLayer;
	SpriteScale = vec2(5.0f);
}

SpriteSheet::SpriteSheet(SharedPtr<Material> spriteMaterial, ivec2& spritePixelSize, uint spriteLayer, vec2 spriteScale)
{
	SpriteMaterial = spriteMaterial;
	SpritePixelSize = spritePixelSize;
	SpriteCells = ivec2(spriteMaterial->GetAlbedoMap()->Width / spritePixelSize.x, spriteMaterial->GetAlbedoMap()->Height / spritePixelSize.y);
	SpriteUVSize = vec2(1.0f / (float)SpriteCells.x, 1.0f / (float)SpriteCells.y);
	SpriteLayer = spriteLayer;
	SpriteScale = spriteScale;
}
