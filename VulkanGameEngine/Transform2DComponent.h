#pragma once
#include "GameObjectComponent.h"
#include "memory"

class Transform2DComponent : public GameObjectComponent
{
public:

    SharedPtr<vec2> GameObjectPosition;
    SharedPtr<vec2> GameObjectRotation;
    SharedPtr<vec2> GameObjectScale;
    SharedPtr<mat4> GameObjectMatrixTransform;

    Transform2DComponent();
    Transform2DComponent(SharedPtr<GameObject> parentGameObjectPtr);
    Transform2DComponent(SharedPtr<GameObject> parentGameObjectPtr, String& name);
    virtual ~Transform2DComponent();

    virtual void Input(float deltaTime) override;
    virtual void Update(float deltaTime) override;
    virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime) override;
    virtual void Destroy() override;
    virtual SharedPtr<GameObjectComponent> Clone() const;
    virtual size_t GetMemorySize() const override;
};