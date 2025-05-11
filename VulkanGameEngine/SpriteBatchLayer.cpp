#include "SpriteBatchLayer.h"
#include "RenderSystem.h"

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
		renderSystem.SpriteBatchLayerObjectList[SpriteBatchLayerID].emplace_back(GameObjectID(x + 1));
	}
	renderSystem.SpriteMeshList[SpriteLayerMeshId] = SpriteMesh(renderSystem.SpriteVertexList, renderSystem.SpriteIndexList, VkGuid());
	renderSystem.SpriteInstanceList[SpriteBatchLayerID] = Vector<SpriteInstanceStruct>(renderSystem.SpriteBatchLayerObjectList[SpriteBatchLayerID].size());
	renderSystem.SpriteInstanceBufferList[SpriteBatchLayerID] = SpriteInstanceBuffer(renderSystem.SpriteInstanceList[SpriteBatchLayerID], renderSystem.SpriteMeshList[SpriteLayerMeshId].GetMeshBufferUsageSettings(), renderSystem.SpriteMeshList[SpriteLayerMeshId].GetMeshBufferPropertySettings(), false);
}

SpriteBatchLayer::~SpriteBatchLayer()
{
}

void SpriteBatchLayer::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
	renderSystem.SpriteInstanceList[SpriteBatchLayerID].clear();
	renderSystem.SpriteInstanceList[SpriteBatchLayerID].reserve(renderSystem.SpriteBatchLayerObjectList[SpriteBatchLayerID].size());
	for (auto& gameObjectID : renderSystem.SpriteBatchLayerObjectList[SpriteBatchLayerID])
	{
		renderSystem.SpriteInstanceList[SpriteBatchLayerID].emplace_back(assetManager.SpriteList[gameObjectID].Update(commandBuffer, deltaTime));
	}

	if (renderSystem.SpriteBatchLayerObjectList[SpriteBatchLayerID].size())
	{
		renderSystem.SpriteInstanceBufferList[SpriteBatchLayerID].UpdateBufferMemory(renderSystem.SpriteInstanceList[SpriteBatchLayerID]);
	}
}