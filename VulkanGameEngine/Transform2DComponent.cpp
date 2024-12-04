#include "Transform2DComponent.h"

Transform2DComponent::Transform2DComponent() : GameObjectComponent()
{
}

Transform2DComponent::Transform2DComponent(std::shared_ptr<GameObject> parentGameObjectPtr) : GameObjectComponent(parentGameObjectPtr, kTransform2DComponent)
{
    GetPositionFunctionPtr = (DLL_Transform2DComponent_GetPositionPtr)GetProcAddress(hModuleRef, "DLL_Transform2DComponent_GetPositionPtr");
    if (!GetPositionFunctionPtr)
    {
        throw std::runtime_error("Could not find DLL_Transform2DComponent_GetPosition function.");
    }

    GetRotationFunctionPtr = (DLL_Transform2DComponent_GetRotationPtr)GetProcAddress(hModuleRef, "DLL_Transform2DComponent_GetRotationPtr");
    if (!GetRotationFunctionPtr)
    {
        throw std::runtime_error("Could not find DLL_Transform2DComponent_GetRotationPtr function.");
    }

    GetScaleFunctionPtr = (DLL_Transform2DComponent_GetScalePtr)GetProcAddress(hModuleRef, "DLL_Transform2DComponent_GetScalePtr");
    if (!GetRotationFunctionPtr)
    {
        throw std::runtime_error("Could not find DLL_Transform2DComponent_GetScalePtr function.");
    }

    GetTransformMatrixFunctionPtr = (DLL_Transform2DComponent_GetTransformMatrixPtr)GetProcAddress(hModuleRef, "DLL_Transform2DComponent_GetTransformMatrixPtr");
    if (!GetRotationFunctionPtr)
    {
        throw std::runtime_error("Could not find DLL_Transform2DComponent_GetTransformMatrixPtr function.");
    }

   GameObjectPosition = std::shared_ptr<vec2>(GetPositionPtr());
   GameObjectRotation = std::shared_ptr<vec2>(GetRotationPtr());
   GameObjectScale = std::shared_ptr<vec2>(GetScalePtr());
   GameObjectMatrixTransform = std::shared_ptr<mat4>(GetMatrixTransformPtr());
}

Transform2DComponent::Transform2DComponent(std::shared_ptr<GameObject> parentGameObjectPtr, String& name) : GameObjectComponent(parentGameObjectPtr, name, "Transform2DComponent", kTransform2DComponent)
{
    GetPositionFunctionPtr = (DLL_Transform2DComponent_GetPositionPtr)GetProcAddress(hModuleRef, "DLL_Transform2DComponent_GetPositionPtr");
    if (!GetPositionFunctionPtr)
    {
        throw std::runtime_error("Could not find DLL_Transform2DComponent_GetPosition function.");
    }

    GetRotationFunctionPtr = (DLL_Transform2DComponent_GetRotationPtr)GetProcAddress(hModuleRef, "DLL_Transform2DComponent_GetRotationPtr");
    if (!GetRotationFunctionPtr)
    {
        throw std::runtime_error("Could not find DLL_Transform2DComponent_GetRotationPtr function.");
    }

    GetScaleFunctionPtr = (DLL_Transform2DComponent_GetScalePtr)GetProcAddress(hModuleRef, "DLL_Transform2DComponent_GetScalePtr");
    if (!GetRotationFunctionPtr)
    {
        throw std::runtime_error("Could not find DLL_Transform2DComponent_GetScalePtr function.");
    }

    GetTransformMatrixFunctionPtr = (DLL_Transform2DComponent_GetTransformMatrixPtr)GetProcAddress(hModuleRef, "DLL_Transform2DComponent_GetTransformMatrixPtr");
    if (!GetRotationFunctionPtr)
    {
        throw std::runtime_error("Could not find DLL_Transform2DComponent_GetTransformMatrixPtr function.");
    }

    GameObjectPosition = std::shared_ptr<vec2>(GetPositionPtr());
    GameObjectRotation = std::shared_ptr<vec2>(GetRotationPtr());
    GameObjectScale = std::shared_ptr<vec2>(GetScalePtr());
    GameObjectMatrixTransform = std::shared_ptr<mat4>(GetMatrixTransformPtr());
}

Transform2DComponent::~Transform2DComponent()
{
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
    return DLLGetMemorySizePtr(ComponentPtr);
}

glm::vec2* Transform2DComponent::GetPositionPtr() const
{
    return GetPositionFunctionPtr(ComponentPtr);
}

vec2* Transform2DComponent::GetRotationPtr() const
{
    return GetRotationFunctionPtr(ComponentPtr);
}

vec2* Transform2DComponent::GetScalePtr() const
{
    return GetScaleFunctionPtr(ComponentPtr);
}

mat4* Transform2DComponent::GetMatrixTransformPtr() const
{
    return GetTransformMatrixFunctionPtr(ComponentPtr);
}