#include "Animation2D.h"

Animation2D::Animation2D()
{
}

Animation2D::Animation2D(const String& animationName, Vector<ivec2>& frameList, float frameHoldTime)
{
	FrameList = frameList;
	FrameHoldTime = frameHoldTime;
	CurrentFrameUV = vec2(0.0f);
	CurrentFrameTime = 0.0f;
	CurrentFrame = 0;
}
