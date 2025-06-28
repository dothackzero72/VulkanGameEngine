#include "SpriteSystem.h"
#include "RenderSystem.h"
#include "BufferSystem.h"
#include "MeshSystem.h"
#include <limits>
#include <algorithm>

SpriteSystem spriteSystem = SpriteSystem();

SpriteSystem::SpriteSystem()
{
    SpriteList.reserve(10000);
    SpriteInstanceList.reserve(10000);
    SpriteIdToListIndexMap.reserve(10000);
    SpriteInstanceBufferMap.reserve(10000);
    SpriteBatchLayerSpriteListMap.reserve(10000);
}

SpriteSystem::~SpriteSystem()
{
}

void SpriteSystem::AddSprite(GameObjectID gameObjectId, VkGuid& spriteVramId)
{
    SpriteList.emplace_back(gameObjectId, spriteVramId);
    SpriteInstanceList.emplace_back(SpriteInstanceStruct());
    SpriteIdToListIndexMap[gameObjectId] = SpriteList.size();
}

SpriteBatchLayer SpriteSystem::AddSpriteLayer(VkGuid& renderPassId)
{
    SpriteBatchLayer spriteBatchLayer;
    //spriteBatchLayer.rendererId = renderPassId;
    //spriteBatchLayer.SpriteBatchLayerID = ++SpriteBatchLayer::NextSpriteBatchLayerID;
    //spriteBatchLayer.SpriteLayerMeshId = meshSystem.CreateSpriteLayerMesh(gameObjectSystem.SpriteVertexList, gameObjectSystem.SpriteIndexList);
    //
    //for (int x = 0; x < spriteSystem.SpriteMap.size(); x++)
    //{
    //    spriteSystem.SpriteBatchLayerObjectListMap[spriteBatchLayer.SpriteBatchLayerID].emplace_back(GameObjectID(x + 1));
    //}
    //spriteSystem.SpriteInstanceListMap[spriteBatchLayer.SpriteBatchLayerID] = Vector<SpriteInstanceStruct>(spriteSystem.SpriteBatchLayerObjectListMap[spriteBatchLayer.SpriteBatchLayerID].size());
    //spriteSystem.SpriteInstanceBufferMap[spriteBatchLayer.SpriteBatchLayerID] = bufferSystem.CreateVulkanBuffer<SpriteInstanceStruct>(renderSystem.renderer, spriteSystem.SpriteInstanceListMap[spriteBatchLayer.SpriteBatchLayerID], MeshBufferUsageSettings, MeshBufferPropertySettings, false);

    return spriteBatchLayer;
}

void SpriteSystem::Update(const float& deltaTime)
{
    for (int x = 0; x < SpriteList.size(); x++)
    {
        SpriteInstanceList[x] = SpriteList[x].Update(deltaTime);
    }
  /*  for (auto& spriteLayerPair : SpriteBatchLayerListMap)
    {
        Vector<SpriteBatchLayer> spriteBatchLayerList = spriteLayerPair.second;
        for (auto& spriteBatchLayer : spriteBatchLayerList)
        {
            const uint spriteBatchLayerId = spriteBatchLayer.SpriteBatchLayerID;

            spriteLayerPair.second.clear();
            spriteLayerPair.second.reserve(SpriteBatchLayerObjectListMap[spriteBatchLayerId].size());
            for (auto& gameObjectID : SpriteBatchLayerObjectListMap[spriteBatchLayerId])
            {
                SpriteInstanceListMap[spriteBatchLayerId].emplace_back(SpriteMap[gameObjectID].Update(commandBuffer, deltaTime));
            }

            if (SpriteBatchLayerObjectListMap[spriteBatchLayerId].size())
            {
                bufferSystem.UpdateBufferMemory(renderSystem.renderer, SpriteInstanceBufferMap[spriteBatchLayerId], SpriteInstanceListMap[spriteBatchLayerId]);
            }
        }
    }*/
}

Sprite* SpriteSystem::FindSprite(GameObjectID gameObjectId)
{
    if (SpriteList.size() <= 200)
    {
        auto it = std::find_if(SpriteList.begin(), SpriteList.end(),
            [gameObjectId](const Sprite& sprite) 
            {
                return sprite.GameObjectId == gameObjectId;
            });
        return it != SpriteList.end() ? &(*it) : nullptr;
    }
    else
    {
        auto it = SpriteIdToListIndexMap.find(gameObjectId);
        return it != SpriteIdToListIndexMap.end() ? &SpriteList[it->second] : nullptr;
    }
}

const SpriteInstanceStruct* SpriteSystem::FindSpriteInstance(GameObjectID gameObjectId)
{
    if (SpriteInstanceList.size() <= 200)
    {
        size_t spriteInstanceIndex = findSpriteIndex(gameObjectId);
        return &SpriteInstanceList[spriteInstanceIndex];
    }
    else
    {
        auto it = SpriteIdToListIndexMap.find(gameObjectId);
        return it != SpriteIdToListIndexMap.end() ? &SpriteInstanceList[it->second] : nullptr;
    }
}

size_t SpriteSystem::findSpriteIndex(GameObjectID gameObjectId) 
{
    auto it = std::find_if(SpriteList.begin(), SpriteList.end(),
        [gameObjectId](const Sprite& sprite) 
        {
            return sprite.GameObjectId == gameObjectId;
        });
    return it != SpriteList.end() ? std::distance(SpriteList.begin(), it)
        : (std::numeric_limits<size_t>::max)();
}
