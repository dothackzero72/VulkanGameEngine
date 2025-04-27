#include "SpriteBatchLayer.h"
#include "RenderSystem.h"

uint32 SpriteBatchLayer::NextSpriteBatchLayerID = 0;

SpriteBatchLayer::SpriteBatchLayer()
{

}

SpriteBatchLayer::SpriteBatchLayer(Vector<GameObject>& gameObjectList, JsonPipeline spriteRenderPipeline)
{
	SpriteBatchLayerID = ++NextSpriteBatchLayerID;
	SpriteRenderPipeline = spriteRenderPipeline;

	SpriteLayerMeshId = SpriteMesh::GetNextIdNumber();
	assetManager.MeshList[SpriteLayerMeshId] = SpriteMesh(SpriteVertexList, SpriteIndexList, 0);

	for (auto& gameObject : gameObjectList)
	{
		Sprite sprite = assetManager.SpriteList[gameObject.GameObjectId];
		GameObjectIDList.emplace_back(gameObject.GameObjectId);
	}

	renderSystem.SpriteInstanceList[SpriteBatchLayerID] = Vector<SpriteInstanceStruct>(gameObjectList.size());
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
	spriteInstanceList.reserve(GameObjectIDList.size());
	for (auto& gameObjectID : GameObjectIDList)
	{
		spriteInstanceList.emplace_back(assetManager.SpriteList[gameObjectID].Update(commandBuffer, deltaTime));
	}

	if (GameObjectIDList.size())
	{
		spriteInstanceBuffer.UpdateBufferMemory(spriteInstanceList);
	}
}

void SpriteBatchLayer::Draw(VkCommandBuffer& commandBuffer, SceneDataBuffer& sceneDataBuffer)
{
	const Vector<SpriteInstanceStruct> spriteInstanceList = renderSystem.SpriteInstanceList[SpriteBatchLayerID];
	const SpriteInstanceBuffer spriteInstanceBuffer = renderSystem.SpriteInstanceBufferList[SpriteBatchLayerID];

	VkDeviceSize offsets[] = { 0 };
	vkCmdPushConstants(commandBuffer, SpriteRenderPipeline.PipelineLayout, VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT, 0, sizeof(SceneDataBuffer), &sceneDataBuffer);
	vkCmdBindPipeline(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, SpriteRenderPipeline.Pipeline);
	vkCmdBindDescriptorSets(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, SpriteRenderPipeline.PipelineLayout, 0, SpriteRenderPipeline.DescriptorSetList.size(), SpriteRenderPipeline.DescriptorSetList.data(), 0, nullptr);
	vkCmdBindVertexBuffers(commandBuffer, 0, 1, assetManager.MeshList[SpriteLayerMeshId].GetVertexBuffer().get(), offsets);
	vkCmdBindVertexBuffers(commandBuffer, 1, 1, &spriteInstanceBuffer.Buffer, offsets);
	vkCmdBindIndexBuffer(commandBuffer, *assetManager.MeshList[SpriteLayerMeshId].GetIndexBuffer().get(), 0, VK_INDEX_TYPE_UINT32);
	vkCmdDrawIndexed(commandBuffer, SpriteIndexList.size(), spriteInstanceList.size(), 0, 0, 0);
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
