#include "SpriteBatchLayer.h"
#include "GameObjectSystem.h"
#include "BufferSystem.h"
#include "Typedef.h"
#include "MeshSystem.h"
#include "LevelSystem.h"

uint32 SpriteBatchLayer::NextSpriteBatchLayerID = 0;

SpriteBatchLayer::SpriteBatchLayer()
{

}

SpriteBatchLayer::SpriteBatchLayer(VkGuid& renderPassId)
{                            
	SpriteBatchLayerID = ++NextSpriteBatchLayerID;

	for (int x = 0; x < levelSystem.SpriteMap.size(); x++)
	{
		levelSystem.SpriteBatchLayerObjectListMap[SpriteBatchLayerID].emplace_back(GameObjectID(x + 1));
	}

	SpriteLayerMeshId = meshSystem.CreateSpriteLayerMesh<Vertex2D>(gameObjectSystem.SpriteVertexList, gameObjectSystem.SpriteIndexList);
	levelSystem.SpriteInstanceListMap[SpriteBatchLayerID] = Vector<SpriteInstanceStruct>(levelSystem.SpriteBatchLayerObjectListMap[SpriteBatchLayerID].size());
	levelSystem.SpriteInstanceBufferMap[SpriteBatchLayerID] = bufferSystem.CreateVulkanBuffer<SpriteInstanceStruct>(cRenderer, levelSystem.SpriteInstanceListMap[SpriteBatchLayerID], meshSystem.MeshBufferUsageSettings, meshSystem.MeshBufferPropertySettings, false);
}

SpriteBatchLayer::~SpriteBatchLayer()
{
}

void SpriteBatchLayer::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
	levelSystem.SpriteInstanceListMap[SpriteBatchLayerID].clear();
	levelSystem.SpriteInstanceListMap[SpriteBatchLayerID].reserve(levelSystem.SpriteBatchLayerObjectListMap[SpriteBatchLayerID].size());
	for (auto& gameObjectID : levelSystem.SpriteBatchLayerObjectListMap[SpriteBatchLayerID])
	{
		levelSystem.SpriteInstanceListMap[SpriteBatchLayerID].emplace_back(levelSystem.SpriteMap[gameObjectID].Update(commandBuffer, deltaTime));
	}

	if (levelSystem.SpriteBatchLayerObjectListMap[SpriteBatchLayerID].size())
	{
		bufferSystem.UpdateBufferMemory(cRenderer, levelSystem.SpriteInstanceBufferMap[SpriteBatchLayerID], levelSystem.SpriteInstanceListMap[SpriteBatchLayerID]);
	}
}