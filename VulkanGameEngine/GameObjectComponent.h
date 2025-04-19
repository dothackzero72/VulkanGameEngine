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
    SharedPtr<GameObject> ParentGameObject;
    ComponentTypeEnum ComponentType;
    SharedPtr<Coral::String> Name = nullptr;
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

    SharedPtr<GameObject> GetParentGameObject() { return ParentGameObject; }
    void* GetCSObjectHandle() const { return CSobject->GetHandle(); }
    const GameObjectComponent* GetCPPObjectHandle() { return this; }
};