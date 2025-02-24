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
    Transform2DComponent(uint32 gameObjectId);
    Transform2DComponent(uint32 gameObjectId, const String& name);
    virtual ~Transform2DComponent();

    virtual void Input(const float& deltaTime) override;
    virtual void Update(VkCommandBuffer& commandBuffer, const float& deltaTime) override;
    virtual void Destroy() override;
    virtual SharedPtr<GameObjectComponent> Clone() const;
    virtual size_t GetMemorySize() const override;
};