#include "LevelLayer.h"
#include "GameObjectSystem.h"
#include "BufferSystem.h"
#include "MeshSystem.h"

LevelLayer::LevelLayer()
{
}

LevelLayer::LevelLayer(VkGuid& levelId, VkGuid& tileSetId, Vector<uint>& tileIdMap, ivec2& levelBounds, int levelLayerIndex)
{
	const LevelTileSet& tileSet = levelSystem.LevelTileSetMap[TileSetId];

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
    const LevelTileSet& tileSet = levelSystem.LevelTileSetMap[TileSetId];
    Vector<Tile> tileSetList = Vector<Tile>(tileSet.LevelTileListPtr, tileSet.LevelTileListPtr + tileSet.LevelTileCount);
    for (uint x = 0; x < LevelBounds.x; x++)
    {
        for (uint y = 0; y < LevelBounds.y; y++)
        {
            const uint& tileId = TileIdMap[(y * LevelBounds.x) + x];
            const Tile& tile = tileSetList[tileId];

            const float LefttSideUV =  tile.TileUVOffset.x;
            const float RightSideUV =  tile.TileUVOffset.x + tileSet.TileUVSize.x;
            const float TopSideUV =    tile.TileUVOffset.y;
            const float BottomSideUV = tile.TileUVOffset.y + tileSet.TileUVSize.y;

            const uint VertexCount =   VertexList.size();
            const vec2 TilePixelSize = tileSet.TilePixelSize * tileSet.TileScale;
            const Vertex2D BottomLeftVertex =  { { x * TilePixelSize.x,                         y * TilePixelSize.y},                     {LefttSideUV, BottomSideUV} };
            const Vertex2D BottomRightVertex = { {(x * TilePixelSize.x) + TilePixelSize.x,      y * TilePixelSize.y},                     {RightSideUV, BottomSideUV} };
            const Vertex2D TopRightVertex =    { {(x * TilePixelSize.x) + TilePixelSize.x,     (y * TilePixelSize.y) + TilePixelSize.y},  {RightSideUV, TopSideUV} };
            const Vertex2D TopLeftVertex =     { { x * TilePixelSize.x,                        (y * TilePixelSize.y) + TilePixelSize.y},  {LefttSideUV, TopSideUV} };

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
    MeshId = meshSystem.CreateLevelLayerMesh(LevelId, VertexList, IndexList);
}
