#include "SpriteBatchLayer.h"
#include "RenderSystem.h"

uint32 SpriteBatchLayer::NextSpriteBatchLayerID = 0;

SpriteBatchLayer::SpriteBatchLayer()
{

}

SpriteBatchLayer::SpriteBatchLayer(uint32 renderPassId, JsonPipeline spriteRenderPipeline)
{
	RenderPassId = renderPassId;
	SpriteBatchLayerID = ++NextSpriteBatchLayerID;

	SpriteLayerMeshId = SpriteMesh::GetNextIdNumber();
	assetManager.MeshList[SpriteLayerMeshId] = SpriteMesh(renderSystem.SpriteVertexList, renderSystem.SpriteIndexList, 0);
	renderSystem.SpriteInstanceList[SpriteBatchLayerID] = Vector<SpriteInstanceStruct>(renderSystem.SpriteBatchLayerObjectList[RenderPassId].size());
	renderSystem.SpriteInstanceBufferList[SpriteBatchLayerID] = SpriteInstanceBuffer(renderSystem.SpriteInstanceList[SpriteBatchLayerID], assetManager.MeshList[SpriteLayerMeshId].GetMeshBufferUsageSettings(), assetManager.MeshList[SpriteLayerMeshId].GetMeshBufferPropertySettings(), false);
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
	Vector<SpriteInstanceStruct> spriteInstanceList = renderSystem.SpriteInstanceList[SpriteBatchLayerID];
	SpriteInstanceBuffer spriteInstanceBuffer = renderSystem.SpriteInstanceBufferList[SpriteBatchLayerID];

	spriteInstanceList.clear();
	spriteInstanceList.reserve(renderSystem.SpriteBatchLayerObjectList[RenderPassId].size());
	for (auto& gameObjectID : renderSystem.SpriteBatchLayerObjectList[RenderPassId])
	{
		spriteInstanceList.emplace_back(assetManager.SpriteList[gameObjectID].Update(commandBuffer, deltaTime));
	}

	if (renderSystem.SpriteBatchLayerObjectList[RenderPassId].size())
	{
		spriteInstanceBuffer.UpdateBufferMemory(spriteInstanceList);
	}
}

void SpriteBatchLayer::Destroy()
{
	assetManager.MeshList[SpriteLayerMeshId].Destroy();
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
