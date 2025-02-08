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
#include "Material.h"
#include "SpriteSheet.h"

class Sprite;
class SpriteSheet;
class GameObjectComponent;
class GameObject 
{
private:
    const String NameSpace = "VulkanGameEngineGameObjectScripts.GameObject";

    size_t ObjectComponentMemorySize = 0;

    SharedPtr<Coral::Type> CSclass;
    SharedPtr<Coral::ManagedObject> CSobject;

public:
    String Name;
    Vector<SharedPtr<GameObjectComponent>> GameObjectComponentList;

    GameObject();
    GameObject(String name);
    GameObject(String name, Vector<ComponentTypeEnum> gameObjectComponentList);
    GameObject(String name, Vector<ComponentTypeEnum> gameObjectComponentList, SpriteSheet& spriteSheet);

    virtual void Input(float deltaTime);
    virtual void Update(float deltaTime);
    virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime);
    virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties);
    virtual void Destroy();
    void AddComponent(SharedPtr<GameObjectComponent> newComponent);
    void RemoveComponent(size_t index);

    Vector<SharedPtr<GameObjectComponent>> GetGameObjectComponentList();
    SharedPtr<GameObjectComponent> GetComponentByName(const std::string& name);
    void* GetCSObjectHandle() const { return CSobject->GetHandle(); }

    std::shared_ptr<GameObjectComponent> GetComponentByComponentType(ComponentTypeEnum type);
};