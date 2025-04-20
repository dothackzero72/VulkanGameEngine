#pragma once
#include <TypeDef.h>
#include "SpriteSheet.h"

struct Animation2D
{
	uint32 CurrentFrame;
	vec2 CurrentFrameUV;
	float CurrentFrameTime;
	float FrameHoldTime;
	Vector<ivec2> FrameList;

	Animation2D();
	Animation2D(const String& animationName, Vector<ivec2>& frameList, float frameHoldTime);
};

