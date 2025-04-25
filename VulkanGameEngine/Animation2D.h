#pragma once
#include <TypeDef.h>

struct Animation2D
{
	String AnimationName;
	uint32 CurrentFrame;
	vec2 CurrentFrameUV;
	float CurrentFrameTime;
	float FrameHoldTime;
	Vector<ivec2> FrameList;

	Animation2D();
	Animation2D(const String& animationName, Vector<ivec2>& frameList, float frameHoldTime);
};

