#include "LevelLayer.h"
#include "RenderSystem.h"

LevelLayer::LevelLayer()
{
}

LevelLayer::~LevelLayer()
{
}

void LevelLayer::LoadLevelTiles()
{
	for (unsigned int x = 0; x < LevelBounds.x; x++)
	{
		for (unsigned int y = 0; y < LevelBounds.y; y++)
		{
			LevelTile tile = TileList[(y * LevelBounds.x) + x];
			const float LefttSideUV =   tile.TileOffset.x * tile.TileOffset.x;
			const float RightSideUV =  (tile.TileOffset.x * tile.TileOffset.x) + tile.TileOffset.x;
			const float TopSideUV =     tile.TileOffset.y * tile.TileOffset.y;
			const float BottomSideUV = (tile.TileOffset.y * tile.TileOffset.y) + tile.TileOffset.y;

			const uint VertexCount = VertexList.size();
			const Vertex2D BottomLeftVertex =  { { x * tile.TileSize.x,                      y * tile.TileSize.y},                     {LefttSideUV, BottomSideUV} };
			const Vertex2D BottomRightVertex = { {(x * tile.TileSize.x) + tile.TileSize.x,   y * tile.TileSize.y},                     {RightSideUV, BottomSideUV} };
			const Vertex2D TopRightVertex =    { {(x * tile.TileSize.x) + tile.TileSize.x,  (y * tile.TileSize.y) + tile.TileSize.y},  {RightSideUV, TopSideUV   } };
			const Vertex2D TopLeftVertex =     { { x * tile.TileSize.x,                     (y * tile.TileSize.y) + tile.TileSize.y},  {LefttSideUV, TopSideUV   } };

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
		}
	}

	renderSystem.LevelLayerMeshList[MeshId] = LevelLayerMesh(renderSystem.SpriteVertexList, renderSystem.SpriteIndexList, MaterialId);
}
