#include "SpriteBatchLayer.h"
#include "MemoryManager.h"

SpriteBatchLayer::SpriteBatchLayer()
{
	SpriteLayerMesh = Mesh2D::CreateMesh2D(SpriteVertexList, SpriteIndexList, nullptr);
	SpriteBuffer = std::make_shared<SpriteInstanceBuffer>(SpriteInstanceBuffer(SpriteInstanceList, SpriteLayerMesh->GetMeshBufferUsageSettings(), SpriteLayerMesh->GetMeshBufferPropertySettings(), false));
}

SpriteBatchLayer::~SpriteBatchLayer()
{
}

void SpriteBatchLayer::AddSprite(SharedPtr<Sprite> sprite)
{
	SpriteList.emplace_back(sprite);
}

void SpriteBatchLayer::Update(float deltaTime)
{
	SpriteInstanceList.clear();
	SpriteInstanceList.reserve(SpriteList.size());
	for (auto& sprite : SpriteList)
	{
		sprite->Update(deltaTime);
		SpriteInstanceList.emplace_back(sprite);
	}
	SpriteBuffer->UpdateBufferMemory(SpriteInstanceList);
}

void SpriteBatchLayer::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
	SpriteLayerMesh->Draw(commandBuffer, pipeline, pipelineLayout, descriptorSet, sceneProperties);
}

void SpriteBatchLayer::Destroy()
{
	SpriteLayerMesh->Destroy();
}
