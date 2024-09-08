#pragma once
#include "Mesh.h"

class Mesh2D : public Mesh
{
private:
protected:
public:
	Mesh2D();
	Mesh2D(List<Vertex2D>& vertexList, List<uint32>& indexList);
	virtual ~Mesh2D();
};

