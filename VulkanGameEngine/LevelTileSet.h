#pragma once
#include "Typedef.h"
#include "VkGuid.h"
#include "LevelTile.h"

struct LevelTileSet
{
	VkGuid			  TileSetId;
	VkGuid			  MaterialId;
	ivec2			  TileSetBounds;
	vec2			  TilePixelSize;
	vec2			  TileUVSize;
	Vector<LevelTile> LevelTileList;
};

