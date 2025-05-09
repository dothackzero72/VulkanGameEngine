#pragma once
#include "Typedef.h"
#include "Vertex.h"
#include "LevelTile.h"
#include "Mesh.h"
#include "LevelLayer.h"

class Level2D
{
private:
	VkGuid				 LevelId;
	VkGuid				 TileSetId;
	ivec2				 LevelBounds;
	Vector<Vector<uint>> TileIdMapLayers;
	Vector<LevelLayer>   LevelLayerList;

public:
	Level2D();
	Level2D(const VkGuid& levelId, const VkGuid& tileSetId, const ivec2& levelBounds, const Vector<Vector<uint>>& tileIdMapLayers);
	~Level2D();

	void Update(const float& deltaTime);
};

