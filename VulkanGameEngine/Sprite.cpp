#include "Sprite.h"

Sprite::Sprite()
{
}

Sprite::Sprite(vec2 spritePosition, vec2 spriteSize, vec4 spriteColor, SharedPtr<Material> material)
{
	SpriteSize = spriteSize;
	SpriteSheetPtr = std::make_shared<SpriteSheet>(SpriteSheet(material));
	const float LeftSideUV = SpriteSheetPtr->LeftSideUV;
	const float RightSideUV = SpriteSheetPtr->RightSideUV;
	const float TopSideUV = SpriteSheetPtr->TopSideUV;
	const float BottomSideUV = SpriteSheetPtr->BottomSideUV;

	//{ { 0.0f, 0.5f }, { 0.0f, 0.0f }, { 1.0f, 0.0f, 0.0f, 1.0f } },
	//{ {0.5f, 0.5f},  {1.0f, 0.0f}, {0.0f, 1.0f, 0.0f, 1.0f} },
	//{ {0.5f, 0.0f},  {1.0f, 1.0f}, {0.0f, 0.0f, 1.0f, 1.0f} },
	//{ {0.0f, 0.0f},  {0.0f, 1.0f}, {1.0f, 1.0f, 0.0f, 1.0f} }

	VertexList.emplace_back(Vertex2D(vec2(0.0f			  , spritePosition.y), vec2(0.0f, 0.0f), SpriteColor, 0));
	VertexList.emplace_back(Vertex2D(vec2(spritePosition.x, spritePosition.y), vec2(1.0f, 0.0f), SpriteColor, 0));
	VertexList.emplace_back(Vertex2D(vec2(spritePosition.x, 0.0f), vec2(1.0f, 1.0f), SpriteColor, 0));
	VertexList.emplace_back(Vertex2D(vec2(0.0f			  , 0.0f), vec2(0.0f, 1.0f), SpriteColor, 0));
}

Sprite::~Sprite()
{
}

void Sprite::Input(float deltaTime)
{

}

void Sprite::Update(float deltaTime)
{
}

void Sprite::BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
{
}

void Sprite::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
}

void Sprite::Destroy()
{
}
