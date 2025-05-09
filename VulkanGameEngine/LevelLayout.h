#pragma once
#include "LevelTileSet.h"
#include "LevelTile.h"

struct LevelLayout
{
	VkGuid LevelLayoutId;
	ivec2  LevelBounds;
	ivec2  TileSizeinPixels;
	Vector<LevelTile> LevelTileList;
	Vector<uint>	  LevelMapList;
};

