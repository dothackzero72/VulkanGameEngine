#include "SpriteBatchLayer.h"
#include "AssetManager.h"
#include "Typedef.h"

uint32 SpriteBatchLayer::NextSpriteBatchLayerID = 0;

SpriteBatchLayer::SpriteBatchLayer()
{

}

SpriteBatchLayer::SpriteBatchLayer(VkGuid& renderPassId)
{
	RenderPassId = renderPassId;                                   
	SpriteBatchLayerID = ++NextSpriteBatchLayerID;

	SpriteLayerMeshId = SpriteMesh::GetNextIdNumber();

	for (int x = 0; x < assetManager.SpriteList.size(); x++)
	{
		assetManager.SpriteBatchLayerObjectList[SpriteBatchLayerID].emplace_back(GameObjectID(x + 1));
	}

	assetManager.SpriteMeshList[SpriteLayerMeshId] = SpriteMesh(assetManager.SpriteVertexList, assetManager.SpriteIndexList, VkGuid());
	assetManager.SpriteInstanceList[SpriteBatchLayerID] = Vector<SpriteInstanceStruct>(assetManager.SpriteBatchLayerObjectList[SpriteBatchLayerID].size());
	assetManager.SpriteInstanceBufferList[SpriteBatchLayerID] = SpriteInstanceBuffer(cRenderer, assetManager.SpriteInstanceList[SpriteBatchLayerID], assetManager.SpriteMeshList[SpriteLayerMeshId].GetMeshBufferUsageSettings(), assetManager.SpriteMeshList[SpriteLayerMeshId].GetMeshBufferPropertySettings(), false);
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
		assetManager.SpriteInstanceBufferList[SpriteBatchLayerID].UpdateBufferMemory(cRenderer, assetManager.SpriteInstanceList[SpriteBatchLayerID]);
	}
}