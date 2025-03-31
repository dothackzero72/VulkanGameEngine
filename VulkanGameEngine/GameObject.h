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
    static uint32 NextGameObjectId;
    uint32 GameObjectId = 0;
    size_t ObjectComponentMemorySize = 0;

    SharedPtr<Coral::Type> CSclass;
    SharedPtr<Coral::ManagedObject> CSobject;

public:

    String Name;
    Vector<SharedPtr<GameObjectComponent>> GameObjectComponentList;
    bool GameObjectAlive = true;

    GameObject();
    GameObject(const String& name);
    GameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentList);
    GameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentList, SpriteSheet& spriteSheet);
    GameObject(const String& name, const Vector<GameObjectComponent>& gameObjectComponentList, SpriteSheet& spriteSheet);

    virtual void Input(const float& deltaTime);
    virtual void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
    virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, Vector<VkDescriptorSet>& descriptorSetList);
    virtual void Destroy();
    void AddComponent(SharedPtr<GameObjectComponent> newComponent);
    void RemoveComponent(size_t index);

    Vector<SharedPtr<GameObjectComponent>> GetGameObjectComponentList();
    SharedPtr<GameObjectComponent> GetComponentByName(const std::string& name);
    void* GetCSObjectHandle() const { return CSobject->GetHandle(); }

    std::shared_ptr<GameObjectComponent> GetComponentByComponentType(ComponentTypeEnum type);
    const uint32 GetId() { return GameObjectId; }
};