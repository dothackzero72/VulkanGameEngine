#pragma once
#include "GameObjectComponent.h"
#include "Transform2DComponent.h"

struct CSGameObjectComponent
{
     GameObject* ParentGameObject;
     ComponentTypeEnum* ComponentType;
     Coral::String Name;
};

struct CSTransform2DComponent : public CSGameObjectComponent
{
    std::shared_ptr<vec2> GameObjectPosition;
    std::shared_ptr<vec2> GameObjectRotation;
    std::shared_ptr<vec2> GameObjectScale;
    std::shared_ptr<mat4> GameObjectMatrixTransform;
};


class InputComponent : public GameObjectComponent
{
private:
    std::weak_ptr<Transform2DComponent> transform2DComponentRef;

public:

    InputComponent();
    InputComponent(std::shared_ptr<GameObject> parentGameObjectPtr);
    InputComponent(std::shared_ptr<GameObject> parentGameObjectPtr, String& name);
    virtual ~InputComponent();

    virtual void Input(float deltaTime) override;
    virtual void Update(float deltaTime) override;
    virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime) override;
    virtual void Destroy() override;
    virtual std::shared_ptr<GameObjectComponent> Clone() const;
    virtual size_t GetMemorySize() const override;
};

