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
    const String CSNameSpace = "VulkanGameEngineGameObjectScripts.Component.";

protected:
    SharedPtr<Coral::Type> CSclass = nullptr;
    SharedPtr<Coral::ManagedObject> CSobject = nullptr;

    std::string GetCSNameSpacePath(ComponentTypeEnum componentType);

public:
    WeakPtr<GameObject> ParentGameObjectPtr;
    SharedPtr<ComponentTypeEnum> ComponentType = nullptr;
    SharedPtr<Coral::String> Name = nullptr;
    size_t MemorySize = 0;

    GameObjectComponent();
    GameObjectComponent(void* ptr, SharedPtr<GameObject> parentGameObjectPtr, ComponentTypeEnum componentType);
    GameObjectComponent(void* ptr, SharedPtr<GameObject> parentGameObjectPtr, String name, ComponentTypeEnum componentType);
    virtual ~GameObjectComponent();

    virtual void Input(float deltaTime);
    virtual void Update(float deltaTime);
    virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime);
    virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
    virtual void Destroy();
    virtual SharedPtr<GameObjectComponent> Clone() const;
    virtual size_t GetMemorySize() const;

    WeakPtr<GameObject> GetParentGameObject() { return ParentGameObjectPtr; }
    void* GetCSObjectHandle() const { return CSobject->GetHandle(); }
    const GameObjectComponent* GetCPPObjectHandle() { return this; }
};