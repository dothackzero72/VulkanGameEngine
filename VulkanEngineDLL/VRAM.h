#pragma once
#include "DLL.h"
#include "VkGuid.h"
#include "Typedef.h"
#include "Material.h"

struct SpriteVram
{
    VkGuid VramSpriteID = VkGuid();
    VkGuid SpriteMaterialID = VkGuid();
    uint SpriteLayer = 0;
    vec4 SpriteColor = vec4(0.0f, 0.0f, 0.0f, 1.0f);
    ivec2 SpritePixelSize = ivec2();
    vec2 SpriteScale = vec2(1.0f, 1.0f);
    ivec2 SpriteCells = ivec2(0, 0);
    vec2 SpriteUVSize = vec2();
    vec2 SpriteSize = vec2(50.0f);
    uint AnimationListID = 0;
};

struct Animation2D
{
	uint  AnimationId;
	float FrameHoldTime;
};
typedef Vector<vec2> AnimationFrames;

#ifdef __cplusplus
extern "C" {
#endif
    DLL_EXPORT SpriteVram VRAM_LoadSpriteVRAM(const char* spritePath, const Material& material, const Texture& texture);
    DLL_EXPORT void VRAM_LoadSpriteAnimation(const char* spritePath, Animation2D* animationListPtr, vec2* animationFrameListPtr, size_t& animationListCount, size_t& animationFrameCount);
    DLL_EXPORT void VRAM_DeleteSpriteAnimation(Animation2D* animationListPtr, vec2* animationFrameListPtr);
#ifdef __cplusplus
}
#endif