#pragma once
#include "memory"

struct Transform2DComponent
{
    vec2 GameObjectPosition = vec2(0.0f);
    vec2 GameObjectRotation = vec2(0.0f);
    vec2 GameObjectScale = vec2(1.0f);

    Transform2DComponent()
    {

    }

    Transform2DComponent(vec2& gameObjectPosition)
    {
        GameObjectPosition = gameObjectPosition;
    }

    Transform2DComponent(vec2& gameObjectPosition, vec2& gameObjectRotation)
    {
        GameObjectPosition = gameObjectPosition;
        GameObjectRotation = gameObjectRotation;
    }

    Transform2DComponent(vec2& gameObjectPosition, vec2& gameObjectRotation, vec2& gameObjectScale)
    {
        GameObjectPosition = gameObjectPosition;
        GameObjectRotation = gameObjectRotation;
        GameObjectScale = GameObjectScale;
    }
};