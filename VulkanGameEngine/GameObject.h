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
    bool GameObjectAlive = true;

    GameObject()
    {

    }

    GameObject(uint32 gameObjectId, bool gameObjectAlive = true)
    {
        GameObjectId = gameObjectId;
        gameObjectAlive = GameObjectAlive;
    }
};
