#include "VRAM.h"
#include "MemorySystem.h"

SpriteVram VRAM_LoadSpriteVRAM(const char* spritePath, const Material& material, const Texture& texture)
{
    nlohmann::json json = Json::ReadJson(spritePath);
    ivec2 spritePixelSize = ivec2{ json["SpritePixelSize"][0], json["SpritePixelSize"][1] };
    ivec2 spriteCells = ivec2(texture.width / spritePixelSize.x, texture.height / spritePixelSize.y);
    ivec2 spriteScale = ivec2{ json["SpriteScale"][0], json["SpriteScale"][1] };
    
    return SpriteVram
    {
        .VramSpriteID = VkGuid(json["VramSpriteId"].get<String>().c_str()),
        .SpriteMaterialID = material.materialGuid,
        .SpriteLayer = json["SpriteLayer"],
        .SpriteColor = vec4{ json["SpriteColor"][0], json["SpriteColor"][1], json["SpriteColor"][2], json["SpriteColor"][3] },
        .SpritePixelSize = ivec2{ json["SpritePixelSize"][0], json["SpritePixelSize"][1] },
        .SpriteScale = ivec2{ json["SpriteScale"][0], json["SpriteScale"][1] },
        .SpriteCells = ivec2(texture.width / spritePixelSize.x, texture.height / spritePixelSize.y),
        .SpriteUVSize = vec2(1.0f / (float)spriteCells.x, 1.0f / (float)spriteCells.y),
        .SpriteSize = vec2(spritePixelSize.x * spriteScale.x, spritePixelSize.y * spriteScale.y),
    };
}

void VRAM_LoadSpriteAnimation(const char* spritePath, Animation2D* animationListPtr, vec2* animationFrameListPtr, size_t& animationListCount, size_t& animationFrameCount)
{
    Vector<Animation2D> animationList;
    Vector<vec2> animationFrameList;

    nlohmann::json json = Json::ReadJson(spritePath);
    for (size_t x = 0; x < json["AnimationList"].size(); ++x)
    {
        Animation2D animation =
        {
            .AnimationId = json["AnimationList"][x]["AnimationId"].get<uint>(),
            .FrameHoldTime = json["AnimationList"][x]["FrameHoldTime"].get<float>()
        };
        animationList.push_back(animation);

        AnimationFrames frameList;
        for (size_t y = 0; y < json["AnimationList"][x]["FrameList"].size(); ++y)
        {
            vec2 frame = 
            {
                json["AnimationList"][x]["FrameList"][y][0].get<float>(),
                json["AnimationList"][x]["FrameList"][y][1].get<float>()
            };
            animationFrameList.emplace_back(frame);
        }
    }

    animationListCount = animationList.size();
    animationFrameCount = animationFrameList.size();

    animationListPtr = memorySystem.AddPtrBuffer<Animation2D>(animationList.size(), __FILE__, __LINE__, __func__);
    std::memcpy(animationListPtr, animationList.data(), animationList.size() * sizeof(Animation2D));

    animationFrameListPtr = memorySystem.AddPtrBuffer<vec2>(animationFrameList.size(), __FILE__, __LINE__, __func__);
    std::memcpy(animationFrameListPtr, animationFrameList.data(), animationFrameList.size() * sizeof(vec2));
}

void VRAM_DeleteSpriteAnimation(Animation2D* animationListPtr, vec2* animationFrameListPtr)
{
    memorySystem.RemovePtrBuffer<Animation2D>(animationListPtr);
    memorySystem.RemovePtrBuffer<vec2>(animationFrameListPtr);
}