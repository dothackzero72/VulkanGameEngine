#include "GameObjectComponent.h"
#include "memory"

class TestScriptComponent : public GameObjectComponent
{
private:

public:
    TestScriptComponent() : GameObjectComponent(kTestScriptComponent)
    {
    }

    TestScriptComponent(String& name) : GameObjectComponent(name, "TestScriptComponent", kTestScriptComponent)
    {

    }

    ~TestScriptComponent()
    {
    }

    virtual void Update(float deltaTime) override
    {
        GameObjectComponent::Update(deltaTime);
    }

    virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime) override
    {
        GameObjectComponent::BufferUpdate(commandBuffer, deltaTime);
    }

    virtual void Destroy() override
    {
        GameObjectComponent::Destroy();
    }

    virtual std::shared_ptr<GameObjectComponent> Clone() const
    {
        return std::make_shared<TestScriptComponent>(*this);
    }

    virtual size_t GetMemorySize() const override
    {
        return DLLGetMemorySizePtr(componentPtr);
    }
};