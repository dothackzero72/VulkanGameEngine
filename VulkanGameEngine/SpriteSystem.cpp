#include "SpriteSystem.h"
#include "RenderSystem.h"
#include "BufferSystem.h"
#include "MeshSystem.h"
#include <limits>
#include <algorithm>
#include "TextureSystem.h"

SpriteSystem spriteSystem = SpriteSystem();

SpriteSystem::SpriteSystem()
{
    SpriteList.reserve(10000);
    SpriteInstanceList.reserve(10000);
    SpriteIdToListIndexMap.reserve(10000);
    SpriteInstanceBufferIdMap.reserve(10000);
    SpriteBatchLayerMap.reserve(10000);
}

SpriteSystem::~SpriteSystem()
{
}

void SpriteSystem::AddSprite(GameObjectID gameObjectId, VkGuid& spriteVramId)
{
    SpriteList.emplace_back(Sprite(gameObjectId, spriteVramId));
    SpriteInstanceList.emplace_back(SpriteInstanceStruct());
    SpriteIdToListIndexMap[gameObjectId] = SpriteList.size();
}

void SpriteSystem::AddSpriteBatchLayer(RenderPassGuid& renderPassId)
{
    SpriteBatchLayerMap[renderPassId].emplace_back(SpriteBatchLayer(renderPassId));
}

void SpriteSystem::AddSpriteInstanceBufferId(UM_SpriteBatchID spriteInstanceBufferId, int BufferId)
{
    SpriteInstanceBufferIdMap[spriteInstanceBufferId] = BufferId;
}

void SpriteSystem::AddSpriteInstanceLayerList(UM_SpriteBatchID spriteBatchId, Vector<SpriteInstanceStruct>& spriteInstanceList)
{
    SpriteInstanceListMap[spriteBatchId] = spriteInstanceList;
}

void SpriteSystem::AddSpriteBatchObjectList(UM_SpriteBatchID spriteBatchId, GameObjectID spriteBatchObject)
{
    SpriteBatchObjectListMap[spriteBatchId].emplace_back(spriteBatchObject);
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
    VkCommandBuffer commandBuffer = renderSystem.BeginSingleTimeCommands();
    for (auto& spriteLayerPair : SpriteBatchLayerMap)
    {
        for (auto& spriteLayer : spriteLayerPair.second)
        {
            spriteLayer.Update(commandBuffer, deltaTime);
        }
    }
    renderSystem.EndSingleTimeCommands(commandBuffer);
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

const SpriteVram& SpriteSystem::FindVramSprite(RenderPassGuid guid)
{
    auto it = SpriteVramMap.find(guid);
    if (it != SpriteVramMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("VramSprite not found for given GUID");
}

const Animation2D& SpriteSystem::FindSpriteAnimation(const UM_AnimationListID& animationId)
{
    auto it = SpriteAnimationMap.find(animationId);
    if (it != SpriteAnimationMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("Animation for Vram not found for given GUID");
}

const Vector<AnimationFrames>& SpriteSystem::FindSpriteAnimationFrames(const VkGuid& vramSpriteId)
{
    auto it = SpriteAnimationFrameListMap.find(vramSpriteId);
    if (it != SpriteAnimationFrameListMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("Animation Frames for Vram not found for given GUID");
}

const SpriteInstanceStruct* SpriteSystem::FindSpriteInstance(GameObjectID gameObjectId)
{
    if (SpriteInstanceList.size() <= 200)
    {
        size_t spriteInstanceIndex = FindSpriteIndex(gameObjectId);
        return &SpriteInstanceList[spriteInstanceIndex];
    }
    else
    {
        auto it = SpriteIdToListIndexMap.find(gameObjectId);
        return it != SpriteIdToListIndexMap.end() ? &SpriteInstanceList[it->second] : nullptr;
    }
}

const int SpriteSystem::FindSpriteInstanceBufferId(UM_SpriteBatchID spriteInstanceBufferId)
{
    auto it = SpriteInstanceBufferIdMap.find(spriteInstanceBufferId);
    if (it != SpriteInstanceBufferIdMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("SpriteInstanceBuffer not found for given GUID");
}

Vector<SpriteInstanceStruct>& SpriteSystem::FindSpriteInstanceList(UM_SpriteBatchID spriteAnimation)
{
    auto it = SpriteInstanceListMap.find(spriteAnimation);
    if (it != SpriteInstanceListMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("SpriteInstanceBuffer not found for given GUID");
}

const Vector<GameObjectID>& SpriteSystem::FindSpriteBatchObjectListMap(UM_SpriteBatchID spriteBatchObjectListId)
{
    auto it = SpriteBatchObjectListMap.find(spriteBatchObjectListId);
    if (it != SpriteBatchObjectListMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("SpriteInstanceBuffer not found for given GUID");
}

Vector<SpriteBatchLayer>& SpriteSystem::FindSpriteBatchLayer(RenderPassGuid& guid)
{
    auto it = SpriteBatchLayerMap.find(guid);
    if (it != SpriteBatchLayerMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("SpriteBatchLayerMap not found for given GUID");
}

size_t SpriteSystem::FindSpriteIndex(GameObjectID gameObjectId) 
{
    auto it = std::find_if(SpriteList.begin(), SpriteList.end(),
        [gameObjectId](const Sprite& sprite) 
        {
            return sprite.GameObjectId == gameObjectId;
        });
    return it != SpriteList.end() ? std::distance(SpriteList.begin(), it)
        : (std::numeric_limits<size_t>::max)();
}

VkGuid SpriteSystem::LoadSpriteVRAM(const String& spriteVramPath)
{
    size_t animationListCount = 0;
    size_t animationFrameCount = 0;

    nlohmann::json json = Json::ReadJson(spriteVramPath);
    VkGuid vramId = VkGuid(json["VramSpriteId"].get<String>().c_str());
    VkGuid materialId = VkGuid(json["MaterialId"].get<String>().c_str());

    auto it = SpriteVramMap.find(vramId);
    if (it != SpriteVramMap.end())
    {
        return vramId;
    }

    const Material& material = materialSystem.FindMaterial(materialId);
    const Texture& texture = textureSystem.FindTexture(material.AlbedoMapId);

    SpriteVramMap[vramId] = VRAM_LoadSpriteVRAM(spriteVramPath.c_str(), material, texture);
    Animation2D* animationListPtr = VRAM_LoadSpriteAnimations(spriteVramPath.c_str(), animationListCount);
    vec2* animationFrameListPtr = VRAM_LoadSpriteAnimationFrames(spriteVramPath.c_str(), animationFrameCount);

    Vector<Animation2D> animation2DList = Vector<Animation2D>(animationListPtr, animationListPtr + animationListCount);
    Vector<vec2> animationFrameList = Vector<vec2>(animationFrameListPtr, animationFrameListPtr + animationFrameCount);

    for (size_t x = 0; x < animation2DList.size(); x++)
    {
        SpriteAnimationMap[animation2DList[x].AnimationId] = animation2DList[x];
    }
    SpriteAnimationFrameListMap[vramId].emplace_back(animationFrameList);

    VRAM_DeleteSpriteVRAM(animationListPtr, animationFrameListPtr);
    return vramId;
}