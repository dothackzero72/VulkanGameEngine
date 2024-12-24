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
    SharedPtr<vec2> GameObjectPosition;
    SharedPtr<vec2> GameObjectRotation;
    SharedPtr<vec2> GameObjectScale;
    SharedPtr<mat4> GameObjectMatrixTransform;
};


class InputComponent : public GameObjectComponent
{
private:
    WeakPtr<Transform2DComponent> transform2DComponentRef;

public:

    InputComponent();
    InputComponent(SharedPtr<GameObject> parentGameObjectPtr);
    InputComponent(SharedPtr<GameObject> parentGameObjectPtr, String& name);
    virtual ~InputComponent();

    virtual void Input(float deltaTime) override;
    virtual void Update(float deltaTime) override;
    virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime) override;
    virtual void Destroy() override;
    virtual SharedPtr<GameObjectComponent> Clone() const;
    virtual size_t GetMemorySize() const override;
};

