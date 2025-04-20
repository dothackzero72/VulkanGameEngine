#include "Sprite.h"
#include "Level2DRenderer.h"

Sprite::Sprite()
{
}

Sprite::Sprite(uint32 gameObjectId, uint32 spriteSheetID)
{
	const SpriteSheet spriteSheet = assetManager.SpriteSheetList[spriteSheetID];
	ParentGameObjectID = gameObjectId;
	CurrentSpriteAnimation = assetManager.VRAMSpriteList[0].AnimationList[kWalking];
}

Sprite::~Sprite()
{
}

void Sprite::Input(const float& deltaTime)
{

}

void Sprite::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
	const Transform2DComponent transform2D = assetManager.TransformComponentList[ParentGameObjectID];
	const SpriteSheet spriteSheet = assetManager.SpriteSheetList[assetManager.VRAMSpriteList[0].SpritesheetID];
	Material material = assetManager.MaterialList[assetManager.VRAMSpriteList[0].SpriteMaterialID];

	mat4 spriteMatrix = mat4(1.0f);
	spriteMatrix = glm::translate(spriteMatrix, vec3(transform2D.GameObjectPosition.x, transform2D.GameObjectPosition.y, 0.0f));
	spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2D.GameObjectRotation.x), vec3(1.0f, 0.0f, 0.0f));
	spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2D.GameObjectRotation.y), vec3(0.0f, 1.0f, 0.0f));
	spriteMatrix = glm::rotate(spriteMatrix, glm::radians(0.0f), vec3(0.0f, 0.0f, 1.0f));
	spriteMatrix = glm::scale(spriteMatrix, vec3(transform2D.GameObjectScale.x, transform2D.GameObjectScale.y, 0.0f));

	SpriteInstance.SpritePosition = transform2D.GameObjectPosition;
	SpriteInstance.SpriteSize = assetManager.VRAMSpriteList[0].SpriteSize;
	SpriteInstance.UVOffset = vec4(spriteSheet.SpriteUVSize.x * CurrentSpriteAnimation.FrameList[CurrentFrame].x, spriteSheet.SpriteUVSize.y * CurrentSpriteAnimation.FrameList[CurrentFrame].y, spriteSheet.SpriteUVSize.x, spriteSheet.SpriteUVSize.y);
	SpriteInstance.Color = assetManager.VRAMSpriteList[0].SpriteColor;
	SpriteInstance.MaterialID = material.GetMaterialBufferIndex();
	SpriteInstance.InstanceTransform = spriteMatrix;

	CurrentSpriteAnimation.CurrentFrameTime += deltaTime;
	if (CurrentSpriteAnimation.CurrentFrameTime >= CurrentSpriteAnimation.FrameHoldTime)
	{
		CurrentFrame += 1;
		CurrentSpriteAnimation.CurrentFrameTime = 0.0f;
		if (CurrentFrame > CurrentSpriteAnimation.FrameList.size() - 1)
		{
			CurrentFrame = 0;
		}
	}
}

void Sprite::Destroy()
{
	SpriteAlive = false;
	//SpriteInstance.reset();
}
