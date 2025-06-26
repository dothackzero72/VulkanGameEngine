#include "Level2D.h"
#include "MemorySystem.h"
#include "vertex.h"

LevelLayer Level2D_LoadLevelInfo(VkGuid& levelId, const LevelTileSet& tileSet, Vector<uint>& tileIdMap, ivec2& levelBounds, int levelLayerIndex)
{
    LevelLayer levelLayout = LevelLayer
    {
        .LevelId = levelId,
        .MaterialId = tileSet.MaterialId,
        .TileSetId = tileSet.TileSetId,
        .LevelLayerIndex = levelLayerIndex,
        .LevelBounds = levelBounds
    };

    Vector<Tile>     tileMap;
    Vector<uint32>   indexList;
    Vector<Vertex2D> vertexList;
    Vector<Tile>     tileSetList = Vector<Tile>(tileSet.LevelTileListPtr, tileSet.LevelTileListPtr + tileSet.LevelTileCount);
    for (uint x = 0; x < levelBounds.x; x++)
    {
        for (uint y = 0; y < levelBounds.y; y++)
        {
            const uint& tileId = tileIdMap[(y * levelBounds.x) + x];
            const Tile& tile = tileSetList[tileId];

            const float LeftSideUV = tile.TileUVOffset.x;
            const float RightSideUV = tile.TileUVOffset.x + tileSet.TileUVSize.x;
            const float TopSideUV = tile.TileUVOffset.y;
            const float BottomSideUV = tile.TileUVOffset.y + tileSet.TileUVSize.y;

            const uint VertexCount = vertexList.size();
            const vec2 TilePixelSize = tileSet.TilePixelSize * tileSet.TileScale;
            const Vertex2D BottomLeftVertex =  { { x * TilePixelSize.x,                         y * TilePixelSize.y},                     {LeftSideUV, BottomSideUV} };
            const Vertex2D BottomRightVertex = { {(x * TilePixelSize.x) + TilePixelSize.x,      y * TilePixelSize.y},                     {RightSideUV, BottomSideUV} };
            const Vertex2D TopRightVertex =    { {(x * TilePixelSize.x) + TilePixelSize.x,     (y * TilePixelSize.y) + TilePixelSize.y},  {RightSideUV, TopSideUV} };
            const Vertex2D TopLeftVertex =     { { x * TilePixelSize.x,                        (y * TilePixelSize.y) + TilePixelSize.y},  {LeftSideUV, TopSideUV} };

            vertexList.emplace_back(BottomLeftVertex);
            vertexList.emplace_back(BottomRightVertex);
            vertexList.emplace_back(TopRightVertex);
            vertexList.emplace_back(TopLeftVertex);

            indexList.emplace_back(VertexCount);
            indexList.emplace_back(VertexCount + 1);
            indexList.emplace_back(VertexCount + 2);
            indexList.emplace_back(VertexCount + 2);
            indexList.emplace_back(VertexCount + 3);
            indexList.emplace_back(VertexCount);

            tileMap.emplace_back(tile);
        }
    }

    uint* TileIdMap = memorySystem.AddPtrBuffer<uint>(tileIdMap.size(), __FILE__, __LINE__, __func__);
    Tile* TileMap = memorySystem.AddPtrBuffer<Tile>(tileMap.size(), __FILE__, __LINE__, __func__);
    Vertex2D* VertexList = memorySystem.AddPtrBuffer<Vertex2D>(vertexList.size(), __FILE__, __LINE__, __func__);
    uint32* IndexList = memorySystem.AddPtrBuffer<uint32>(indexList.size(), __FILE__, __LINE__, __func__);

    levelLayout.TileIdMap = tileIdMap.data();
    levelLayout.TileMap = tileMap.data();
    levelLayout.VertexList = vertexList.data();
    levelLayout.IndexList = indexList.data();
    levelLayout.TileIdMapCount = tileIdMap.size();
    levelLayout.TileMapCount = tileMap.size();
    levelLayout.VertexListCount = vertexList.size();
    levelLayout.IndexListCount = indexList.size();

    return levelLayout;
}