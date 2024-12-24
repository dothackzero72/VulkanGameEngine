#pragma once
#include <TypeDef.h>

class Animation2D
{
private:
	uint32 CurrentFrame;
	Frame2D CurrentFrameUV;
	float CurrentFrameTime;
	float FrameHoldTime;
	std::vector<FrameOffset> FrameList;

public:
	Animation2D();
	Animation2D(std::vector<FrameOffset> frameList, float frameHoldTime, uint32_t StartFrame = 0);
	virtual ~Animation2D();

	void Update(float deltaTime, glm::vec2 spriteUVSize, uint32_t SpriesInSpriteSheet, bool FlipSpriteX);
	uint32_t GetCurrentFrame() { return CurrentFrame; }
	Frame2D GetFrame() { return CurrentFrameUV; }
};

