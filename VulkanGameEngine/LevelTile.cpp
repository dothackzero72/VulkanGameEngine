#include "LevelTile.h"

LevelTile::LevelTile()
{
}

LevelTile::LevelTile(uint tileId, vec2& tileSize, ivec2& tileOffset, vec2& tileUVSize, int tileLayer)
{
	TileId = tileId;
	TileSize = tileSize;
	TileOffset = tileOffset;
	TileUVSize = tileUVSize;
	TileLayer = tileLayer;
}

LevelTile::~LevelTile()
{
}

void LevelTile::LoadTile(nlohmann::json& json)
{
}
