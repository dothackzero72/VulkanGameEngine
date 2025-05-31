#include "SpriteBatchLayer.h"
#include "AssetManager.h"
#include "VulkanBufferSystem.h"
#include "Typedef.h"
#include "MeshSystem.h"

uint32 SpriteBatchLayer::NextSpriteBatchLayerID = 0;

SpriteBatchLayer::SpriteBatchLayer()
{

}

SpriteBatchLayer::SpriteBatchLayer(VkGuid& renderPassId)
{
	RenderPassId = renderPassId;                                   
	SpriteBatchLayerID = ++NextSpriteBatchLayerID;

	for (int x = 0; x < assetManager.SpriteList.size(); x++)
	{
		assetManager.SpriteBatchLayerObjectList[SpriteBatchLayerID].emplace_back(GameObjectID(x + 1));
	}

	assetManager.SpriteMeshList[SpriteLayerMeshId] = meshSystem. (assetManager.SpriteVertexList, assetManager.SpriteIndexList, VkGuid());
	assetManager.SpriteInstanceList[SpriteBatchLayerID] = Vector<SpriteInstanceStruct>(assetManager.SpriteBatchLayerObjectList[SpriteBatchLayerID].size());
	assetManager.SpriteInstanceBufferList[SpriteBatchLayerID] = bufferSystem.CreateVulkanBuffer<SpriteInstanceStruct>(cRenderer, assetManager.SpriteInstanceList[SpriteBatchLayerID], assetManager.SpriteMeshList[SpriteLayerMeshId].MeshBufferUsageSettings, assetManager.SpriteMeshList[SpriteLayerMeshId].MeshBufferPropertySettings, false);
}

SpriteBatchLayer::~SpriteBatchLayer()
{
}

void SpriteBatchLayer::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
	assetManager.SpriteInstanceList[SpriteBatchLayerID].clear();
	assetManager.SpriteInstanceList[SpriteBatchLayerID].reserve(assetManager.SpriteBatchLayerObjectList[SpriteBatchLayerID].size());
	for (auto& gameObjectID : assetManager.SpriteBatchLayerObjectList[SpriteBatchLayerID])
	{
		assetManager.SpriteInstanceList[SpriteBatchLayerID].emplace_back(assetManager.SpriteList[gameObjectID].Update(commandBuffer, deltaTime));
	}

	if (assetManager.SpriteBatchLayerObjectList[SpriteBatchLayerID].size())
	{
		bufferSystem.UpdateBufferMemory(cRenderer, assetManager.SpriteInstanceBufferList[SpriteBatchLayerID], assetManager.SpriteInstanceList[SpriteBatchLayerID]);
	}
}