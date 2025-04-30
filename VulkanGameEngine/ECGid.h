#pragma once
#include "TypeDef.h"

typedef uint UM_SpriteBatchID;
typedef uint UM_RenderPassID;
typedef uint32 UM_GameObjectID;
typedef uint32 UM_TextureID;
typedef uint32 UM_MaterialID;
typedef uint32 UM_RenderPassID;
typedef uint32 UM_PipelineID;
typedef uint32 UM_SpriteSheetID;
typedef uint32 UM_SpriteVRAMID;
typedef uint32 UM_AnimationFrameId;
typedef uint32 UM_AnimationListID;

//struct SpriteID
//{
//    uint id = 0;
//};

struct RenderPassID
{
    uint id = 0;

    RenderPassID()
    {
        id = 0;
    }

    RenderPassID(uint id)
    {
        id = id;
    }

    bool operator==(const RenderPassID& other) const
    {
        return id == other.id;
    }
};

struct SpriteBatchID
{
    uint id = 0;

    SpriteBatchID()
    {
        id = 0;
    }

    SpriteBatchID(uint id)
    {
        id = id;
    }

    bool operator==(const SpriteBatchID& other) const
    {
        return id == other.id;
    }
};

struct GameObjectID
{
    uint id = 0;

    GameObjectID()
    {
        id = 0;
    }

    GameObjectID(uint id)
    {
        id = id;
    }

    bool operator==(const GameObjectID& other) const
    {
        return id == other.id;
    }


};

struct SpriteMeshID {
    int id = 0;

    SpriteMeshID()
    {
        id = 0;
    }

    SpriteMeshID(uint id)
    {
        id = id;
    }

    bool operator==(const SpriteMeshID& other) const
    {
        return id == other.id;
    }
};

namespace std 
{
    template <>
    struct hash<RenderPassID> {
        size_t operator()(const RenderPassID& key) const {
            return std::hash<int>()(key.id);
        }
    };

    template <>
    struct hash<SpriteMeshID> {
        size_t operator()(const SpriteMeshID& key) const {
            return std::hash<int>()(key.id);
        }
    };

    template <>
    struct hash<GameObjectID> {
        size_t operator()(const GameObjectID& key) const {
            return std::hash<int>()(key.id);
        }
    };

    template <>
    struct hash<SpriteBatchID> {
        size_t operator()(const SpriteBatchID& key) const {
            return std::hash<int>()(key.id);
        }
    };
}