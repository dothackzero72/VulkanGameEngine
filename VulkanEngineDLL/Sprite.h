//#include "DLL.h"
//#include "Typedef.h"
//#include "VkGuid.h"
//#include "ECSid.h"
//#include "Material.h"
//#include "Transform2DComponent.h"
//
//struct Sprite
//{
//    uint CurrentAnimationID = 0;
//    uint CurrentFrame = 0;
//    VkGuid SpriteVramId;
//    float CurrentFrameTime = 0.0f;
//    bool SpriteAlive = true;
//    GameObjectID GameObjectId;
//    uint SpriteID = 0;
//    ivec2 FlipSprite = vec2(0);
//    vec2 SpritePosition = vec2(0.0f);
//    vec2 SpriteRotation = vec2(0.0f);
//    vec2 SpriteScale = vec2(1.0f);
//};
//
//struct SpriteInstanceStruct
//{
//    vec2  SpritePosition;
//    vec4  UVOffset;
//    vec2  SpriteSize;
//    ivec2 FlipSprite;
//    vec4  Color;
//    mat4  InstanceTransform;
//    uint  MaterialID;
//
//    SpriteInstanceStruct()
//    {
//        SpritePosition = vec2(0.0f);
//        UVOffset = vec4(0.0f);
//        SpriteSize = vec2(0.0f);
//        FlipSprite = ivec2(0);
//        Color = vec4(0.0f);
//        MaterialID = 0;
//        InstanceTransform = mat4(1.0f);
//    }
//
//    SpriteInstanceStruct(vec2 spritePosition, vec4 uv, vec2 spriteSize, ivec2 flipSprite, vec4 color, uint materialID, mat4 instanceTransform, uint spriteLayer)
//    {
//        SpritePosition = spritePosition;
//        UVOffset = uv;
//        SpriteSize = spriteSize;
//        FlipSprite = flipSprite;
//        Color = color;
//        MaterialID = materialID;
//        InstanceTransform = instanceTransform;
//    }
//};
//
//struct SpriteInstanceVertex2D
//{
//    vec2 SpritePosition;
//    vec4 UVOffset;
//    vec2 SpriteSize;
//    ivec2 FlipSprite;
//    vec4 Color;
//    mat4 InstanceTransform;
//    uint MaterialID;
//
//    SpriteInstanceVertex2D()
//    {
//        SpritePosition = vec2(0.0f);
//        UVOffset = vec4(0.0f);
//        SpriteSize = vec2(0.0f);
//        FlipSprite = ivec2(0);
//        Color = vec4(0.0f);
//        MaterialID = 0;
//        InstanceTransform = mat4(1.0f);
//    }
//
//    SpriteInstanceVertex2D(vec2 spritePosition, vec4 uv, vec2 spriteSize, ivec2 flipSprite, vec4 color, uint materialID, mat4 instanceTransform, uint spriteLayer)
//    {
//        SpritePosition = spritePosition;
//        UVOffset = uv;
//        SpriteSize = spriteSize;
//        FlipSprite = flipSprite;
//        Color = color;
//        MaterialID = materialID;
//        InstanceTransform = instanceTransform;
//    }
//};
//
//struct SpriteVram
//{
//    VkGuid VramSpriteID = VkGuid();
//    VkGuid SpriteMaterialID = VkGuid();
//    uint SpriteLayer = 0;
//    vec4 SpriteColor = vec4(0.0f, 0.0f, 0.0f, 1.0f);
//    ivec2 SpritePixelSize = ivec2();
//    vec2 SpriteScale = vec2(1.0f, 1.0f);
//    ivec2 SpriteCells = ivec2(0, 0);
//    vec2 SpriteUVSize = vec2();
//    vec2 SpriteSize = vec2(50.0f);
//    uint AnimationListID = 0;
//};
//
//
//
//static uint NextSpriteID;
//enum SpriteAnimationEnum
//{
//    kStanding,
//    kWalking
//};
//
//
//struct Animation2D
//{
//    uint  AnimationId;
//    float FrameHoldTime;
//};
//typedef Vector<vec2> AnimationFrames;
//
//DLL_EXPORT SpriteInstanceStruct Sprite_UpdateSprite(Sprite& sprite,
//                                                    const Transform2DComponent& transform2D,
//                                                    const SpriteVram& vram,
//                                                    const Animation2D& animation,
//                                                    const AnimationFrames& frameList,
//                                                    const Material& material,
//                                                    const ivec2& currentFrame,
//                                                    const float& deltaTime);
//
//DLL_EXPORT SpriteInstanceStruct* Sprite_BatchUpdateSprites(Sprite* spriteList,
//                                                           const Transform2DComponent* transform2DList,
//                                                           const SpriteVram* vramList,
//                                                           const Animation2D* animationList,
//                                                           const AnimationFrames* frameList,
//                                                           const Material* materialList,
//                                                           const ivec2* currentFrameList, 
//                                                           const float& deltaTime, 
//                                                           SpriteInstanceStruct* spriteInstanceList,
//                                                           size_t spriteCount);
//
//DLL_EXPORT void Sprite_SetSpriteAnimation(Sprite& sprite, SpriteAnimationEnum spriteAnimation);