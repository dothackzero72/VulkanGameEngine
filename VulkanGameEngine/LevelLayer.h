#pragma once
#include "Typedef.h"
#include "Vertex.h"
#include "LevelTile.h"
#include "LevelTileSet.h"
#include "Mesh.h"

class LevelLayer
{
private:

	void LoadLevelMesh();

public:
	VkGuid				LevelId;
	uint				MeshId;
	VkGuid				MaterialId;
	VkGuid				TileSetId;
	int					LevelLayerIndex;
	ivec2				LevelBounds;
	Vector<uint>		TileIdMap;
	Vector<LevelTile>	TileList;
	Vector<Vertex2D>	VertexList;
	Vector<uint32>		IndexList;

	LevelLayer();
	LevelLayer(VkGuid& tileSetId, Vector<uint>& tileIdMap, ivec2& levelBounds, int levelLayerIndex);
	~LevelLayer();

	void Update(const float& deltaTime);
};

