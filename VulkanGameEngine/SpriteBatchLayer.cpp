#include "SpriteBatchLayer.h"
#include "MemoryManager.h"

SpriteBatchLayer::SpriteBatchLayer()
{
	 Material = Material::CreateMaterial("Material1");
	 Material->SetAlbedoMap(MemoryManager::GetTextureList()[0]);

	SpriteDrawList.emplace_back(std::make_shared<Sprite>(Sprite()));
	VertexList = SpriteDrawList[0]->VertexList;
	IndexList = SpriteDrawList[0]->IndexList;
	SpriteLayerMesh = Mesh2D::CreateMesh2D(VertexList, IndexList, Material);
}

SpriteBatchLayer::~SpriteBatchLayer()
{
}

void SpriteBatchLayer::BuildSpriteLayer(List<SharedPtr<Sprite>>& spriteDrawList)
{
	for (const SharedPtr<Sprite> sprite : SpriteDrawList)
	{
		VertexList = SpriteDrawList[0]->VertexList;
		IndexList = SpriteDrawList[0]->IndexList;
		SpriteLayerMesh = std::make_shared<Mesh2D>(Mesh2D(VertexList, IndexList, nullptr));
	}
}

void SpriteBatchLayer::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
	SpriteLayerMesh->Draw(commandBuffer, pipeline, pipelineLayout, descriptorSet, sceneProperties);
}

void SpriteBatchLayer::Destroy()
{
	SpriteLayerMesh->Destroy();
	Material->Destroy();
	VertexList.clear();
	IndexList.clear();
}
