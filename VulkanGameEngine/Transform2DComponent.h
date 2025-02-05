#pragma once
#include "GameObjectComponent.h"
#include "memory"

class Transform2DComponent : public GameObjectComponent
{
public:

    vec2 GameObjectPosition = vec2(0.0f);
    vec2 GameObjectRotation = vec2(0.0f);
    vec2 GameObjectScale = vec2(1.0f);
    mat4 GameObjectMatrixTransform = mat4(0.0f);

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