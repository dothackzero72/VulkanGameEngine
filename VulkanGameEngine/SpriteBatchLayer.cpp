#include "SpriteBatchLayer.h"
#include "RenderSystem.h"

uint32 SpriteBatchLayer::NextSpriteBatchLayerID = 0;

SpriteBatchLayer::SpriteBatchLayer()
{

}

SpriteBatchLayer::SpriteBatchLayer(uint32 renderPassId)
{
	RenderPassId = renderPassId;
	SpriteBatchLayerID = ++NextSpriteBatchLayerID;

	SpriteLayerMeshId = SpriteMesh::GetNextIdNumber();
	renderSystem.SpriteMeshList[SpriteLayerMeshId] = SpriteMesh(renderSystem.SpriteVertexList, renderSystem.SpriteIndexList, 0);
	renderSystem.SpriteInstanceList[SpriteBatchLayerID] = Vector<SpriteInstanceStruct>(renderSystem.SpriteBatchLayerObjectList[RenderPassId].size());
	renderSystem.SpriteInstanceBufferList[SpriteBatchLayerID] = SpriteInstanceBuffer(renderSystem.SpriteInstanceList[SpriteBatchLayerID], renderSystem.SpriteMeshList[SpriteLayerMeshId].GetMeshBufferUsageSettings(), renderSystem.SpriteMeshList[SpriteLayerMeshId].GetMeshBufferPropertySettings(), false);
	SortSpritesByLayer();
}

SpriteBatchLayer::~SpriteBatchLayer()
{
}

void SpriteBatchLayer::AddSprite(uint gameObjectID)
{
	//GameObjectIDList.emplace_back(gameObjectID);
	//SortSpritesByLayer(GameObjectIDList);
}

void SpriteBatchLayer::RemoveSprite(uint gameObjectID)
{
	//sprite->Destroy();
	//GameObjectIDList.erase(std::remove_if(GameObjectIDList.begin(), GameObjectIDList.end(),
	//	[&sprite](const uint32& compairGameObjectID = gameObjectID) {
	//		return compairGameObjectID == gameObjectID;
	//	}),
	//	GameObjectIDList.end());
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

void SpriteBatchLayer::Destroy()
{
	renderSystem.SpriteMeshList[SpriteLayerMeshId].Destroy();
}

void SpriteBatchLayer::SortSpritesByLayer()
{
	//std::sort(sprites.begin(), sprites.end(), [](const SharedPtr<Sprite>& spriteA, const SharedPtr<Sprite>& spriteB) {
	//		if (spriteA &&
	//			spriteB)
	//		{
	//			return spriteA->SpriteLayer > spriteB->SpriteLayer;
	//		}
	//		return true;
	//	});
}
