#pragma once
#include "Typedef.h"
#include "VkGuid.h"

struct LevelTileSet
{
	VkGuid TileSetId;
	VkGuid MaterialId;
	vec2   TilePixelSize;
	ivec2  TileSetBounds;
	vec2   TileUVSize;
};

