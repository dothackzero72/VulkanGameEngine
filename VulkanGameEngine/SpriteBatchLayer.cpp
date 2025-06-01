#include "SpriteBatchLayer.h"
#include "AssetManager.h"
#include "VulkanBufferSystem.h"
#include "Typedef.h"
#include "MeshSystem.h"
#include "LevelSystem.h"

uint32 SpriteBatchLayer::NextSpriteBatchLayerID = 0;

SpriteBatchLayer::SpriteBatchLayer()
{

}

SpriteBatchLayer::SpriteBatchLayer(VkGuid& renderPassId)
{
	RenderPassId = renderPassId;                                   
	SpriteBatchLayerID = ++NextSpriteBatchLayerID;

	for (int x = 0; x < levelSystem.SpriteList.size(); x++)
	{
		levelSystem.SpriteBatchLayerObjectList[SpriteBatchLayerID].emplace_back(GameObjectID(x + 1));
	}

	meshSystem.CreateSpriteLayerMesh<Vertex2D>(assetManager.SpriteVertexList, assetManager.SpriteIndexList);
	levelSystem.SpriteInstanceList[SpriteBatchLayerID] = Vector<SpriteInstanceStruct>(levelSystem.SpriteBatchLayerObjectList[SpriteBatchLayerID].size());
	levelSystem.SpriteInstanceBufferList[SpriteBatchLayerID] = bufferSystem.CreateVulkanBuffer<SpriteInstanceStruct>(cRenderer, levelSystem.SpriteInstanceList[SpriteBatchLayerID], meshSystem.MeshBufferUsageSettings, meshSystem.MeshBufferPropertySettings, false);
}

SpriteBatchLayer::~SpriteBatchLayer()
{
}

void SpriteBatchLayer::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
	levelSystem.SpriteInstanceList[SpriteBatchLayerID].clear();
	levelSystem.SpriteInstanceList[SpriteBatchLayerID].reserve(levelSystem.SpriteBatchLayerObjectList[SpriteBatchLayerID].size());
	for (auto& gameObjectID : levelSystem.SpriteBatchLayerObjectList[SpriteBatchLayerID])
	{
		levelSystem.SpriteInstanceList[SpriteBatchLayerID].emplace_back(levelSystem.SpriteList[gameObjectID].Update(commandBuffer, deltaTime));
	}

	if (levelSystem.SpriteBatchLayerObjectList[SpriteBatchLayerID].size())
	{
		bufferSystem.UpdateBufferMemory(cRenderer, levelSystem.SpriteInstanceBufferList[SpriteBatchLayerID], levelSystem.SpriteInstanceList[SpriteBatchLayerID]);
	}
}