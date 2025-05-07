#pragma once
extern "C"
{
    #include "Keyboard.h"
}
#include <memory>
#include "Material.h"
#include "ECSid.h"

struct GameObject
{
    GameObjectID GameObjectId;

    GameObject()
    {

    }

    GameObject(GameObjectID gameObjectId)
    {
        GameObjectId = gameObjectId;
    }
};
