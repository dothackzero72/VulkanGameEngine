#include "Sprite.h"
#include "Level2DRenderer.h"

uint32 Sprite::NextSpriteID = 0;

Sprite::Sprite()
{
}

Sprite::Sprite(uint32 gameObjectId, uint32 SpriteVRAMID)
{
	const SpriteVRAM spriteVRAM = assetManager.VRAMSpriteList[SpriteVRAMID];

	ParentGameObjectID = gameObjectId;
	SpriteID = ++NextSpriteID;
	CurrentSpriteAnimation = spriteVRAM.AnimationList[kWalking];
}

Sprite::~Sprite()
{
}

void Sprite::Input(const float& deltaTime)
{

}

SpriteInstanceStruct Sprite::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
	const Transform2DComponent transform2D = assetManager.TransformComponentList[ParentGameObjectID];
	const SpriteVRAM vRAM = assetManager.VRAMSpriteList[SpriteVRAMID];
	SpriteInstanceStruct spriteInstance;
	Material material = assetManager.MaterialList[assetManager.VRAMSpriteList[SpriteVRAMID].SpriteMaterialID];

	mat4 spriteMatrix = mat4(1.0f);
	spriteMatrix = glm::translate(spriteMatrix, vec3(transform2D.GameObjectPosition.x, transform2D.GameObjectPosition.y, 0.0f));
	spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2D.GameObjectRotation.x), vec3(1.0f, 0.0f, 0.0f));
	spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2D.GameObjectRotation.y), vec3(0.0f, 1.0f, 0.0f));
	spriteMatrix = glm::rotate(spriteMatrix, glm::radians(0.0f), vec3(0.0f, 0.0f, 1.0f));
	spriteMatrix = glm::scale(spriteMatrix, vec3(transform2D.GameObjectScale.x, transform2D.GameObjectScale.y, 0.0f));

	spriteInstance.SpritePosition = transform2D.GameObjectPosition;
	spriteInstance.SpriteSize = assetManager.VRAMSpriteList[SpriteVRAMID].SpriteSize;
	spriteInstance.UVOffset = vec4(vRAM.SpriteUVSize.x * CurrentSpriteAnimation.FrameList[CurrentFrame].x, vRAM.SpriteUVSize.y * CurrentSpriteAnimation.FrameList[CurrentFrame].y, vRAM.SpriteUVSize.x, vRAM.SpriteUVSize.y);
	spriteInstance.Color = assetManager.VRAMSpriteList[SpriteVRAMID].SpriteColor;
	spriteInstance.MaterialID = material.GetMaterialBufferIndex();
	spriteInstance.InstanceTransform = spriteMatrix;

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
	return spriteInstance;
}

void Sprite::Destroy()
{
	SpriteAlive = false;
	//SpriteInstance.reset();
}
