#pragma once
#include "GameObjectComponentDLL.h"

public ref class TestScriptComponentDLL : public GameObjectComponentDLL
{
    private:
        TestScriptComponent_CS^ component;

    public:
        TestScriptComponentDLL();
        TestScriptComponentDLL(void* gameObjectPtr);
        TestScriptComponentDLL(void* gameObjectPtr, String^ name);
        void Update(long startTime) override;
        void BufferUpdate(VkCommandBuffer commandBuffer, long startTime) override;
        void Destroy() override;
        int GetMemorySize() override;
};