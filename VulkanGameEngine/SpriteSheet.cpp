#include "SpriteSheet.h"
#include "AssetManager.h"

SpriteSheet::SpriteSheet()
{
}

SpriteSheet::SpriteSheet(uint32 spriteMaterialID, ivec2& spritePixelSize, uint spriteLayer)
{
	Material material = assetManager.MaterialList[spriteMaterialID];

	SpriteMaterialID = spriteMaterialID;
	SpritePixelSize = spritePixelSize;
	SpriteCells = ivec2(material.GetAlbedoMap()->Width / spritePixelSize.x, material.GetAlbedoMap()->Height / spritePixelSize.y);
	SpriteUVSize = vec2(1.0f / (float)SpriteCells.x, 1.0f / (float)SpriteCells.y);
	SpriteLayer = spriteLayer;
	SpriteScale = vec2(5.0f);
}

SpriteSheet::SpriteSheet(uint32 spriteMaterialID, ivec2& spritePixelSize, uint spriteLayer, vec2 spriteScale)
{
	Material material = assetManager.MaterialList[spriteMaterialID];

	SpriteMaterialID = spriteMaterialID;
	SpritePixelSize = spritePixelSize;
	SpriteCells = ivec2(material.GetAlbedoMap()->Width / spritePixelSize.x, material.GetAlbedoMap()->Height / spritePixelSize.y);
	SpriteUVSize = vec2(1.0f / (float)SpriteCells.x, 1.0f / (float)SpriteCells.y);
	SpriteLayer = spriteLayer;
	SpriteScale = spriteScale;
}
