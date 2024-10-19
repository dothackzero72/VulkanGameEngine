#pragma once
#include "GameObjectComponent.h"
#include "Typedef.h"

class TestComponent : public GameObjectComponent
{
    public:
        ivec4 a = ivec4(1, 2, 3, 4);
        ivec3 b = ivec3(5, 6, 7);
        ivec2 c = ivec2(8, 9);
        ivec1 d = ivec1(10);

        TestComponent() : GameObjectComponent() 
        {
            MemorySize = GetMemorySize();
        }

        TestComponent(String name) : GameObjectComponent(name)
        {
            MemorySize = GetMemorySize();
        }

        virtual ~TestComponent() override
        {

        }

        virtual std::shared_ptr<GameObjectComponent> Clone() override
        {
            return std::make_shared<TestComponent>(*this);
        }

        virtual size_t GetMemorySize() const override
        {
            return sizeof(*this);
        }
};
