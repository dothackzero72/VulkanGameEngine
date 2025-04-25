#pragma once
#include "GameObjectComponent.h"
#include "memory"

class Transform2DComponent
{
public:
    vec2 GameObjectPosition = vec2(0.0f);
    vec2 GameObjectRotation = vec2(0.0f);
    vec2 GameObjectScale = vec2(1.0f);
    mat4 GameObjectMatrixTransform = mat4(0.0f);

    Transform2DComponent();
    Transform2DComponent(uint32 gameObjectId, const vec2& position);
    Transform2DComponent(uint32 gameObjectId, const vec2& position, const String& name);
    ~Transform2DComponent();

    void Input(const float& deltaTime);
    void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
    void Destroy();
};