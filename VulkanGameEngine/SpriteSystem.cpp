#include "SpriteSystem.h"
#include "RenderSystem.h"
#include "BufferSystem.h"
#include "TextureSystem.h"
#include "GameObjectSystem.h"
#include "LevelSystem.h"
#include "MaterialSystem.h"
#include "MeshSystem.h"
#include <limits>
#include <algorithm>

SpriteSystem spriteSystem = SpriteSystem();

SpriteSystem::SpriteSystem()
{
    SpriteList.reserve(10000);
    SpriteInstanceList.reserve(10000);
    SpriteIdToListIndexMap.reserve(10000);
    SpriteInstanceBufferIdMap.reserve(10000);
    SpriteBatchLayerList.reserve(10000);
}

SpriteSystem::~SpriteSystem()
{
}

void SpriteSystem::BatchSpriteUpdate(SpriteInstanceStruct* spriteInstanceList, Sprite* spriteList, const Transform2DComponent* transform2DList, const SpriteVram* vramList, const Animation2D* animationList, const AnimationFrames* frameList, const Material* materialList, size_t spriteCount, float deltaTime) 
{
    for (size_t x = 0; x < spriteCount; x++) 
    {
        glm::mat4 spriteMatrix = glm::mat4(1.0f);
        if (spriteList[x].LastSpritePosition != spriteList[x].SpritePosition) {
            spriteMatrix = glm::translate(spriteMatrix, glm::vec3(transform2DList[x].GameObjectPosition.x, transform2DList[x].GameObjectPosition.y, 0.0f));
            spriteList[x].LastSpritePosition = spriteList[x].SpritePosition;
        }
        if (spriteList[x].LastSpriteRotation != spriteList[x].SpriteRotation) {
            spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2DList[x].GameObjectRotation.x), glm::vec3(1.0f, 0.0f, 0.0f));
            spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2DList[x].GameObjectRotation.y), glm::vec3(0.0f, 1.0f, 0.0f));
            spriteMatrix = glm::rotate(spriteMatrix, glm::radians(0.0f), glm::vec3(0.0f, 0.0f, 1.0f));
            spriteList[x].LastSpriteRotation = spriteList[x].SpriteRotation;
        }
        if (spriteList[x].LastSpriteScale != spriteList[x].SpriteScale) {
            spriteMatrix = glm::scale(spriteMatrix, glm::vec3(transform2DList[x].GameObjectScale.x, transform2DList[x].GameObjectScale.y, 1.0f));
            spriteList[x].LastSpriteScale = spriteList[x].SpriteScale;
        }

        spriteList[x].CurrentFrameTime += deltaTime;
        if (spriteList[x].CurrentFrameTime >= animationList[x].FrameHoldTime) 
        {
            spriteList[x].CurrentFrame += 1;
            spriteList[x].CurrentFrameTime = 0.0f;
            if (spriteList[x].CurrentFrame >= spriteCount) 
            {
                spriteList[x].CurrentFrame = 0;
            }
        }

        const ivec2& currentFrame = frameList[x][spriteList[x].CurrentFrame];
        spriteInstanceList[x].SpritePosition = transform2DList[x].GameObjectPosition;
        spriteInstanceList[x].SpriteSize = vramList[x].SpriteSize;
        spriteInstanceList[x].MaterialID = materialList[x].ShaderMaterialBufferIndex;
        spriteInstanceList[x].InstanceTransform = spriteMatrix;
        spriteInstanceList[x].FlipSprite = spriteList[x].FlipSprite;
        spriteInstanceList[x].UVOffset = glm::vec4(vramList[x].SpriteUVSize.x * currentFrame.x, vramList[x].SpriteUVSize.y * currentFrame.y, vramList[x].SpriteUVSize.x, vramList[x].SpriteUVSize.y);
    }
}

SpriteInstanceStruct SpriteSystem::PerSpriteUpdate(Sprite& sprite, float deltaTime)
{
    const Transform2DComponent& transform2D = gameObjectSystem.FindTransform2DComponent(sprite.GameObjectId);
    const SpriteVram& vram = spriteSystem.FindVramSprite(sprite.SpriteVramId);
    const Animation2D& animation = spriteSystem.FindSpriteAnimation(sprite.CurrentAnimationID);
    const AnimationFrames& frameList = spriteSystem.FindSpriteAnimationFrames(vram.VramSpriteID)[sprite.CurrentAnimationID];
    const Material& material = materialSystem.FindMaterial(vram.SpriteMaterialID);
    const ivec2& currentFrame = frameList[sprite.CurrentFrame];

    mat4 spriteMatrix = mat4(1.0f);
    if (sprite.LastSpritePosition != sprite.SpritePosition)
    {
        spriteMatrix = glm::translate(spriteMatrix, vec3(transform2D.GameObjectPosition.x, transform2D.GameObjectPosition.y, 0.0f));
        sprite.LastSpritePosition == sprite.SpritePosition;
    }
    if (sprite.LastSpriteRotation != sprite.SpriteRotation)
    {
        spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2D.GameObjectRotation.x), vec3(1.0f, 0.0f, 0.0f));
        spriteMatrix = glm::rotate(spriteMatrix, glm::radians(transform2D.GameObjectRotation.y), vec3(0.0f, 1.0f, 0.0f));
        spriteMatrix = glm::rotate(spriteMatrix, glm::radians(0.0f), vec3(0.0f, 0.0f, 1.0f));
        sprite.LastSpriteRotation == sprite.SpriteRotation;
    }
    if (sprite.LastSpriteScale != sprite.SpriteScale)
    {
        spriteMatrix = glm::scale(spriteMatrix, vec3(transform2D.GameObjectScale.x, transform2D.GameObjectScale.y, 1.0f));
        sprite.LastSpriteScale == sprite.SpriteScale;
    }

    sprite.CurrentFrameTime += deltaTime;
    if (sprite.CurrentFrameTime >= animation.FrameHoldTime) {
        sprite.CurrentFrame += 1;
        sprite.CurrentFrameTime = 0.0f;
        if (sprite.CurrentFrame >= frameList.size())
        {
            sprite.CurrentFrame = 0;
        }
    }

    SpriteInstanceStruct spriteInstance;
    spriteInstance.SpritePosition = transform2D.GameObjectPosition;
    spriteInstance.SpriteSize = vram.SpriteSize;
    spriteInstance.MaterialID = material.ShaderMaterialBufferIndex;
    spriteInstance.InstanceTransform = spriteMatrix;
    spriteInstance.FlipSprite = sprite.FlipSprite;
    spriteInstance.UVOffset = vec4(vram.SpriteUVSize.x * currentFrame.x, vram.SpriteUVSize.y * currentFrame.y, vram.SpriteUVSize.x, vram.SpriteUVSize.y);

    return spriteInstance;
}

