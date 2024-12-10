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

class GameObject;
class GameObjectComponent
{
private:
    const String CSNameSpace = "VulkanGameEngineGameObjectScripts.";

protected:
    std::shared_ptr<Coral::Type> CSclass = nullptr;
    std::shared_ptr<Coral::ManagedObject> CSobject = nullptr;

public:
    std::shared_ptr<GameObject> ParentGameObjectPtr = nullptr;
    std::shared_ptr<ComponentTypeEnum> ComponentType = nullptr;
    std::shared_ptr<Coral::String> Name = nullptr;
    size_t MemorySize = 0;

    GameObjectComponent();
    GameObjectComponent(std::shared_ptr<GameObject> parentGameObjectPtr, String className, ComponentTypeEnum componentType);
    GameObjectComponent(std::shared_ptr<GameObject> parentGameObjectPtr, String name, String className, ComponentTypeEnum componentType);
    GameObjectComponent(std::shared_ptr<GameObject> parentGameObjectPtr, String name, String componentName, String className, ComponentTypeEnum componentType);
    virtual ~GameObjectComponent();

    virtual void Input(InputKey key, KeyState keyState);
    virtual void Update(float deltaTime);
    virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime);
    virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
    virtual void Destroy();
    virtual std::shared_ptr<GameObjectComponent> Clone() const;
    virtual size_t GetMemorySize() const;

    virtual std::string GetClassName() const 
    {
        return "GameObjectComponent";
    }

    std::shared_ptr<GameObject> GetParentGameObject() { return ParentGameObjectPtr; }
    void* GetCSObjectHandle() const { return CSobject->GetHandle(); }
};