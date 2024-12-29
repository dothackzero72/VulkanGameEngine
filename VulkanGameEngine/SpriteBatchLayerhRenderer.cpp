#include "SpriteBatchLayerhRenderer.h"

SpriteBatchLayerhRenderer::SpriteBatchLayerhRenderer()
{
}

SpriteBatchLayerhRenderer::~SpriteBatchLayerhRenderer()
{
}

void SpriteBatchLayerhRenderer::BuildSpriteLayer()
{
	for (const SharedPtr<Sprite> sprite : SpriteDrawList)
	{
		
	/*	
		VertexList { {0.0f              ,  0.0f              },  {0.0f, 0.0f}, {1.0f, 0.0f, 0.0f, 1.0f} },
		  { {sprite->Position.x,  0.0f              },  {1.0f, 0.0f}, {0.0f, 1.0f, 0.0f, 1.0f} },
		  { {sprite->Position.x,  sprite->Position.y},  { 1.0f, 1.0f }, {0.0f, 0.0f, 1.0f, 1.0f} },
		  { {0.0f              ,  sprite->Position.y},  {0.0f, 1.0f}, {1.0f, 1.0f, 0.0f, 1.0f} }
		
		std::vector<uint32> SpriteIndexList =
		{
		  0, 3, 1,
		  1, 3, 2
		};*/
	}
}

void SpriteBatchLayerhRenderer::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
}
