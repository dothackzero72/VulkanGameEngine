#include "Level2D.h"
#include "RenderSystem.h"

Level2D::Level2D()
{
}

Level2D::Level2D(const VkGuid& levelId, const VkGuid& tileSetId, const ivec2& levelBounds, const Vector<Vector<uint>>& tileIdMapLayers)
{
	LevelId = levelId;
	TileSetId = tileSetId;
	LevelBounds = levelBounds;
	TileIdMapLayers = tileIdMapLayers;

	for (int x = 0; x < tileIdMapLayers.size(); x++)
	{
		LevelLayerList.emplace_back(LevelLayer(TileSetId, TileIdMapLayers[x], LevelBounds, x));
	}
}

Level2D::~Level2D()
{
}

void Level2D::Update(const float& deltaTime)
{
	for (auto& levelLayer : LevelLayerList)
	{
		levelLayer.Update(deltaTime);
	}
}
