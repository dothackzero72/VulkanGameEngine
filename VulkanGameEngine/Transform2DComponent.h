#pragma once
#include "GameObjectComponent.h"
#include "memory"

extern "C"
{
    typedef vec2* (*DLL_Transform2DComponent_GetPositionPtr)(void* wrapperHandle);
    typedef vec2* (*DLL_Transform2DComponent_GetRotationPtr)(void* wrapperHandle);
    typedef vec2* (*DLL_Transform2DComponent_GetScalePtr)(void* wrapperHandle);
    typedef mat4* (*DLL_Transform2DComponent_GetTransformMatrixPtr)(void* wrapperHandle);
}

class Transform2DComponent : public GameObjectComponent
{
private:
    DLL_Transform2DComponent_GetPositionPtr GetPositionFunctionPtr;
    DLL_Transform2DComponent_GetRotationPtr GetRotationFunctionPtr;
    DLL_Transform2DComponent_GetScalePtr GetScaleFunctionPtr;
    DLL_Transform2DComponent_GetTransformMatrixPtr GetTransformMatrixFunctionPtr;

    vec2* GetPositionPtr() const;
    vec2* GetRotationPtr() const;
    vec2* GetScalePtr() const;
    mat4* GetMatrixTransformPtr() const;

public:

    std::shared_ptr<vec2> GameObjectPosition;
    std::shared_ptr<vec2> GameObjectRotation;
    std::shared_ptr<vec2> GameObjectScale;
    std::shared_ptr<mat4> GameObjectMatrixTransform;

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