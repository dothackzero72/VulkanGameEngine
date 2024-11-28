#include "GameObjectComponent.h"
#include "MemoryManager.h"

GameObjectComponent::GameObjectComponent()
{

}

GameObjectComponent::GameObjectComponent(ComponentTypeEnum componentType)
{
    Name = "component";
    ComponentType = componentType;
}

GameObjectComponent::GameObjectComponent(String name, ComponentTypeEnum componentType)
{
    Name = name;
    ComponentType = componentType;
}

GameObjectComponent::GameObjectComponent(String name, String componentName, ComponentTypeEnum componentType)
{
    Name = name;
    ComponentType = componentType;
    hModuleRef = MemoryManager::GetEntityComponentSytemModule();
    StartUp(componentName);
}

GameObjectComponent::~GameObjectComponent()
{
}

void GameObjectComponent::StartUp(String& componentName)
{
    CreateComponentPtr = (DLL_CreateComponent)GetProcAddress(hModuleRef, ("DLL_Create" + componentName).c_str());
    if (!CreateComponentPtr)
    {
        throw std::runtime_error("Could not find DLL_CreateTestScriptComponent function.");
    }

    DLLUpdatePtr = (DLL_ComponentUpdate)GetProcAddress(hModuleRef, ("DLL_" + componentName + "_Update").c_str());
    if (!DLLUpdatePtr)
    {
        throw std::runtime_error("Could not find DLL_TestScriptComponent_Update function.");
    }

    DLLBufferUpdatePtr = (DLL_ComponentBufferUpdate)GetProcAddress(hModuleRef, ("DLL_" + componentName + "_BufferUpdate").c_str());
    if (!DLLBufferUpdatePtr)
    {
        throw std::runtime_error("Could not find DLL_TestScriptComponent_BufferUpdate function.");
    }

    DLLDestroyPtr = (DLL_ComponentDestroy)GetProcAddress(hModuleRef, ("DLL_" + componentName + "_Destroy").c_str());
    if (!DLLDestroyPtr)
    {
        throw std::runtime_error("Could not find DLL_TestScriptComponent_Destroy function.");
    }

    DLLGetMemorySizePtr = (DLL_ComponentGetMemorySize)GetProcAddress(hModuleRef, ("DLL_" + componentName + "_GetMemorySize").c_str());
    if (!DLLGetMemorySizePtr)
    {
        throw std::runtime_error("Could not find DLL_TestScriptComponent_GetMemorySize function.");
    }

    wrapperHandle = CreateComponentPtr(componentPtr);
    if (!wrapperHandle)
    {
        throw std::runtime_error("Component creation failed.");
    }
}

 void GameObjectComponent::Update(float deltaTime)
{
    DLLUpdatePtr(componentPtr, deltaTime);
}

 void GameObjectComponent::BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
{
   // DLLBufferUpdatePtr(componentPtr, commandBuffer, deltaTime);
}

 void GameObjectComponent::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{

}

 void GameObjectComponent::Destroy()
 {

 }
