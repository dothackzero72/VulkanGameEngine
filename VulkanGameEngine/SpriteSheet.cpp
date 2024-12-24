#include "SpriteSheet.h"

SpriteSheet::SpriteSheet()
{
}

SpriteSheet::SpriteSheet(SharedPtr<Material> material, vec2 spriteSize, ivec2 tileSizeInPixels, ivec2 SpritePosition, std::vector<ivec2>& AnimationFrameOffsets, float FrameTime)
{
}

SpriteSheet::~SpriteSheet()
{
}

void SpriteSheet::Update(float deltaTime)
{
	if (DrawSprite)
	{
		CurrentFrameTime += deltaTime;
		if (CurrentFrameTime >= FrameHoldTime)
		{
			CurrentFrame += 1;
			if (CurrentFrame > AnimationFrameOffsets.size() - 1)
			{
				CurrentFrame = 0;
			}
			CurrentFrameTime = 0.0f;
		}

		CurrentSpriteUV = glm::vec2(SpriteUVSize.x * AnimationFrameOffsets[CurrentFrame].x, SpriteUVSize.y * AnimationFrameOffsets[CurrentFrame].y);
	}
}