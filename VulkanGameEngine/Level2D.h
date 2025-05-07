#pragma once
#include "Typedef.h"
#include "Vertex.h"
#include "LevelTile.h"
#include "Mesh.h"
#include "LevelLayer.h"

class Level2D
{
private:
	VkGuid				MaterialId;
	ivec2				LevelBounds;
	Vector<LevelLayer> levelLayerList;

	void LoadLevelTiles();

public:
	Level2D();
	Level2D(const String& levelPath);
	~Level2D();
};

