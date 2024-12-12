#include "Transform2DComponent.h"

Transform2DComponent::Transform2DComponent() : GameObjectComponent()
{
}

Transform2DComponent::Transform2DComponent(std::shared_ptr<GameObject> parentGameObjectPtr) : GameObjectComponent(this, parentGameObjectPtr, kTransform2DComponent)
{
    GameObjectPosition = std::shared_ptr<vec2>(CSobject->InvokeMethod<vec2*>("GetPositionPtr"));
    GameObjectRotation = std::shared_ptr<vec2>(CSobject->InvokeMethod<vec2*>("GetRotationPtr"));
    GameObjectScale = std::shared_ptr<vec2>(CSobject->InvokeMethod<vec2*>("GetScalePtr"));
    GameObjectMatrixTransform = std::shared_ptr<mat4>(CSobject->InvokeMethod<mat4*>("GetTransformMatrixPtr"));
}

Transform2DComponent::Transform2DComponent(std::shared_ptr<GameObject> parentGameObjectPtr, String& name) : GameObjectComponent(this, parentGameObjectPtr, name, kTransform2DComponent)
{
    GameObjectPosition = std::shared_ptr<vec2>(CSobject->InvokeMethod<vec2*>("GetPositionPtr"));
    GameObjectRotation = std::shared_ptr<vec2>(CSobject->InvokeMethod<vec2*>("GetRotationPtr"));
    GameObjectScale = std::shared_ptr<vec2>(CSobject->InvokeMethod<vec2*>("GetScalePtr"));
    GameObjectMatrixTransform = std::shared_ptr<mat4>(CSobject->InvokeMethod<mat4*>("GetTransformMatrixPtr"));
}

Transform2DComponent::~Transform2DComponent()
{
}

void Transform2DComponent::Input(KeyBoardKeys key, float deltaTime)
{
    GameObjectComponent::Input(key, deltaTime);
}

void Transform2DComponent::Update(float deltaTime)
{
    GameObjectComponent::Update(deltaTime);
}

void Transform2DComponent::BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
{
    GameObjectComponent::BufferUpdate(commandBuffer, deltaTime);
}

void Transform2DComponent::Destroy()
{
    GameObjectComponent::Destroy();
}

std::shared_ptr<GameObjectComponent> Transform2DComponent::Clone() const
{
    return std::make_shared<Transform2DComponent>(*this);
}

size_t Transform2DComponent::GetMemorySize() const
{
    return sizeof(Transform2DComponent);
}
