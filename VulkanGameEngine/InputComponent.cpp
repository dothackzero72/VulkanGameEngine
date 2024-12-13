#include "InputComponent.h"
#include "MemoryManager.h"

InputComponent::InputComponent() : GameObjectComponent()
{
}

InputComponent::InputComponent(std::shared_ptr<GameObject> parentGameObjectPtr) : GameObjectComponent(this, parentGameObjectPtr, kInputComponent)
{
   // transform2DComponentRef = std::shared_ptr<Transform2DComponent>(CSobject->InvokeMethod<Transform2DComponent*>("GetCPPComponentPtr"));
}

InputComponent::InputComponent(std::shared_ptr<GameObject> parentGameObjectPtr, String& name) : GameObjectComponent(this, parentGameObjectPtr, name, kInputComponent)
{
    //auto adfd = this;
    //auto fds4 = static_cast<Transform2DComponent*>(parentGameObjectPtr->GameObjectComponentList[0].get())->GetCPPObjectHandle();
    //auto fds4s = parentGameObjectPtr->GameObjectComponentList[0].get();
    //void* asd = CSobject->InvokeMethod<void*>("GetCPPComponentPtr");
    //Transform2DComponent* fds = static_cast<Transform2DComponent*>(asd);
    //transform2DComponentRef = std::shared_ptr<Transform2DComponent>(fds);
}

InputComponent::~InputComponent()
{
}

void InputComponent::Input(float deltaTime)
{
    GameObjectComponent::Input(deltaTime);
}

void InputComponent::Update(float deltaTime)
{
    GameObjectComponent::Update(deltaTime);
}

void InputComponent::BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
{
    GameObjectComponent::BufferUpdate(commandBuffer, deltaTime);
}

void InputComponent::Destroy()
{
    GameObjectComponent::Destroy();
}

std::shared_ptr<GameObjectComponent> InputComponent::Clone() const
{
    return std::make_shared<InputComponent>(*this);
}

size_t InputComponent::GetMemorySize() const
{
    return sizeof(InputComponent);
}