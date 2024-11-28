#pragma once
#include "GameObjectComponent.h"
#include "memory"

class Transform2DComponent : public GameObjectComponent
{
private:

public:
    Transform2DComponent();
    Transform2DComponent(std::shared_ptr<GameObject> parentGameObjectPtr);
    Transform2DComponent(std::shared_ptr<GameObject> parentGameObjectPtr, String& name);
    virtual ~Transform2DComponent();

    virtual void Update(float deltaTime) override;
    virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime) override;
    virtual void Destroy() override;
    virtual std::shared_ptr<GameObjectComponent> Clone() const;
    virtual size_t GetMemorySize() const override;
};