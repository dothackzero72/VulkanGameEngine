#pragma once
extern "C"
{
    #include "Keyboard.h"
}
#include <memory>
#include "Material.h"
#include "ECGid.h"

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
