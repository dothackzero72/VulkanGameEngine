#pragma once
#include "Typedef.h"
#include "LevelLayer.h"


class Level2D
{
public:
	VkGuid				 LevelId;
	VkGuid				 TileSetId;
	ivec2				 LevelBounds;
	Vector<Vector<uint>> TileIdMapLayers;
	Vector<LevelLayer>	 LevelLayerList;

	Level2D();
	Level2D(const VkGuid& levelId, const VkGuid& tileSetId, const ivec2& levelBounds, const Vector<Vector<uint>>& tileIdMapLayers);
	~Level2D();

	void Update(const float& deltaTime);
};

