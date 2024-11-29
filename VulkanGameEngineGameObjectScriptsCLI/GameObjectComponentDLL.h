#pragma once
#include "DLLExport.h"
#include <vcclr.h>
#include <vulkan/vulkan_core.h>

#using <../VulkanGameEngineGameObjectScripts/obj/Debug/net8.0/win-x64/ref/VulkanGameEngineGameObjectScripts.dll>

using namespace System;
using namespace VulkanGameEngineGameObjectScripts;

public ref class GameObjectComponentDLL
{
public:
    void* ParentObjectPtr;
    String^ Name;
    uint64_t MemorySize;
    ComponentTypeEnum ComponentType;

    GameObjectComponentDLL();
    GameObjectComponentDLL(void* GameObjectPtr, ComponentTypeEnum componentType);
    GameObjectComponentDLL(void* GameObjectPtr, String^ name, ComponentTypeEnum componentType);

    virtual void Update(long startTime) = 0;
    virtual void BufferUpdate(VkCommandBuffer commandBuffer, long startTime) = 0;
    virtual void Destroy() = 0;
    virtual int GetMemorySize() = 0;
    //std::string GetName();
    //uint32_t GetComponentType();
};