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

    std::string GetCSNameSpacePath(ComponentTypeEnum componentType);

protected:
    std::shared_ptr<Coral::Type> CSclass = nullptr;
    std::shared_ptr<Coral::ManagedObject> CSobject = nullptr;

public:
    std::weak_ptr<GameObject> ParentGameObjectPtr;
    std::shared_ptr<ComponentTypeEnum> ComponentType = nullptr;
    std::shared_ptr<Coral::String> Name = nullptr;
    size_t MemorySize = 0;

    GameObjectComponent();
    GameObjectComponent(void* ptr, std::shared_ptr<GameObject> parentGameObjectPtr, ComponentTypeEnum componentType);
    GameObjectComponent(void* ptr, std::shared_ptr<GameObject> parentGameObjectPtr, String name, ComponentTypeEnum componentType);
    virtual ~GameObjectComponent();

    virtual void Input(KeyBoardKeys key, float deltaTime);
    virtual void Update(float deltaTime);
    virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime);
    virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
    virtual void Destroy();
    virtual std::shared_ptr<GameObjectComponent> Clone() const;
    virtual size_t GetMemorySize() const;

    std::weak_ptr<GameObject> GetParentGameObject() { return ParentGameObjectPtr; }
    void* GetCSObjectHandle() const { return CSobject->GetHandle(); }
};