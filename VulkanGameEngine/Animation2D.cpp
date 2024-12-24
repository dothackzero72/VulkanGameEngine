#include "Animation2D.h"

Animation2D::Animation2D()
{
}

Animation2D::Animation2D(std::vector<FrameOffset> frameList, float frameHoldTime, uint32_t StartFrame)
{
}

Animation2D::~Animation2D()
{
}

void Animation2D::Update(float deltaTime, glm::vec2 spriteUVSize, uint32_t spritesInSpriteSheet, bool FlipSpriteX)
{
	CurrentFrameTime += deltaTime;
	while (CurrentFrameTime >= FrameHoldTime)
	{
		CurrentFrame += 1;
		if (CurrentFrame > FrameList.size() - 1)
		{
			CurrentFrame = 0;
		}
		CurrentFrameTime -= FrameHoldTime;
	}

	if (!FlipSpriteX)
	{
		CurrentFrameUV = Frame2D(spriteUVSize.x * FrameList[CurrentFrame].x, 0.0f);
	}
	else
	{
		CurrentFrameUV = Frame2D(spriteUVSize.x * ((spritesInSpriteSheet - 1) - FrameList[CurrentFrame].x), 0.0f);
	}
}
