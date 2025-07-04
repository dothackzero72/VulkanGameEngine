#pragma once
#include <memory>
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
