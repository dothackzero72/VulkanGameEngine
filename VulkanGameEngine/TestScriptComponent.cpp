#include "TestScriptComponent.h"

TestScriptComponent::TestScriptComponent()
{
}

TestScriptComponent::TestScriptComponent(std::shared_ptr<GameObject> parentGameObjectPtr) : GameObjectComponent(parentGameObjectPtr, kTestScriptComponent)
{
}

TestScriptComponent::TestScriptComponent(std::shared_ptr<GameObject> parentGameObjectPtr, String& name) : GameObjectComponent(parentGameObjectPtr, name, "TestScriptComponent", kTestScriptComponent)
{

}

TestScriptComponent::~TestScriptComponent()
{
}

void TestScriptComponent::Update(float deltaTime)
{
    GameObjectComponent::Update(deltaTime);
}

void TestScriptComponent::BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
{
    GameObjectComponent::BufferUpdate(commandBuffer, deltaTime);
}

void TestScriptComponent::Destroy()
{
    GameObjectComponent::Destroy();
}

std::shared_ptr<GameObjectComponent> TestScriptComponent::Clone() const 
{
    return std::make_shared<TestScriptComponent>(*this);
}

size_t TestScriptComponent::GetMemorySize() const
{
    return DLLGetMemorySizePtr(ComponentPtr);
}