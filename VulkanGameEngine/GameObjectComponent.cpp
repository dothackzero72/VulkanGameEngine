#include "GameObjectComponent.h"
#include "MemoryManager.h"

GameObjectComponent::GameObjectComponent()
{

}

GameObjectComponent::GameObjectComponent(std::shared_ptr<GameObject> parentGameObjectPtr, String className, ComponentTypeEnum componentType)
{
    //Name = "component";
    ComponentType = std::make_shared<ComponentTypeEnum>(componentType);
    ParentGameObjectPtr = parentGameObjectPtr;

    CSclass = std::make_shared<Coral::Type>(MemoryManager::GetECSassemblyModule()->GetType(CSNameSpace + className));
    CSobject = std::make_shared<Coral::ManagedObject>(CSclass->CreateInstance(ParentGameObjectPtr->GetCSObjectHandle()));
}

GameObjectComponent::GameObjectComponent(std::shared_ptr<GameObject> parentGameObjectPtr, String name, String className, ComponentTypeEnum componentType)
{
    //Name = name;
    ComponentType = std::make_shared<ComponentTypeEnum>(componentType);
    ParentGameObjectPtr = parentGameObjectPtr;

    CSclass = std::make_shared<Coral::Type>(MemoryManager::GetECSassemblyModule()->GetType(CSNameSpace + className));
    CSobject = std::make_shared<Coral::ManagedObject>(CSclass->CreateInstance(ParentGameObjectPtr->GetCSObjectHandle()));
}

GameObjectComponent::GameObjectComponent(std::shared_ptr<GameObject> parentGameObjectPtr, String name, String componentName, String className, ComponentTypeEnum componentType)
{
    //Name = name;
    ComponentType = std::make_shared<ComponentTypeEnum>(componentType);
    ParentGameObjectPtr = parentGameObjectPtr;

    CSclass = std::make_shared<Coral::Type>(MemoryManager::GetECSassemblyModule()->GetType(CSNameSpace + className));
    CSobject = std::make_shared<Coral::ManagedObject>(CSclass->CreateInstance(ParentGameObjectPtr->GetCSObjectHandle()));
}

GameObjectComponent::~GameObjectComponent()
{
}

void GameObjectComponent::Input(InputKey key, KeyState keyState)
{
    CSobject->InvokeMethod("Input", key, keyState);
}

void GameObjectComponent::Update(float deltaTime)
{
    CSobject->InvokeMethod("Update", deltaTime);
}

void GameObjectComponent::BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
{
    CSobject->InvokeMethod("BufferUpdate", commandBuffer, deltaTime);
}

void GameObjectComponent::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{
    CSobject->InvokeMethod("Destroy");
}

void GameObjectComponent::Destroy()
{
}

std::shared_ptr<GameObjectComponent> GameObjectComponent::Clone() const
{
    return std::shared_ptr<GameObjectComponent>();
}

size_t GameObjectComponent::GetMemorySize() const
{
    return sizeof(GameObjectComponent);
}
