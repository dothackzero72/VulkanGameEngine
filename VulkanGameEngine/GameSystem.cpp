#include "GameSystem.h"

void GameSystem::DestroyDeadGameObjects()
{
    if (assetManager.GameObjectList.empty())
    {
        return;
    }

    //Vector<SharedPtr<GameObject>> deadGameObjectList;
    //for (auto it = GameObjectList.begin(); it != GameObjectList.end(); ++it) {
    //    if (!(*it)->GameObjectAlive) {
    //        deadGameObjectList.push_back(*it);
    //    }
    //}

    //if (!deadGameObjectList.empty())
    //{
    //    for (auto& gameObject : deadGameObjectList) {
    //        if (SharedPtr spriteComponent = gameObject->GetComponentByComponentType(kSpriteComponent)) {
    //            SharedPtr sprite = std::dynamic_pointer_cast<SpriteComponent>(spriteComponent);
    //            if (sprite) {
    //                SharedPtr spriteObject = sprite->GetSprite();
    //                SpriteLayerList[0]->RemoveSprite(spriteObject);
    //            }
    //        }
    //        gameObject->Destroy();
    //    }

    //    GameObjectList.erase(std::remove_if(GameObjectList.begin(), GameObjectList.end(),
    //        [&](const SharedPtr<GameObject>& gameObject) {
    //            return !gameObject->GameObjectAlive;
    //        }),
    //        GameObjectList.end());
    //}
}

GameSystem::GameSystem()
{
}

GameSystem::~GameSystem()
{
}

void GameSystem::Update(const float& deltaTime)
{
    //DestroyDeadGameObjects();
    //VkCommandBuffer commandBuffer = renderer.BeginSingleTimeCommands();
    //for (auto& spriteLayer : renderSystem.spr)
    //{
    //    spriteLayer.Update(commandBuffer, deltaTime);
    //}
    //renderer.EndSingleTimeCommands(commandBuffer);
}
