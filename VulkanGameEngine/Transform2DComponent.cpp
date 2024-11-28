#include "Transform2DComponent.h"

Transform2DComponent::Transform2DComponent() : GameObjectComponent()
{
}

Transform2DComponent::Transform2DComponent(std::shared_ptr<GameObject> parentGameObjectPtr) : GameObjectComponent(parentGameObjectPtr, kTransform2DComponent)
{
}

Transform2DComponent::Transform2DComponent(std::shared_ptr<GameObject> parentGameObjectPtr, String& name) : GameObjectComponent(parentGameObjectPtr, name, "Transform2DComponent", kTransform2DComponent)
{

}

Transform2DComponent::~Transform2DComponent()
{
}

void Transform2DComponent::Update(float deltaTime)
{
    GameObjectComponent::Update(deltaTime);
}

void Transform2DComponent::BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
{
    GameObjectComponent::BufferUpdate(commandBuffer, deltaTime);
}

void Transform2DComponent::Destroy()
{
    GameObjectComponent::Destroy();
}

std::shared_ptr<GameObjectComponent> Transform2DComponent::Clone() const
{
    return std::make_shared<Transform2DComponent>(*this);
}

size_t Transform2DComponent::GetMemorySize() const
{
    return DLLGetMemorySizePtr(ComponentPtr);
}