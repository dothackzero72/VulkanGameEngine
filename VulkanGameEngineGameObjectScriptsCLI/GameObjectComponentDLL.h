#pragma once
#include "DLLExport.h"
#include <vcclr.h>
#include <vulkan/vulkan_core.h>
#include <iostream>
#include <unordered_map>
#include <msclr/marshal_cppstd.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp> 

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
using namespace VulkanGameEngineGameObjectScripts;

public ref class GameObjectComponentDLL
{
public:
    void* WrapperObjectPtr;
    
    String^ Name;
    uint64_t MemorySize;
    ComponentTypeEnum ComponentType;

    GameObjectComponentDLL();
    GameObjectComponentDLL(void* wrapperObjectPtr, ComponentTypeEnum componentType);
    GameObjectComponentDLL(void* wrapperObjectPtr, std::string name, ComponentTypeEnum componentType);

    virtual void Update(float deltaTime) = 0;
    virtual void BufferUpdate(VkCommandBuffer commandBuffer, float deltaTime) = 0;
    virtual void Destroy() = 0;
    virtual int GetMemorySize() = 0;
};
