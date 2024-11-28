#pragma once
#include <memory>
#include "GameObjectComponent.h"

class GameObjectComponent;
class GameObject 
{
private:
    size_t ObjectComponentMemorySize = 0;

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
};