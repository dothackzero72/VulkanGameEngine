#pragma once
extern "C"
{
    #include <Keyboard.h>
}
#include <memory>
#include "GameObjectComponent.h"
#include <Coral/HostInstance.hpp>
#include <Coral/GC.hpp>
#include <Coral/Array.hpp>
#include <Coral/Attribute.hpp>

class GameObjectComponent;
class GameObject 
{
private:
    const String NameSpace = "VulkanGameEngineGameObjectScripts.GameObject";

    size_t ObjectComponentMemorySize = 0;

    SharedPtr<Coral::Type> CSclass;
    SharedPtr<Coral::ManagedObject> CSobject;

    GameObject(String name);
    GameObject(String name, List<SharedPtr<GameObjectComponent>> gameObjectComponentList);

public:
    String Name;
    List<SharedPtr<GameObjectComponent>> GameObjectComponentList;

    GameObject();
    static SharedPtr<GameObject> CreateGameObject(String name);
    static SharedPtr<GameObject> CreateGameObject(String name, List<ComponentTypeEnum> gameObjectComponentList);

    virtual void Input(float deltaTime);
    virtual void Update(float deltaTime);
    virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime);
    virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
    virtual void Destroy();
    void AddComponent(SharedPtr<GameObjectComponent> newComponent);
    void RemoveComponent(size_t index);

    List<SharedPtr<GameObjectComponent>> GetGameObjectComponentList();
    SharedPtr<GameObjectComponent> GetComponentByName(const std::string& name);
    SharedPtr<GameObjectComponent> GetComponentByComponentType(ComponentTypeEnum type);
    void* GetCSObjectHandle() const { return CSobject->GetHandle(); }
};