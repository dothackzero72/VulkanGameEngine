#pragma once
extern "C"
{
#include <VulkanWindow.h>
#include <GLFWWindow.h>
}

#include <windows.h>
#include <vulkan/vulkan_core.h>
#include "Typedef.h"
#include "SceneDataBuffer.h"
#include "GameObject.h"
#include <Coral/HostInstance.hpp>
#include <Coral/GC.hpp>
#include <Coral/Array.hpp>
#include <Coral/Attribute.hpp>
#include <Keyboard.h>

class GameObject;
class GameObjectComponent
{
private:
protected:
public:
    uint32 ParentGameObjectID;
    ComponentTypeEnum ComponentType;
    String Name;
    size_t MemorySize = 0;

    GameObjectComponent();
    GameObjectComponent(uint32 gameObjectId, ComponentTypeEnum componentType);
    GameObjectComponent(uint32 gameObjectId, const String& name, ComponentTypeEnum componentType);
    virtual ~GameObjectComponent();

    virtual void Input(const float& deltaTime);
    virtual void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
    virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, Vector<VkDescriptorSet>& descriptorSetList);
    virtual void Destroy();
    virtual SharedPtr<GameObjectComponent> Clone() const;
    virtual size_t GetMemorySize() const;
};