void SpriteSystem::AddSprite(GameObjectID gameObjectId, VkGuid& spriteVramId)
{
    SpriteList.emplace_back(Sprite(gameObjectId, spriteVramId));
    SpriteInstanceList.emplace_back(SpriteInstanceStruct());
    SpriteIdToListIndexMap[gameObjectId] = SpriteList.size();
}

void SpriteSystem::AddSpriteBatchLayer(RenderPassGuid& renderPassId)
{
    SpriteBatchLayerList.emplace_back(SpriteBatchLayer(renderPassId));
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
    if (SpriteList.size() > 100)
    {
        std::vector<Transform2DComponent> transform2D(SpriteInstanceList.size());
        std::vector<SpriteVram> vram(SpriteInstanceList.size());
        std::vector<Animation2D> animation(SpriteInstanceList.size());
        std::vector<AnimationFrames> frameList(SpriteInstanceList.size());
        std::vector<Material> material(SpriteInstanceList.size());

        for (size_t x = 0; x < SpriteInstanceList.size(); ++x)
        {
            const auto& instance = SpriteInstanceList[x];
            const auto& sprite = SpriteList[x];
            transform2D[x] = gameObjectSystem.FindTransform2DComponent(sprite.GameObjectId);
            vram[x] = FindVramSprite(sprite.SpriteVramId);
            animation[x] = FindSpriteAnimation(sprite.CurrentAnimationID);
            frameList[x] = spriteSystem.FindSpriteAnimationFrames(vram[x].VramSpriteID)[sprite.CurrentAnimationID];
            material[x] = materialSystem.FindMaterial(vram[x].SpriteMaterialID);
        }
        BatchSpriteUpdate(SpriteInstanceList.data(), SpriteList.data(), transform2D.data(), vram.data(), animation.data(), frameList.data(), material.data(), SpriteInstanceList.size(), deltaTime);
    }
    else
    {
        for (int x = 0; x < SpriteList.size(); x++)
        {
            SpriteInstanceList[x] = PerSpriteUpdate(SpriteList[x], deltaTime);
        }
    }

    VkCommandBuffer commandBuffer = renderSystem.BeginSingleTimeCommands();
    for (auto& spriteLayer : SpriteBatchLayerList)
    {
        spriteLayer.Update(commandBuffer, deltaTime);
    }
    renderSystem.EndSingleTimeCommands(commandBuffer);
}

void SpriteSystem::SetSpriteAnimation(Sprite& sprite, Sprite::SpriteAnimationEnum spriteAnimation)
{
    if (sprite.CurrentAnimationID == spriteAnimation)
    {
        return;
    }

    sprite.CurrentAnimationID = spriteAnimation;
    sprite.CurrentFrame = 0;
    sprite.CurrentFrameTime = 0.0f;
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

const SpriteVram& SpriteSystem::FindVramSprite(VkGuid vramSpriteId)
{
    auto it = std::find_if(SpriteVramList.begin(), SpriteVramList.end(),
        [vramSpriteId](const SpriteVram& sprite)
        {
            return sprite.VramSpriteID == vramSpriteId;
        });
    return *it;
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

 Vector<AnimationFrames>& SpriteSystem::FindSpriteAnimationFrames(const VkGuid& vramSpriteId)
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

Vector<SpriteBatchLayer> SpriteSystem::FindSpriteBatchLayer(RenderPassGuid& guid) 
{
    std::vector<SpriteBatchLayer> matchingLayers;
    std::copy_if(SpriteBatchLayerList.begin(), SpriteBatchLayerList.end(),
        std::back_inserter(matchingLayers),
        [guid](const SpriteBatchLayer& sprite) {
            return sprite.RenderPassId == guid;
        });
    return matchingLayers;
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

    auto it = std::find_if(SpriteVramList.begin(), SpriteVramList.end(),
        [vramId](const SpriteVram& sprite)
        {
            return sprite.VramSpriteID == vramId;
        });
    if (it != SpriteVramList.end())
    {
        return vramId;
    }

    const Material& material = materialSystem.FindMaterial(materialId);
    const Texture& texture = textureSystem.FindTexture(material.AlbedoMapId);

    SpriteVramList.emplace_back(VRAM_LoadSpriteVRAM(spriteVramPath.c_str(), material, texture));
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