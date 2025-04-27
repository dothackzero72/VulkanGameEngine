#include "GameObjectComponent.h"

GameObjectComponent::GameObjectComponent()
{

}

GameObjectComponent::GameObjectComponent(uint32 gameObjectId, ComponentTypeEnum componentType)
{
    Name = "default";
    ParentGameObjectID = gameObjectId;
    ComponentType = componentType;
}

GameObjectComponent::GameObjectComponent(uint32 gameObjectId, const String& name, ComponentTypeEnum componentType)
{
    Name = name;
    ParentGameObjectID = gameObjectId;
    ComponentType = componentType;

}

GameObjectComponent::~GameObjectComponent()
{
}

void GameObjectComponent::Input(const float& deltaTime)
{
    //CSobject->InvokeMethod("Input", KeyboardKeyCode::KEY_D, deltaTime);
}

void GameObjectComponent::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
    //CSobject->InvokeMethod("BufferUpdate", commandBuffer, deltaTime);
}

void GameObjectComponent::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& pipelineLayout, Vector<VkDescriptorSet>& descriptorSetList)
{

}

void GameObjectComponent::Destroy()
{
    //CSobject->InvokeMethod("Destroy");
}

SharedPtr<GameObjectComponent> GameObjectComponent::Clone() const
{
    return SharedPtr<GameObjectComponent>();
}

size_t GameObjectComponent::GetMemorySize() const
{
    return sizeof(GameObjectComponent);
}
