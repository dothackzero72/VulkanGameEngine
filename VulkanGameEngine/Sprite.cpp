#include "Sprite.h"

Sprite::Sprite()
{
}

Sprite::Sprite(SharedPtr<GameObject> parentGameObject, SpriteSheet& spriteSheet)
{
	ParentGameObject = parentGameObject;
	Transform2D = std::dynamic_pointer_cast<Transform2DComponent>(ParentGameObject->GetComponentByComponentType(ComponentTypeEnum::kTransform2DComponent));

	SpriteLayer = spriteSheet.GetSpriteLayer();
	SpriteSize = vec2(spriteSheet.GetSpritePixelSize().x, spriteSheet.GetSpritePixelSize().y);
	SpriteColor = vec4(0.0f, 0.0f, 0.0f, 1.0f);
	SpriteMaterial = spriteSheet.GetSpriteMaterial();
	SpriteInstance = std::make_shared<SpriteInstanceStruct>(SpriteInstanceStruct());
}

Sprite::~Sprite()
{
}

void Sprite::Input(const float& deltaTime)
{

}

void Sprite::Update(const float& deltaTime)
{
	mat4 spriteMatrix = mat4(1.0f);
	spriteMatrix = glm::translate(spriteMatrix, vec3(Transform2D->GameObjectPosition.x, Transform2D->GameObjectPosition.y, 0.0f));
	spriteMatrix = glm::rotate(spriteMatrix, glm::radians(Transform2D->GameObjectRotation.x), vec3(1.0f, 0.0f, 0.0f));
	spriteMatrix = glm::rotate(spriteMatrix, glm::radians(Transform2D->GameObjectRotation.y), vec3(0.0f, 1.0f, 0.0f));
	spriteMatrix = glm::rotate(spriteMatrix, glm::radians(0.0f), vec3(0.0f, 0.0f, 1.0f));
	spriteMatrix = glm::scale(spriteMatrix, vec3(Transform2D->GameObjectScale.x, Transform2D->GameObjectScale.y, 0.0f));

	vec2 spriteUVOffset = vec2(0.058823529411765f, 1.0f);
	SpriteInstance->SpritePosition = Transform2D->GameObjectPosition;
	SpriteInstance->SpriteSize = SpriteSize;
	SpriteInstance->UVOffset = vec4(spriteUVOffset.x * 3, 0.0f, spriteUVOffset.x, spriteUVOffset.y);
	SpriteInstance->Color = SpriteColor;
	SpriteInstance->MaterialID = (SpriteMaterial) ? SpriteMaterial->GetMaterialBufferIndex() : 0;
	SpriteInstance->InstanceTransform = spriteMatrix;
}

void Sprite::BufferUpdate(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
}

void Sprite::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
}

void Sprite::Destroy()
{
	SpriteAlive = false;
	SpriteInstance.reset();
}
