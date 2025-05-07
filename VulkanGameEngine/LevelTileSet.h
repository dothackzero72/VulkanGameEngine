#pragma once
#include "Typedef.h"
#include "VkGuid.h"

struct LevelTileSet
{
	VkGuid TileSetId;
	VkGuid MaterialId;
	vec2   TileSizeInPixels;
	vec2   TileUVOffset;
	ivec2  TileSetBounds;

	LevelTileSet()
	{

	}

	LevelTileSet
};

