//#include "Sprite.h"
//
//SpriteInstanceStruct Sprite_UpdateSprite(Sprite& sprite,
//    const Transform2DComponent& transform2D,
//    const SpriteVram& vram,
//    const Animation2D& animation,
//    const AnimationFrames& frameList,
//    const Material& material,
//    const ivec2& currentFrame,
//    const float& deltaTime)
//{
//    mat4 spriteMatrix = mat4(1.0f);
//    const vec2 LastSpritePosition = sprite.SpritePosition;
//    const vec2 LastSpriteRotation = sprite.SpriteRotation;
//    const vec2 LastSpriteScale = sprite.SpriteScale;
//
//    if (LastSpritePosition != sprite.SpritePosition)
//    {
//        spriteMatrix = glm::translate(spriteMatrix, vec3(transform2D.GameObjectPosition.x, transform2D.GameObjectPosition.y, 0.0f));
//    }
//    if (LastSpriteRotation != sprite.SpriteRotation)
//    {
//        spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2D.GameObjectRotation.x), vec3(1.0f, 0.0f, 0.0f));
//        spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2D.GameObjectRotation.y), vec3(0.0f, 1.0f, 0.0f));
//        spriteMatrix = glm::rotate(spriteMatrix, glm::radians(0.0f), vec3(0.0f, 0.0f, 1.0f));
//    }
//    if (LastSpriteScale != sprite.SpriteScale)
//    {
//        spriteMatrix = glm::scale(spriteMatrix, vec3(transform2D.GameObjectScale.x, transform2D.GameObjectScale.y, 1.0f));
//    }
//
//    sprite.CurrentFrameTime += deltaTime;
//    if (sprite.CurrentFrameTime >= animation.FrameHoldTime) {
//        sprite.CurrentFrame += 1;
//        sprite.CurrentFrameTime = 0.0f;
//        if (sprite.CurrentFrame >= frameList.size())
//        {
//            sprite.CurrentFrame = 0;
//        }
//    }
//
//    SpriteInstanceStruct spriteInstance;
//    spriteInstance.SpritePosition = transform2D.GameObjectPosition;
//    spriteInstance.SpriteSize = vram.SpriteSize;
//    spriteInstance.MaterialID = material.ShaderMaterialBufferIndex;
//    spriteInstance.InstanceTransform = spriteMatrix;
//    spriteInstance.FlipSprite = sprite.FlipSprite;
//    spriteInstance.UVOffset = vec4(vram.SpriteUVSize.x * currentFrame.x, vram.SpriteUVSize.y * currentFrame.y, vram.SpriteUVSize.x, vram.SpriteUVSize.y);
//
//    return spriteInstance;
//}
//
//void Sprite_SetSpriteAnimation(Sprite& sprite, SpriteAnimationEnum spriteAnimation)
//{
//    if (sprite.CurrentAnimationID == spriteAnimation)
//    {
//        return;
//    }
//
//    sprite.CurrentAnimationID = spriteAnimation;
//    sprite.CurrentFrame = 0;
//    sprite.CurrentFrameTime = 0.0f;
//}
////
////DLL_EXPORT SpriteInstanceStruct* Sprite_BatchUpdateSprites(const Transform2DComponent& transform2D, const SpriteVram& vram, const Animation2D& animation, const AnimationFrames& frameList, const Material& material, const ivec2& currentFrame, const float& deltaTime, SpriteInstanceStruct* spriteList, size_t spriteCount)
////{
////	return nullptr;
////}
