#pragma once
#include "TypeDef.h"
#include "SpriteSheet.h"
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
	WeakPtr<Material>			  LevelMaterial;
	Vector<WeakPtr<Tile>>         TileList;

	void LoadTiles();

public:
	LevelMap();
	virtual ~LevelMap();

	void LoadLevelMap(Vector<WeakPtr<Tile>>& tileList, WeakPtr<Material> levelMaterial);
};