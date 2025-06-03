#include "MeshSystem.h"

uint MeshSystem::NextMeshId = 0;
uint MeshSystem::NextSpriteMeshId;
uint MeshSystem::NextLevelLayerMeshId;

MeshSystem meshSystem = MeshSystem();

MeshSystem::MeshSystem()
{
}

MeshSystem::~MeshSystem()
{
}

const Mesh& MeshSystem::FindMesh(const uint& id)
{
    auto it = MeshMap.find(id);
    if (it != MeshMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("Mesh not found for given GUID");
}

const Mesh& MeshSystem::FindSpriteMesh(const uint& id)
{
    auto it = SpriteMeshMap.find(id);
    if (it != SpriteMeshMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("Sprite Mesh not found for given GUID");
}

const Vector<Mesh>& MeshSystem::FindLevelLayerMeshList(const LevelGuid& guid)
{
    auto it = LevelLayerMeshListMap.find(guid);
    if (it != LevelLayerMeshListMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("Level Layer Mesh not found for given GUID");
}

const Vector<Vertex2D>& MeshSystem::FindVertex2DList(const uint& id)
{
    auto it = Vertex2DListMap.find(id);
    if (it != Vertex2DListMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("Vertex2D not found for given GUID");
}

const Vector<uint>& MeshSystem::FindIndexList(const uint& id)
{
    auto it = IndexListMap.find(id);
    if (it != IndexListMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("IndexList not found for given GUID");
}

const Vector<Mesh>& MeshSystem::MeshList()
{
    Vector<Mesh> meshList;
    for (const auto& meshMap : MeshMap)
    {
        meshList.emplace_back(meshMap.second);
    }
    return meshList;
}

const Vector<Mesh>& MeshSystem::SpriteMeshList()
{
    Vector<Mesh> spriteMeshList;
    for (const auto& spriteMesh : SpriteMeshMap)
    {
        spriteMeshList.emplace_back(spriteMesh.second);
    }
    return spriteMeshList;
}
