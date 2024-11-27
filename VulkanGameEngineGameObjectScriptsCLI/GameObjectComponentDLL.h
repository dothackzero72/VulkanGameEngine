#pragma once
#include "DLLExport.h"
#using <../VulkanGameEngineGameObjectScripts/obj/Debug/net8.0/win-x64/ref/VulkanGameEngineGameObjectScripts.dll>
#include <vcclr.h>
#include <msclr/marshal_cppstd.h>
#include <vulkan/vulkan_core.h>

using namespace System;
using namespace VulkanGameEngineGameObjectScripts;

public ref class GameObjectComponentDLL
{
public:
    String^ Name;
    uint64_t MemorySize;
    ComponentTypeEnum ComponentType;

    GameObjectComponentDLL()
    {

    }

    GameObjectComponentDLL(ComponentTypeEnum componentType)
    {
        Name = "unnamed";
        ComponentType = componentType;
    }

    GameObjectComponentDLL(String^ name, ComponentTypeEnum componentType)
    {
        Name = name;
        ComponentType = componentType;
    }

    virtual void Update(long startTime) = 0;
    virtual void BufferUpdate(VkCommandBuffer commandBuffer, long startTime) = 0;
    virtual void Destroy() = 0;
    virtual int GetMemorySize() = 0;
};