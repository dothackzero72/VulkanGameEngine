#pragma once
extern "C"
{
    #include <Keyboard.h>
}
#include <memory>
#include "GameObjectComponent.h"
#include "Material.h"
#include "SpriteSheet.h"

class Sprite;
class SpriteSheet;
class GameObjectComponent;
class GameObject 
{
private:
    static uint32 NextGameObjectId;
    size_t ObjectComponentMemorySize = 0;

public:
    uint32 GameObjectId = 0;
    String Name;
    Vector<SharedPtr<GameObjectComponent>> GameObjectComponentList;
    bool GameObjectAlive = true;

    GameObject();
    GameObject(const String& name);
    GameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentList);
    GameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentList, uint32 spriteSheetId);
    GameObject(const String& name, const Vector<GameObjectComponent>& gameObjectComponentList, uint32 spriteSheetId);

    virtual void Input(const float& deltaTime);
    virtual void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
    virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, Vector<VkDescriptorSet>& descriptorSetList);
    virtual void Destroy();
    void AddComponent(SharedPtr<GameObjectComponent> newComponent);
    void RemoveComponent(size_t index);

    Vector<SharedPtr<GameObjectComponent>> GetGameObjectComponentList();
    SharedPtr<GameObjectComponent> GetComponentByName(const std::string& name);

    std::shared_ptr<GameObjectComponent> GetComponentByComponentType(ComponentTypeEnum type);
    const uint32 GetId() { return GameObjectId; }
};