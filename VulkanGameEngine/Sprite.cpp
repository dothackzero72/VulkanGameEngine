#include "Sprite.h"
#include "GameObjectSystem.h"
#include "RenderSystem.h"
#include "LevelSystem.h"
#include "MaterialSystem.h"

uint32 Sprite::NextSpriteID = 0;

Sprite::Sprite()
{
}

Sprite::Sprite(GameObjectID gameObjectId, VkGuid& spriteVramId)
{
	SpriteID = ++NextSpriteID;
    GameObjectId = gameObjectId;
    SpriteVramId = spriteVramId;
	CurrentAnimationID = kStanding;
}

Sprite::~Sprite()
{
}

SpriteInstanceStruct Sprite::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
    const Transform2DComponent& transform2D = gameObjectSystem.FindTransform2DComponent(GameObjectId);
    const SpriteVram& vram = levelSystem.VramSpriteMap.at(SpriteVramId);
    const Animation2D& animation = levelSystem.AnimationMap.at(CurrentAnimationID);
    const AnimationFrames& frameList = levelSystem.AnimationFrameListMap[vram.VramSpriteID][CurrentAnimationID];
    const Material& material = materialSystem.FindMaterial(vram.SpriteMaterialID);
    const ivec2& currentFrame = frameList[CurrentFrame];

    mat4 spriteMatrix = mat4(1.0f);
    if (LastSpritePosition != SpritePosition)
    {
        spriteMatrix = glm::translate(spriteMatrix, vec3(transform2D.GameObjectPosition.x, transform2D.GameObjectPosition.y, 0.0f));
        LastSpritePosition == SpritePosition;
    }
    if (LastSpriteRotation != SpriteRotation)
    {
        spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2D.GameObjectRotation.x), vec3(1.0f, 0.0f, 0.0f));
        spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2D.GameObjectRotation.y), vec3(0.0f, 1.0f, 0.0f));
        spriteMatrix = glm::rotate(spriteMatrix, glm::radians(0.0f), vec3(0.0f, 0.0f, 1.0f));
        LastSpriteRotation == SpriteRotation;
    }
    if (LastSpriteScale != SpriteScale)
    {
        spriteMatrix = glm::scale(spriteMatrix, vec3(transform2D.GameObjectScale.x, transform2D.GameObjectScale.y, 1.0f));
        LastSpriteScale == SpriteScale;
    }

    CurrentFrameTime += deltaTime;
    if (CurrentFrameTime >= animation.FrameHoldTime) {
        CurrentFrame += 1;
        CurrentFrameTime = 0.0f;
        if (CurrentFrame >= frameList.size()) 
        {
            CurrentFrame = 0;
        }
    }

    SpriteInstanceStruct spriteInstance;
    spriteInstance.SpritePosition = transform2D.GameObjectPosition;
    spriteInstance.SpriteSize = vram.SpriteSize;
    spriteInstance.MaterialID = material.ShaderMaterialBufferIndex;
    spriteInstance.InstanceTransform = spriteMatrix;
    spriteInstance.FlipSprite = FlipSprite;
    spriteInstance.UVOffset = vec4(vram.SpriteUVSize.x * currentFrame.x, vram.SpriteUVSize.y * currentFrame.y, vram.SpriteUVSize.x, vram.SpriteUVSize.y);

    return spriteInstance;
}

void Sprite::SetSpriteAnimation(SpriteAnimationEnum spriteAnimation)
{
    if (CurrentAnimationID == spriteAnimation)
    {
        return;
    }

    CurrentAnimationID = spriteAnimation;
    CurrentFrame = 0;
    CurrentFrameTime = 0.0f;
}
