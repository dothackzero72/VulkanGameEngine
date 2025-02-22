#include "Transform3DComponent.h"
#include "MemoryManager.h"

Transform3DComponent::Transform3DComponent() : GameObjectComponent()
{
}

Transform3DComponent::Transform3DComponent(SharedPtr<GameObject> parentGameObjectPtr) : GameObjectComponent(this, parentGameObjectPtr, kTransform3DComponent)
{
    //GameObjectPosition = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetPositionPtr"));
    //GameObjectRotation = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetRotationPtr"));
    //GameObjectScale = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetScalePtr"));
    //GameObjectMatrixTransform = SharedPtr<mat4>(CSobject->InvokeMethod<mat4*>("GetTransformMatrixPtr"));
}

Transform3DComponent::Transform3DComponent(SharedPtr<GameObject> parentGameObjectPtr, String& name) : GameObjectComponent(this, parentGameObjectPtr, name, kTransform3DComponent)
{
    //GameObjectPosition = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetPositionPtr"));
    //GameObjectRotation = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetRotationPtr"));
    //GameObjectScale = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetScalePtr"));
    //GameObjectMatrixTransform = SharedPtr<mat4>(CSobject->InvokeMethod<mat4*>("GetTransformMatrixPtr"));
}

Transform3DComponent::~Transform3DComponent()
{
}

void Transform3DComponent::Input(float deltaTime)
{
    GameObjectComponent::Input(deltaTime);
}

void Transform3DComponent::Update(float deltaTime)
{
    GameObjectComponent::Update(deltaTime);
}

void Transform3DComponent::BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
{
    GameObjectComponent::BufferUpdate(commandBuffer, deltaTime);
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
