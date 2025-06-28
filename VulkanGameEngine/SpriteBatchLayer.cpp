#include "SpriteBatchLayer.h"
#include "GameObjectSystem.h"
#include "BufferSystem.h"
#include "Typedef.h"
#include "MeshSystem.h"
#include "LevelSystem.h"
#include "SpriteSystem.h"

uint32 SpriteBatchLayer::NextSpriteBatchLayerID = 0;

SpriteBatchLayer::SpriteBatchLayer()
{

}

SpriteBatchLayer::SpriteBatchLayer(VkGuid& renderPassId)
{                            
	SpriteBatchLayerID = ++NextSpriteBatchLayerID;

	for (int x = 0; x < spriteSystem.SpriteListRef().size(); x++)
	{
		levelSystem.SpriteBatchLayerObjectListMap[SpriteBatchLayerID].emplace_back(GameObjectID(x + 1));
	}

	SpriteLayerMeshId = meshSystem.CreateSpriteLayerMesh(gameObjectSystem.SpriteVertexList, gameObjectSystem.SpriteIndexList);
	levelSystem.SpriteInstanceListMap[SpriteBatchLayerID] = Vector<SpriteInstanceStruct>(levelSystem.SpriteBatchLayerObjectListMap[SpriteBatchLayerID].size());
	levelSystem.SpriteInstanceBufferMap[SpriteBatchLayerID] = bufferSystem.CreateVulkanBuffer<SpriteInstanceStruct>(renderSystem.renderer, levelSystem.SpriteInstanceListMap[SpriteBatchLayerID], MeshBufferUsageSettings, MeshBufferPropertySettings, false);
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
		levelSystem.SpriteInstanceListMap[SpriteBatchLayerID].emplace_back(*spriteSystem.FindSpriteInstance(gameObjectID));
	}

	if (levelSystem.SpriteBatchLayerObjectListMap[SpriteBatchLayerID].size())
	{
		bufferSystem.UpdateBufferMemory(renderSystem.renderer, levelSystem.SpriteInstanceBufferMap[SpriteBatchLayerID], levelSystem.SpriteInstanceListMap[SpriteBatchLayerID]);
	}
}