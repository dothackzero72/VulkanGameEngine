#include "Sprite.h"
#include "AssetManager.h"
#include "RenderSystem.h"

uint32 Sprite::NextSpriteID = 0;

Sprite::Sprite()
{
}

Sprite::Sprite(GameObjectID gameObjectId, VkGuid& spriteVramId)
{
	SpriteID = ++NextSpriteID;
    GameObjectId = gameObjectId;
    SpriteVramId = spriteVramId;
	CurrentAnimationID = kWalking;
}

Sprite::~Sprite()
{
}

SpriteInstanceStruct Sprite::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
    const Transform2DComponent& transform2D = assetManager.TransformComponentList.at(GameObjectId);
    const SpriteVram& vram = renderSystem.VramSpriteList.at(SpriteVramId);
    const Animation2D& animation = assetManager.AnimationList.at(vram.AnimationListID);
    const Vector<ivec2>& frameList = assetManager.AnimationFrameList.at(animation.AnimationFrameId);
    const Material& material = renderSystem.MaterialList.at(vram.SpriteMaterialID);

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
    SpriteInstanceStruct spriteInstance;
    spriteInstance.SpritePosition = transform2D.GameObjectPosition;
    spriteInstance.SpriteSize = vram.SpriteSize;
    spriteInstance.MaterialID = material.MaterialBufferIndex;
    spriteInstance.InstanceTransform = spriteMatrix;

    if (CurrentFrame < frameList.size()) 
    {
        const ivec2& currentFrame = frameList[CurrentFrame];
        vec2 spriteUVSize = vram.SpriteUVSize;
        spriteInstance.UVOffset = vec4(spriteUVSize.x * currentFrame.x, spriteUVSize.y * currentFrame.y, spriteUVSize.x, spriteUVSize.y);
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

    return spriteInstance;
}
