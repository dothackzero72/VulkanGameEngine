#pragma once
#include "GameObjectComponent.h"
#include "Typedef.h"

class TestComponent : public GameObjectComponent
{
    public:
        ivec4 a;
        ivec3 b;
        ivec2 c;
        ivec1 d;

        TestComponent() : GameObjectComponent() 
        {
            MemorySize = GetMemorySize();
        }

        TestComponent(String name) : GameObjectComponent(name)
        {
            MemorySize = GetMemorySize();
        }

        virtual size_t GetMemorySize() const override
        {
            return sizeof(*this);
        }
};
