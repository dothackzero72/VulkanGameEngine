#pragma once
#include "LevelTileSet.h"
#include "Tile.h"

struct LevelLayout
{
	VkGuid					  LevelLayoutId;
	ivec2					  LevelBounds;
	ivec2					  TileSizeinPixels;
	Vector<Tile>			  LevelTileList;
	Vector<Vector<uint>>	  LevelMapList;
};

