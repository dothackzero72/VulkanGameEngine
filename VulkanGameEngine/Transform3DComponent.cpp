#include "Transform3DComponent.h"

Transform3DComponent::Transform3DComponent() : GameObjectComponent()
{
}

Transform3DComponent::Transform3DComponent(uint32 gameObjectId) : GameObjectComponent(gameObjectId, kTransform3DComponent)
{
    //GameObjectPosition = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetPositionPtr"));
    //GameObjectRotation = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetRotationPtr"));
    //GameObjectScale = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetScalePtr"));
    //GameObjectMatrixTransform = SharedPtr<mat4>(CSobject->InvokeMethod<mat4*>("GetTransformMatrixPtr"));
}

Transform3DComponent::Transform3DComponent(uint32 gameObjectId, String& name) : GameObjectComponent(gameObjectId, name, kTransform3DComponent)
{
    //GameObjectPosition = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetPositionPtr"));
    //GameObjectRotation = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetRotationPtr"));
    //GameObjectScale = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetScalePtr"));
    //GameObjectMatrixTransform = SharedPtr<mat4>(CSobject->InvokeMethod<mat4*>("GetTransformMatrixPtr"));
}

Transform3DComponent::~Transform3DComponent()
{
}

void Transform3DComponent::Input(const float& deltaTime)
{
    GameObjectComponent::Input(deltaTime);
}

void Transform3DComponent::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
    GameObjectComponent::Update(commandBuffer, deltaTime);
}

void Transform3DComponent::Destroy()
{
    GameObjectComponent::Destroy();
}

SharedPtr<GameObjectComponent> Transform3DComponent::Clone() const
{
    return std::make_shared<Transform3DComponent>(*this);
}

size_t Transform3DComponent::GetMemorySize() const
{
    return sizeof(Transform3DComponent);
}
