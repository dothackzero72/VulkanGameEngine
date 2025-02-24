#include "Transform2DComponent.h"
#include "MemoryManager.h"

Transform2DComponent::Transform2DComponent() : GameObjectComponent()
{
}

Transform2DComponent::Transform2DComponent(uint32 gameObjectId) : GameObjectComponent(gameObjectId, kTransform2DComponent)
{
    //GameObjectPosition = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetPositionPtr"));
    //GameObjectRotation = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetRotationPtr"));
    //GameObjectScale = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetScalePtr"));
    //GameObjectMatrixTransform = SharedPtr<mat4>(CSobject->InvokeMethod<mat4*>("GetTransformMatrixPtr"));
}

Transform2DComponent::Transform2DComponent(uint32 gameObjectId, const String& name) : GameObjectComponent(gameObjectId, name, kTransform2DComponent)
{
    //GameObjectPosition = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetPositionPtr"));
    //GameObjectRotation = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetRotationPtr"));
    //GameObjectScale = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetScalePtr"));
    //GameObjectMatrixTransform = SharedPtr<mat4>(CSobject->InvokeMethod<mat4*>("GetTransformMatrixPtr"));
}

Transform2DComponent::~Transform2DComponent()
{
}

void Transform2DComponent::Input(const float& deltaTime)
{
    GameObjectComponent::Input(deltaTime);
}

void Transform2DComponent::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
    GameObjectComponent::Update(commandBuffer, deltaTime);
}

void Transform2DComponent::Destroy()
{
    GameObjectComponent::Destroy();
}

SharedPtr<GameObjectComponent> Transform2DComponent::Clone() const
{
    return std::make_shared<Transform2DComponent>(*this);
}

size_t Transform2DComponent::GetMemorySize() const
{
    return sizeof(Transform2DComponent);
}
