#pragma once
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

    std::shared_ptr<Coral::Type> CSclass;
    std::shared_ptr<Coral::ManagedObject> CSobject;

    GameObject(String name);
    GameObject(String name, List<std::shared_ptr<GameObjectComponent>> gameObjectComponentList);

public:
    String Name;
    List<std::shared_ptr<GameObjectComponent>> GameObjectComponentList;

    GameObject();
    static std::shared_ptr<GameObject> CreateGameObject(String name);
    static std::shared_ptr<GameObject> CreateGameObject(String name, List<ComponentTypeEnum> gameObjectComponentList);

    virtual void Update(float deltaTime);
    virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime);
    virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
    virtual void Destroy();
    void AddComponent(std::shared_ptr<GameObjectComponent> newComponent);
    void RemoveComponent(size_t index);

    List<std::shared_ptr<GameObjectComponent>> GetGameObjectComponentList();
    std::shared_ptr<GameObjectComponent> GetComponentByName(const std::string& name);
    std::shared_ptr<GameObjectComponent> GetComponentByComponentType(ComponentTypeEnum type);
    void* GetCSObjectHandle() const { return CSobject->GetHandle(); }
};