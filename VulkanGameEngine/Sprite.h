#pragma once
#include "SpriteSheet.h"
#include "Animation2D.h"

class Sprite
{
private:
	uint32 CurrentAnimationID = 0;

	SharedPtr<SpriteSheet> SpriteSheet;
	List<SharedPtr<Animation2D>> AnimationList;

public:
	Sprite();
	virtual ~Sprite();

	void Update(float deltaTime);
};

