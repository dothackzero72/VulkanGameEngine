#include "TileMap.h"



LevelMap::LevelMap()
{

}

LevelMap::~LevelMap()
{

}

void LevelMap::LoadLevelMap(Vector<WeakPtr<Tile>>& tileList, WeakPtr<Material> levelMaterial)
{
	TileList = tileList;
	LevelMaterial = levelMaterial;
	LoadTiles();
}

void LevelMap::LoadTiles()
{

}