#pragma once
#include "animation2D.h"

struct SpriteVram
{
	VkGuid				 VramSpriteID;
	VkGuid				 SpriteMaterialID;
	uint				 SpriteLayer = 0;
	vec4			 	 SpriteColor = vec4(0.0f, 0.0f, 0.0f, 1.0f);
	ivec2				 SpritePixelSize;
	vec2				 SpriteScale;
	ivec2				 SpriteCells;
	vec2				 SpriteUVSize;
	vec2				 SpriteSize = vec2(50.0f);
	uint				 AnimationListID;
};