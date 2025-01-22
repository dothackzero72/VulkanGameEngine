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

	//VertexList.emplace_back(Vertex2D(vec2(spritePosition.x,				   spritePosition.y + spriteSize.y), vec2( 0.0f, 0.0f ), spriteColor));
	//VertexList.emplace_back(Vertex2D(vec2(spritePosition.x + spriteSize.x, spritePosition.y + spriteSize.y), vec2( 1.0f, 0.0f ), spriteColor));
	//VertexList.emplace_back(Vertex2D(vec2(spritePosition.x + spriteSize.x, spritePosition.y),				 vec2( 1.0f, 1.0f ), spriteColor));
	//VertexList.emplace_back(Vertex2D(vec2(spritePosition.x,				   spritePosition.y),				 vec2( 0.0f, 1.0f ), spriteColor));


	//Vertex2D(vec2(spritePosition.x, spritePosition.y + spriteSize.y), vec2(0.0f, 0.0f), spriteColor)
}

Sprite::~Sprite()
{
}

void Sprite::Input(float deltaTime)
{

}

void Sprite::Update(float deltaTime)
{
	mat4 MeshMatrix = mat4(1.0f);
	MeshMatrix = glm::translate(MeshMatrix, MeshPosition);
	MeshMatrix = glm::rotate(MeshMatrix, glm::radians(MeshRotation.x), vec3(1.0f, 0.0f, 0.0f));
	MeshMatrix = glm::rotate(MeshMatrix, glm::radians(MeshRotation.y), vec3(0.0f, 1.0f, 0.0f));
	MeshMatrix = glm::rotate(MeshMatrix, glm::radians(MeshRotation.z), vec3(0.0f, 0.0f, 1.0f));
	MeshMatrix = glm::scale(MeshMatrix, MeshScale);
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
