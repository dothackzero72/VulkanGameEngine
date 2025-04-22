#include "SpriteSheet.h"
#include "AssetManager.h"

SpriteSheet::SpriteSheet()
{
}

SpriteSheet::SpriteSheet(uint32 spriteMaterialID, ivec2& spritePixelSize, uint spriteLayer)
{
	const Material material = assetManager.MaterialList[spriteMaterialID];
	const Texture texture = assetManager.TextureList[material.AlbedoMap];

	SpriteMaterialID = spriteMaterialID;
	SpritePixelSize = spritePixelSize;
	SpriteCells = ivec2(texture.Width / spritePixelSize.x, texture.Height / spritePixelSize.y);
	SpriteUVSize = vec2(1.0f / (float)SpriteCells.x, 1.0f / (float)SpriteCells.y);
	SpriteLayer = spriteLayer;
	SpriteScale = vec2(5.0f);
}

SpriteSheet::SpriteSheet(uint32 spriteMaterialID, ivec2& spritePixelSize, uint spriteLayer, vec2 spriteScale)
{
	Material material = assetManager.MaterialList[spriteMaterialID];
	const Texture texture = assetManager.TextureList[material.AlbedoMap];

	SpriteMaterialID = spriteMaterialID;
	SpritePixelSize = spritePixelSize;
	SpriteCells = ivec2(texture.Width / spritePixelSize.x, texture.Height / spritePixelSize.y);
	SpriteUVSize = vec2(1.0f / (float)SpriteCells.x, 1.0f / (float)SpriteCells.y);
	SpriteLayer = spriteLayer;
	SpriteScale = spriteScale;
}
