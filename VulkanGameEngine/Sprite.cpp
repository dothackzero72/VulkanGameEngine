#include "Sprite.h"
#include "Level2DRenderer.h"
#include "AssetManager.h"

uint32 Sprite::NextSpriteID = 0;

Sprite::Sprite()
{
}

Sprite::Sprite(uint32 gameObjectId, uint32 SpriteVRAMID)
{
	const SpriteVRAM spriteVRAM = assetManager.VRAMSpriteList[SpriteVRAMID];

	ParentGameObjectID = gameObjectId;
	SpriteID = ++NextSpriteID;
	CurrentAnimationID = kWalking;
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
	const SpriteVRAM vram = assetManager.VRAMSpriteList[SpriteVRAMID];
	const Animation2D animation = assetManager.AnimationList[vram.AnimationListID];
	const Vector<ivec2> frameList = assetManager.AnimationFrameList[animation.AnimationFrameId];
	const Material material = assetManager.MaterialList[assetManager.VRAMSpriteList[SpriteVRAMID].SpriteMaterialID];
	SpriteInstanceStruct spriteInstance;

	mat4 spriteMatrix = mat4(1.0f);
	spriteMatrix = glm::translate(spriteMatrix, vec3(transform2D.GameObjectPosition.x, transform2D.GameObjectPosition.y, 0.0f));
	//spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2D.GameObjectRotation.x), vec3(1.0f, 0.0f, 0.0f));
	//spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2D.GameObjectRotation.y), vec3(0.0f, 1.0f, 0.0f));
	//spriteMatrix = glm::rotate(spriteMatrix, glm::radians(0.0f), vec3(0.0f, 0.0f, 1.0f));
	spriteMatrix = glm::scale(spriteMatrix, vec3(transform2D.GameObjectScale.x, transform2D.GameObjectScale.y, 0.0f));

	spriteInstance.SpritePosition = transform2D.GameObjectPosition;
	spriteInstance.SpriteSize = assetManager.VRAMSpriteList[SpriteVRAMID].SpriteSize;
	spriteInstance.UVOffset = vec4(vram.SpriteUVSize.x * frameList[CurrentFrame].x, vram.SpriteUVSize.y * frameList[CurrentFrame].y, vram.SpriteUVSize.x, vram.SpriteUVSize.y);
	spriteInstance.Color = assetManager.VRAMSpriteList[SpriteVRAMID].SpriteColor;
	spriteInstance.MaterialID = material.MaterialBufferIndex;
	spriteInstance.InstanceTransform = spriteMatrix;

	CurrentFrameTime += deltaTime;
	if (CurrentFrameTime >= animation.FrameHoldTime)
	{
		CurrentFrame += 1;
		CurrentFrameTime = 0.0f;
		if (CurrentFrame > frameList.size() - 1)
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
