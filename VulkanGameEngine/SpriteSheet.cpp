#include "SpriteSheet.h"

SpriteSheet::SpriteSheet()
{
}

SpriteSheet::SpriteSheet(SharedPtr<Material> spriteMaterial, vec2& spritePixelSize, uint spriteLayer)
{
	SpriteMaterial = spriteMaterial;
	SpritePixelSize = spritePixelSize;
	SpriteCells = glm::ivec2(spriteMaterial->GetAlbedoMap()->Width / spritePixelSize.x, spriteMaterial->GetAlbedoMap()->Height / spritePixelSize.y);
	SpriteUVSize = glm::vec2(1.0f / (float)SpriteCells.x, 1.0f / (float)SpriteCells.y);
	SpriteLayer = spriteLayer;
}

SpriteSheet::~SpriteSheet()
{
}
