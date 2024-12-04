#pragma once
#include "GameObjectComponentDLL.h"
#include "ComponentRegistry.h"

public ref class Transform2DComponentDLL : public GameObjectComponentDLL
{
private:
    Transform2DComponent_CS^ component;

public:
    Transform2DComponentDLL();
    Transform2DComponentDLL(void* wrapperObjectPtrKey, std::string name);

    void Update(float deltaTime) override;
    void BufferUpdate(VkCommandBuffer commandBuffer, float deltaTime) override;
    void Destroy() override;
    int GetMemorySize() override;
    glm::vec2* GetPositionPtr() { return reinterpret_cast<glm::vec2*>(component->GetPositionPtr()); }
    glm::vec2* GetRotationPtr() { return reinterpret_cast<glm::vec2*>(component->GetRotationPtr()); }
    glm::vec2* GetScalePtr() { return reinterpret_cast<glm::vec2*>(component->GetScalePtr()); }
    glm::mat4* GetTransformMatrixPtr() { return reinterpret_cast<glm::mat4*>(component->GetTransformMatrixPtr()); }
};