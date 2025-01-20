#include "Sprite.h"

Sprite::Sprite()
{
}

Sprite::Sprite(vec2 spritePosition, vec2 spriteSize, vec4 spriteColor, SharedPtr<Material> material)
{
	SpriteSize = spriteSize;
	SpriteSheetPtr = std::make_shared<SpriteSheet>(SpriteSheet(material));
	MaterialPtr = material;

	const float LeftSideUV = SpriteSheetPtr->LeftSideUV;
	const float RightSideUV = SpriteSheetPtr->RightSideUV;
	const float TopSideUV = SpriteSheetPtr->TopSideUV;
	const float BottomSideUV = SpriteSheetPtr->BottomSideUV;

	VertexList.emplace_back(Vertex2D(vec2(spritePosition.x,				   spritePosition.y + spriteSize.y), vec2( 0.0f, 0.0f ), spriteColor, static_cast<uint>(material->GetMaterialBufferIndex())));
	VertexList.emplace_back(Vertex2D(vec2(spritePosition.x + spriteSize.x, spritePosition.y + spriteSize.y), vec2( 1.0f, 0.0f ), spriteColor, static_cast<uint>(material->GetMaterialBufferIndex())));
	VertexList.emplace_back(Vertex2D(vec2(spritePosition.x + spriteSize.x, spritePosition.y),				 vec2( 1.0f, 1.0f ), spriteColor, static_cast<uint>(material->GetMaterialBufferIndex())));
	VertexList.emplace_back(Vertex2D(vec2(spritePosition.x,				   spritePosition.y),				 vec2( 0.0f, 1.0f ), spriteColor, static_cast<uint>(material->GetMaterialBufferIndex())));
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
