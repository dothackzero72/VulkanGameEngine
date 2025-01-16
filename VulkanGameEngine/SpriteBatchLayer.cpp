#include "SpriteBatchLayer.h"
#include "MemoryManager.h"

SpriteBatchLayer::SpriteBatchLayer()
{
	 Material = Material::CreateMaterial("Material1");
	 Material->SetAlbedoMap(MemoryManager::GetTextureList()[0]);

	SpriteDrawList.emplace_back(std::make_shared<Sprite>(Sprite(vec2(0.0f, 0.0f), vec2(1.0f), vec4(0.0, 0.0f, 0.0f, 1.0f), Material)));
	SpriteDrawList.emplace_back(std::make_shared<Sprite>(Sprite(vec2(1.2f, 0.0f), vec2(1.0f), vec4(1.0, 0.0f, 0.0f, 1.0f), Material)));

	AddSprite(SpriteDrawList);

	SpriteLayerMesh = Mesh2D::CreateMesh2D(VertexList, IndexList, Material);
}

SpriteBatchLayer::~SpriteBatchLayer()
{
}

void SpriteBatchLayer::AddSprite(List<SharedPtr<Sprite>>& spriteVertexList)
{
	uint32 baseIndex = 0;
	for(int x = 0; x < SpriteDrawList.size(); x++)
	{
		for (auto& vertex : SpriteDrawList[x]->VertexList)
		{
			VertexList.emplace_back(vertex);
		}

		IndexList.emplace_back(baseIndex + 0);
		IndexList.emplace_back(baseIndex + 1);
		IndexList.emplace_back(baseIndex + 3);
		IndexList.emplace_back(baseIndex + 1);
		IndexList.emplace_back(baseIndex + 2);
		IndexList.emplace_back(baseIndex + 3);
		baseIndex += 6;
	}
}

void SpriteBatchLayer::BuildSpriteLayer(List<SharedPtr<Sprite>>& spriteDrawList)
{
	for (const SharedPtr<Sprite> sprite : SpriteDrawList)
	{
		VertexList = SpriteDrawList[0]->VertexList;
		//IndexList = SpriteDrawList[0]->IndexList;
		//SpriteLayerMesh = std::make_shared<Mesh2D>(Mesh2D(VertexList, IndexList, nullptr));
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
