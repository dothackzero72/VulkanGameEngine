#pragma once
#include "Typedef.h"
#include "VkGuid.h"
#include "Vertex.h"

class LevelTile
{
private:
public:
	uint				TileId;
	vec2				TileSize;
	ivec2				TileOffset;
	vec2				TileUVSize;
	vec2				TileUV;
	int					TileLayer;


	LevelTile();
	LevelTile(uint TileId, vec2& tileSize, ivec2& tileOffset, vec2& tileUVSize, int tileLayer);
	~LevelTile();

	void LoadTile(nlohmann::json& json);
};

