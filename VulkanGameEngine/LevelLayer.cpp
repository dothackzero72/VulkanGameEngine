#include "LevelLayer.h"
#include "RenderSystem.h"
#include "LevelTileSet.h"
LevelLayer::LevelLayer()
{
}

LevelLayer::LevelLayer(VkGuid& levelId, VkGuid& tileSetId, Vector<uint>& tileIdMap, ivec2& levelBounds, int levelLayerIndex)
{
	const LevelTileSet& tileSet = renderSystem.LevelTileSetList[TileSetId];

	LevelId = levelId;
	TileSetId = tileSetId;
	MaterialId = tileSet.MaterialId;
	LevelBounds = levelBounds;
	TileIdMap = tileIdMap;
	LevelLayerIndex = levelLayerIndex;

	LoadLevelMesh();
}

LevelLayer::~LevelLayer()
{
}

void LevelLayer::Update(const float& deltaTime)
{
	Vector<Tile> updateTileList;
	std::copy_if(TileMap.begin(), TileMap.end(),
		std::back_inserter(updateTileList),
		[](const Tile& obj) {
			return obj.IsAnimatedTile; 
		});

	for (auto& updateTile : updateTileList)
	{
		//updateTile.Update(deltaTime);
	}
}

void LevelLayer::LoadLevelMesh()
{
	const LevelTileSet& tileSet = renderSystem.LevelTileSetList[TileSetId];
	for (unsigned int x = 0; x < LevelBounds.x - 1; x++)
	{
		for (unsigned int y = 0; y < LevelBounds.y - 1; y++)
		{
			const uint& tileId = TileIdMap[(y * LevelBounds.x) + x];
			const Tile& tile = tileSet.LevelTileList[tileId];

			const float LefttSideUV =   tile.TileUVOffset.x * tile.TileUVOffset.x;
			const float RightSideUV =  (tile.TileUVOffset.x * tile.TileUVOffset.x) + tile.TileUVOffset.x;
			const float TopSideUV =     tile.TileUVOffset.y * tile.TileUVOffset.y;
			const float BottomSideUV = (tile.TileUVOffset.y * tile.TileUVOffset.y) + tile.TileUVOffset.y;

			const uint VertexCount = VertexList.size();
			const Vertex2D BottomLeftVertex =  { {  x * tileSet.TilePixelSize.x,						      y * tileSet.TilePixelSize.y},                             {LefttSideUV, BottomSideUV} };
			const Vertex2D BottomRightVertex = { { (x * tileSet.TilePixelSize.x) + tileSet.TilePixelSize.x,   y * tileSet.TilePixelSize.y},                             {RightSideUV, BottomSideUV} };
			const Vertex2D TopRightVertex =    { { (x * tileSet.TilePixelSize.x) + tileSet.TilePixelSize.x,  (y * tileSet.TilePixelSize.y) + tileSet.TilePixelSize.y},  {RightSideUV, TopSideUV   } };
			const Vertex2D TopLeftVertex =     { {  x * tileSet.TilePixelSize.x,						     (y * tileSet.TilePixelSize.y) + tileSet.TilePixelSize.y},  {LefttSideUV, TopSideUV   } };

			VertexList.emplace_back(BottomLeftVertex);
			VertexList.emplace_back(BottomRightVertex);
			VertexList.emplace_back(TopRightVertex);
			VertexList.emplace_back(TopLeftVertex);

			IndexList.emplace_back(VertexCount);
			IndexList.emplace_back(VertexCount + 1);
			IndexList.emplace_back(VertexCount + 2);
			IndexList.emplace_back(VertexCount + 2);
			IndexList.emplace_back(VertexCount + 3);
			IndexList.emplace_back(VertexCount);

			TileMap.emplace_back(tile);
		}
	}

	renderSystem.LevelLayerMeshList[LevelId].emplace_back(LevelLayerMesh(VertexList, IndexList, MaterialId));
}
