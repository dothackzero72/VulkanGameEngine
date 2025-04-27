#include "Transform2DComponent.h"

Transform2DComponent::Transform2DComponent()
{
}

Transform2DComponent::Transform2DComponent(uint32 gameObjectId, const vec2& position)
{
    GameObjectPosition = position;
    //GameObjectPosition = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetPositionPtr"));
    //GameObjectRotation = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetRotationPtr"));
    //GameObjectScale = SharedPtr<vec2>(CSobject->InvokeMethod<vec2*>("GetScalePtr"));
    //GameObjectMatrixTransform = SharedPtr<mat4>(CSobject->InvokeMethod<mat4*>("GetTransformMatrixPtr"));
}

Transform2DComponent::Transform2DComponent(uint32 gameObjectId, const vec2& position, const String& name) 
{
    GameObjectPosition = position;
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
    //GameObjectComponent::Input(deltaTime);
}

void Transform2DComponent::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
   // GameObjectComponent::Update(commandBuffer, deltaTime);
}

void Transform2DComponent::Destroy()
{
   // GameObjectComponent::Destroy();
}