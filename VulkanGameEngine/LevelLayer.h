#pragma once
#include "Typedef.h"
#include "Vertex.h"
#include "LevelTile.h"
#include "LevelTileSet.h"
#include "Mesh.h"

class LevelLayer
{
private:

	void LoadLevelTiles();

public:
	uint				MeshId;
	VkGuid				MaterialId;
	ivec2				LevelBounds;
	LevelTileSet		TileSet;
	Vector<LevelTile>	TileList;
	Vector<Vertex2D>	VertexList;
	Vector<uint32>		IndexList;

	LevelLayer();
	~LevelLayer();
};

