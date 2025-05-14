#pragma once
#include <vector>
#include "Typedef.h"
#include "VkGuid.h"
#include "Tile.h"

struct LevelTileSet
{
	VkGuid			  TileSetId;
	VkGuid			  MaterialId;
	Vector<VkGuid>	  RenderPassIds;
	ivec2			  TileSetBounds;
	vec2			  TilePixelSize;
	vec2			  TileScale = vec2(5.0f);
	vec2			  TileUVSize;
	Vector<Tile>	  LevelTileList;
};

