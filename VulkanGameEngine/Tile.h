#pragma once
#include "Typedef.h"

struct Tile
{
	uint  TileId;
	ivec2 TileUVCellOffset;
	vec2  TileUVOffset;
	int	  TileLayer;
	bool  IsAnimatedTile;
};

