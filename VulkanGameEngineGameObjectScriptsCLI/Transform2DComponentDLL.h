#pragma once
#include "GameObjectComponentDLL.h"
#include <msclr/marshal_cppstd.h>

using namespace System;
using namespace System::Runtime::InteropServices;
public ref class Transform2DComponentDLL : public GameObjectComponentDLL
{
private:
    Transform2DComponent_CS^ component;

public:
    Transform2DComponentDLL();
    Transform2DComponentDLL(void* gameObjectPtr, std::string name);

    void Update(float deltaTime) override;
    void BufferUpdate(VkCommandBuffer commandBuffer, float deltaTime) override;
    void Destroy() override;
    int GetMemorySize() override;
};