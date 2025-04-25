#pragma once
#include "TypeDef.h"
#include "Animation2D.h"
#include "SceneDataBuffer.h"
#include "Vertex.h"
#include "GameObject.h"
#include "Transform2DComponent.h"

struct Tile
{
	uint32 TileSizeinPixels = 256;
	vec2 TilePosition = vec2(0.0f);
};

class LevelMap
{
private:
	SharedPtr<Material>			  LevelMaterial;
	Vector<SharedPtr<Tile>>         TileList;

	void LoadTiles();

public:
	LevelMap();
	virtual ~LevelMap();

	void LoadLevelMap(Vector<SharedPtr<Tile>>& tileList, SharedPtr<Material> levelMaterial);
};