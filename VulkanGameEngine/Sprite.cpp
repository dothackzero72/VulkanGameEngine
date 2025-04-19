#include "Sprite.h"
#include "Level2DRenderer.h"

Sprite::Sprite()
{
}

Sprite::Sprite(uint32 gameObjectId, uint32 spriteSheetID)
{
	const SpriteSheet spriteSheet = assetManager.SpriteSheetList[spriteSheetID];

	ParentGameObjectID = gameObjectId;
	SpritesheetID = spriteSheetID;
	SpriteLayer = spriteSheet.SpriteLayer;
	SpriteSize = vec2(spriteSheet.SpritePixelSize.x * spriteSheet.SpriteScale.x, spriteSheet.SpritePixelSize.y * spriteSheet.SpriteScale.y);
	SpriteColor = vec4(0.0f, 0.0f, 0.0f, 1.0f);
	SpriteMaterialID = spriteSheet.SpriteMaterialID;
	SpriteInstance = std::make_shared<SpriteInstanceStruct>(SpriteInstanceStruct());

	Vector<ivec2> frameList =
	{
		ivec2(0, 0),
		ivec2(1, 0)
	};

	AnimationList[kStanding] = Animation2D("Standing", frameList, 0.2f);

	Vector<ivec2> frameList2 =
	{
		ivec2(3, 0),
		ivec2(4, 0),
		ivec2(5, 0),
		ivec2(4, 0)
	};
	AnimationList[kWalking] = Animation2D("Walking", frameList2, 0.5f);
	CurrentSpriteAnimation = AnimationList[kWalking];
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
	const SpriteSheet spriteSheet = assetManager.SpriteSheetList[SpritesheetID];
	Material material = assetManager.MaterialList[SpriteMaterialID];

	mat4 spriteMatrix = mat4(1.0f);
	spriteMatrix = glm::translate(spriteMatrix, vec3(transform2D.GameObjectPosition.x, transform2D.GameObjectPosition.y, 0.0f));
	spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2D.GameObjectRotation.x), vec3(1.0f, 0.0f, 0.0f));
	spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2D.GameObjectRotation.y), vec3(0.0f, 1.0f, 0.0f));
	spriteMatrix = glm::rotate(spriteMatrix, glm::radians(0.0f), vec3(0.0f, 0.0f, 1.0f));
	spriteMatrix = glm::scale(spriteMatrix, vec3(transform2D.GameObjectScale.x, transform2D.GameObjectScale.y, 0.0f));

	SpriteInstance->SpritePosition = transform2D.GameObjectPosition;
	SpriteInstance->SpriteSize = SpriteSize;
	SpriteInstance->UVOffset = vec4(spriteSheet.SpriteUVSize.x * CurrentSpriteAnimation.FrameList[CurrentFrame].x, spriteSheet.SpriteUVSize.y * CurrentSpriteAnimation.FrameList[CurrentFrame].y, spriteSheet.SpriteUVSize.x, spriteSheet.SpriteUVSize.y);
	SpriteInstance->Color = SpriteColor;
	SpriteInstance->MaterialID = material.GetMaterialBufferIndex();
	SpriteInstance->InstanceTransform = spriteMatrix;

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
	SpriteInstance.reset();
}
