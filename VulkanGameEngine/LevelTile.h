#pragma once
#include "Typedef.h"
#include "VkGuid.h"
#include "Vertex.h"

class LevelTile
{
private:
public:
	uint				TileId;
	vec2				TileUVOffset;
	int					TileLayer;
	bool				IsAnimatedTile;

	LevelTile();
	LevelTile(uint TileId, vec2& tilePixelSize, ivec2& tileMapCoords, vec2& TileUVSize, int tileLayer);
	~LevelTile();

	void Update(const float& deltaTime);
};

