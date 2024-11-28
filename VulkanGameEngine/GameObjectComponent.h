#pragma once
#include <windows.h>
#include <vulkan/vulkan_core.h>
#include "Typedef.h"
#include "SceneDataBuffer.h"

enum ComponentTypeEnum
{
	kUndefined,
	kRenderMesh2DComponent,
    kTestScriptComponent
};

#define GET_CLASS_NAME(classname) std::wstring(L#classname)

extern "C"
{
    typedef void* (*DLL_CreateComponent)(void* wrapperHandle);
    typedef void (*DLL_ComponentUpdate)(void* wrapperHandle, long startTime);
    typedef void (*DLL_ComponentBufferUpdate)(void* wrapperHandle, VkCommandBuffer commandBuffer, long startTime);
    typedef void (*DLL_ComponentDestroy)(void* wrapperHandle);
    typedef int (*DLL_ComponentGetMemorySize)(void* wrapperHandle);
}

class GameObjectComponent
{
private:
    HMODULE hModuleRef = nullptr;
    DLL_CreateComponent CreateComponentPtr = nullptr;
    DLL_ComponentUpdate DLLUpdatePtr = nullptr;
    DLL_ComponentBufferUpdate DLLBufferUpdatePtr = nullptr;
    DLL_ComponentDestroy DLLDestroyPtr = nullptr;

    void StartUp(String& componentName);
protected:
    void* componentPtr = nullptr;
    void* wrapperHandle = nullptr;
    DLL_ComponentGetMemorySize DLLGetMemorySizePtr = nullptr;

public:
    String Name = "Component";
    size_t MemorySize = 0;
    ComponentTypeEnum ComponentType = ComponentTypeEnum::kUndefined;

    GameObjectComponent();
    GameObjectComponent(ComponentTypeEnum componentType);
    GameObjectComponent(String name, ComponentTypeEnum componentType);
    GameObjectComponent(String name, String componentName, ComponentTypeEnum componentType);
    virtual ~GameObjectComponent();

    virtual void Update(float deltaTime);
    virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime);
    virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
    virtual void Destroy();
    virtual std::shared_ptr<GameObjectComponent> Clone() const = 0;
    virtual size_t GetMemorySize() const = 0;
};
