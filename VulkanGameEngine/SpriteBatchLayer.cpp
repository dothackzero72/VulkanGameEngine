#include "SpriteBatchLayer.h"
#include "MemoryManager.h"

SpriteBatchLayer::SpriteBatchLayer()
{
	material = Material::CreateMaterial("Material1");
	material->SetAlbedoMap(MemoryManager::GetTextureList()[0]);

	 Material2 = Material::CreateMaterial("Material2");
	 Material2->SetAlbedoMap(MemoryManager::GetTextureList()[1]);


	SpriteDrawList.emplace_back(std::make_shared<Sprite>(Sprite(vec2(0.0f, 0.0f),   vec2(128.0f, 256.0f), vec4(0.0, 0.0f, 0.0f, 1.0f), material)));
	SpriteDrawList.emplace_back(std::make_shared<Sprite>(Sprite(vec2(128.0f, 0.0f), vec2(128.0f, 256.0f), vec4(1.0, 0.0f, 0.0f, 1.0f), Material2)));
	SpriteDrawList.emplace_back(std::make_shared<Sprite>(Sprite(vec2(256.0f, 0.0f), vec2(128.0f, 256.0f), vec4(1.0, 0.0f, 0.0f, 1.0f), material)));
	AddSprite(SpriteDrawList);

	SpriteLayerMesh = Mesh2D::CreateMesh2D(VertexList, IndexList, material);
}

SpriteBatchLayer::~SpriteBatchLayer()
{
}

void SpriteBatchLayer::AddSprite(List<SharedPtr<Sprite>>& spriteVertexList)
{
	SpriteInstanceStruct spriteInstance;
	spriteInstance.

    uint32_t baseIndex = VertexList.size();
    for (auto& sprite : spriteVertexList)
    {
        for (const auto& vertex : sprite->VertexList)
        {
            VertexList.push_back(vertex);
        }

        IndexList.emplace_back(baseIndex + 0); 
        IndexList.emplace_back(baseIndex + 1);
        IndexList.emplace_back(baseIndex + 3); 
        IndexList.emplace_back(baseIndex + 1);
        IndexList.emplace_back(baseIndex + 2); 
        IndexList.emplace_back(baseIndex + 3); 

        baseIndex += 4;
    }
}

void SpriteBatchLayer::BuildSpriteLayer(List<SharedPtr<Sprite>>& spriteDrawList)
{
	for (const SharedPtr<Sprite> sprite : SpriteDrawList)
	{
		//VertexList = SpriteDrawList[0]->VertexList;
		//IndexList = SpriteDrawList[0]->IndexList;
		//SpriteLayerMesh = std::make_shared<Mesh2D>(Mesh2D(VertexList, IndexList, nullptr));
	}
}

void SpriteBatchLayer::Update(float deltaTime)
{
	VertexList.clear();
	IndexList.clear();

	uint32_t baseIndex = VertexList.size();
	for (auto& sprite : SpriteDrawList)
	{
		for (const auto& vertex : sprite->VertexList)
		{
			VertexList.push_back(vertex);
		}

		IndexList.emplace_back(baseIndex + 0);
		IndexList.emplace_back(baseIndex + 1);
		IndexList.emplace_back(baseIndex + 3);
		IndexList.emplace_back(baseIndex + 1);
		IndexList.emplace_back(baseIndex + 2);
		IndexList.emplace_back(baseIndex + 3);

		baseIndex += 4;
	}
	SpriteLayerMesh->VertexBufferUpdate(deltaTime, VertexList, IndexList);
}

void SpriteBatchLayer::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
	SpriteLayerMesh->Draw(commandBuffer, pipeline, pipelineLayout, descriptorSet, sceneProperties);
}

void SpriteBatchLayer::Destroy()
{
	SpriteLayerMesh->Destroy();
	material->Destroy();
	VertexList.clear();
	IndexList.clear();
}
