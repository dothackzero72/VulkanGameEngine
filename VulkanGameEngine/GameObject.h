#pragma once
extern "C"
{
    #include <Keyboard.h>
}
#include <memory>
#include "GameObjectComponent.h"
#include "Material.h"

struct GameObject
{
    uint32 GameObjectId = 0;

    GameObject()
    {

    }

    GameObject(uint32 gameObjectId)
    {
        GameObjectId = gameObjectId;
    }
};
