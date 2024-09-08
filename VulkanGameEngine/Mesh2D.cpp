#include "Mesh2D.h"

Mesh2D::Mesh2D() : Mesh()
{
}

Mesh2D::Mesh2D(List<Vertex2D>& vertexList, List<uint32>& indexList) : Mesh()
{
	std::vector<Vertex2D> SpriteVertexList =
	{
	  { {0.0f, 0.5f},  {0.0f, 0.0f}, {1.0f, 0.0f, 0.0f, 1.0f} },
	  { {0.5f, 0.5f},  {1.0f, 0.0f}, {0.0f, 1.0f, 0.0f, 1.0f} },
	  { {0.5f, 0.0f},  {1.0f, 1.0f}, {0.0f, 0.0f, 1.0f, 1.0f} },
	  { {0.0f, 0.0f},  {0.0f, 1.0f}, {1.0f, 1.0f, 0.0f, 1.0f} }
	};
	std::vector<uint32> SpriteIndexList = 
	{
	  0, 1, 3,
	  1, 2, 3
	};
	MeshStartUp<Vertex2D>(SpriteVertexList, SpriteIndexList);
}

Mesh2D::~Mesh2D()
{
}
