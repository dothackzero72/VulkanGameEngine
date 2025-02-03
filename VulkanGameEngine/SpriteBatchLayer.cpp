#include "SpriteBatchLayer.h"
#include "MemoryManager.h"

SpriteBatchLayer::SpriteBatchLayer()
{

}

SpriteBatchLayer::SpriteBatchLayer(SharedPtr<JsonPipeline> spriteRenderPipeline, List<SharedPtr<Sprite>> spriteList)
{
	SpriteRenderPipeline = spriteRenderPipeline;
	SpriteLayerMesh = Mesh2D::CreateMesh2D(SpriteVertexList, SpriteIndexList, nullptr);
	SpriteList = spriteList;
	for (auto& sprite : SpriteList)
	{
		sprite->Update(0.0f);
		SpriteInstanceList.emplace_back(*sprite->GetSpriteInstance().get());
	}
	SpriteBuffer = SpriteInstanceBuffer(SpriteInstanceList, SpriteLayerMesh->GetMeshBufferUsageSettings(), SpriteLayerMesh->GetMeshBufferPropertySettings(), false);
	SortSpritesByLayer(SpriteList);
}

SpriteBatchLayer::~SpriteBatchLayer()
{
}

SharedPtr<SpriteBatchLayer> SpriteBatchLayer::CreateSpriteBatchLayer(SharedPtr<JsonPipeline> spriteRenderPipeline, List<SharedPtr<Sprite>>& spriteList)
{
	SharedPtr<SpriteBatchLayer> spriteBatch = MemoryManager::AllocateSpriteBatchLayer();
	new (spriteBatch.get()) SpriteBatchLayer(spriteRenderPipeline, spriteList);
	return spriteBatch;
}

void SpriteBatchLayer::AddSprite(SharedPtr<Sprite> sprite)
{
	SpriteList.emplace_back(sprite);
	SortSpritesByLayer(SpriteList);
}

void SpriteBatchLayer::RemoveSprite(SharedPtr<Sprite> sprite)
{
	sprite->Destroy();
}

void SpriteBatchLayer::Update(float deltaTime)
{
	SpriteInstanceList.clear();
	SpriteInstanceList.reserve(SpriteList.size());

	for (auto& sprite : SpriteList)
	{
		sprite->Update(deltaTime);
		SpriteInstanceList.emplace_back(*sprite->GetSpriteInstance().get());
	}
	SpriteBuffer.UpdateBufferMemory(SpriteInstanceList);
}

void SpriteBatchLayer::Draw(VkCommandBuffer& commandBuffer, SceneDataBuffer& sceneProperties)
{
	VkDeviceSize offsets[] = { 0 };
	vkCmdPushConstants(commandBuffer, SpriteRenderPipeline->PipelineLayout, VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT, 0, sizeof(SceneDataBuffer), &sceneProperties);
	vkCmdBindPipeline(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, SpriteRenderPipeline->Pipeline);
	vkCmdBindDescriptorSets(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, SpriteRenderPipeline->PipelineLayout, 0, 1, &SpriteRenderPipeline->DescriptorSetList[0], 0, nullptr);
	vkCmdBindVertexBuffers(commandBuffer, 0, 1, SpriteLayerMesh->GetVertexBuffer().get(), offsets);
	vkCmdBindVertexBuffers(commandBuffer, 1, 1, &SpriteBuffer.Buffer, offsets);
	vkCmdBindIndexBuffer(commandBuffer, *SpriteLayerMesh->GetIndexBuffer().get(), 0, VK_INDEX_TYPE_UINT32);
	vkCmdDrawIndexed(commandBuffer, SpriteIndexList.size(), SpriteInstanceList.size(), 0, 0, 0);
}

void SpriteBatchLayer::Destroy()
{
	SpriteRenderPipeline.reset();
	SpriteLayerMesh->Destroy();
}
