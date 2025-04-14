#pragma once
#include "TypeDef.h"
#include "SpriteSheet.h"
#include "Animation2D.h"
#include "SceneDataBuffer.h"
#include "Vertex.h"
#include "GameObject.h"
#include "Transform2DComponent.h"
#include "AssetManager.h"

class AssetManager;
class Sprite
{
	friend class SpriteSheet;
private:
	uint32 CurrentAnimationID = 0;

	vec2 SpriteSize = vec2(50.0f);
	vec4 SpriteColor = vec4(0.0f, 0.0f, 0.0f, 1.0f);

	WeakPtr<GameObject> ParentGameObject;
	WeakPtr<Transform2DComponent> Transform2D;

	SpriteSheet Spritesheet;
	uint SpriteMaterialID;
	SharedPtr<SpriteInstanceStruct> SpriteInstance;
	Animation2D CurrentSpriteAnimation;

	Vector<Animation2D> AnimationList;
	uint CurrentFrame = 0;

	bool SpriteAlive = true;

public:

	vec2 SpritePosition = vec2(0.0f);
	uint SpriteLayer = 0;
	vec2 SpriteRotation = vec2(0.0f);
	vec2 SpriteScale = vec2(1.0f);

	Sprite();
	Sprite(uint32 id, SpriteSheet& spriteSheet);
	virtual ~Sprite();

	virtual void Input(const float& deltaTime);
	virtual void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
	virtual void Destroy();

	SharedPtr<SpriteInstanceStruct> GetSpriteInstance() { return SpriteInstance; }
};

