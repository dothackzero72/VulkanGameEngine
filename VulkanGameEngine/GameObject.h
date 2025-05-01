#pragma once
extern "C"
{
    #include <Keyboard.h>
}
#include <memory>
#include "GameObjectComponent.h"
#include "Material.h"
#include "ECGid.h"

struct GameObject
{
    GameObjectID GameObjectId = 0;

    GameObject()
    {

    }

    GameObject(GameObjectID gameObjectId)
    {
        GameObjectId = gameObjectId;
    }
};
