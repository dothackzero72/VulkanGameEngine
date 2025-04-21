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
    uint32 SpriteID = 0;
    String Name;
    Vector<SharedPtr<GameObjectComponent>> GameObjectComponentList;
    bool GameObjectAlive = true;

    GameObject();
    GameObject(const String& name);
    GameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentList);
    GameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentList, uint32 spriteID);
    GameObject(const String& name, const Vector<GameObjectComponent>& gameObjectComponentList, uint32 spriteID);

    void Input(const float& deltaTime);
    void Update(VkCommandBuffer& commandBuffer, const float& deltaTime);
    void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, Vector<VkDescriptorSet>& descriptorSetList);
    void Destroy();
    void AddComponent(SharedPtr<GameObjectComponent> newComponent);
    void RemoveComponent(size_t index);

    Vector<SharedPtr<GameObjectComponent>> GetGameObjectComponentList();
    SharedPtr<GameObjectComponent> GetComponentByName(const std::string& name);

    std::shared_ptr<GameObjectComponent> GetComponentByComponentType(ComponentTypeEnum type);
    const uint32 GetId() { return GameObjectId; }
};