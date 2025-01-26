#include "Sprite.h"

Sprite::Sprite()
{
}

Sprite::Sprite(vec2 spritePosition, vec2 spriteSize, vec4 spriteColor, SharedPtr<Material> material)
{
	SpriteSize = spriteSize;
	SpriteColor = spriteColor;
	SpriteMaterial = material;

	SpriteSheetPtr = std::make_shared<SpriteSheet>(SpriteSheet(material));
	SpriteInstance = std::make_shared<SpriteInstanceStruct>(SpriteInstanceStruct());
}

Sprite::~Sprite()
{
}

void Sprite::Input(float deltaTime)
{

}

void Sprite::Update(float deltaTime)
{
	mat4 spriteMatrix = mat4(1.0f);
	spriteMatrix = glm::translate(spriteMatrix, vec3(SpritePosition.x, SpritePosition.y, 0.0f));
	spriteMatrix = glm::rotate(spriteMatrix, glm::radians(SpriteRotation.x), vec3(1.0f, 0.0f, 0.0f));
	spriteMatrix = glm::rotate(spriteMatrix, glm::radians(SpriteRotation.y), vec3(0.0f, 1.0f, 0.0f));
	spriteMatrix = glm::rotate(spriteMatrix, glm::radians(0.0f), vec3(0.0f, 0.0f, 1.0f));
	spriteMatrix = glm::scale(spriteMatrix, vec3(SpriteScale.x, SpriteScale.y, 0.0f));

	SpriteInstance->SpriteSize = SpriteSize;
	SpriteInstance->UVOffset = vec2(0.0f, 0.0f);
	SpriteInstance->Color = SpriteColor;
	SpriteInstance->MaterialID = (SpriteMaterial) ? SpriteMaterial->GetMaterialBufferIndex() : 0;
	SpriteInstance->InstanceTransform = spriteMatrix;
}

void Sprite::BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
{
}

void Sprite::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
}

void Sprite::Destroy()
{
	SpriteInstance.reset();
}
