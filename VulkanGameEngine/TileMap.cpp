#include "TileMap.h"



LevelMap::LevelMap()
{

}

LevelMap::~LevelMap()
{

}

void LevelMap::LoadLevelMap(Vector<SharedPtr<Tile>>& tileList, SharedPtr<Material> levelMaterial)
{
	TileList = tileList;
	LevelMaterial = levelMaterial;
	LoadTiles();
}

void LevelMap::LoadTiles()
{

}