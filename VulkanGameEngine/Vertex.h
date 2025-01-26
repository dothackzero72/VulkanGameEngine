#pragma once
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <vector>
#include <vulkan/vulkan_core.h>
#include <TypeDef.h>

struct Vertex2D
{
    vec2 Position = vec2(0.0f);
    vec2 UV = vec2(0.0f);

    Vertex2D()
    {
        Position = vec2(0.0f);

    }

    Vertex2D(vec2 position, vec2 uv)
    {
        Position = position;
        UV = uv;
    }

    static std::vector<VkVertexInputBindingDescription> GetBindingDescriptions()
    {
        return std::vector<VkVertexInputBindingDescription>
        {
            VkVertexInputBindingDescription
            {
                .binding = 0,
                .stride = sizeof(Vertex2D),
                .inputRate = VK_VERTEX_INPUT_RATE_VERTEX
            }
        };
    }

    static std::vector<VkVertexInputAttributeDescription> GetAttributeDescriptions()
    {
        return std::vector<VkVertexInputAttributeDescription>
        {
            VkVertexInputAttributeDescription
            {
                .location = 0,
                .binding = 0,
                .format = VK_FORMAT_R32G32_SFLOAT,
                .offset = offsetof(Vertex2D, Position)
            },
            VkVertexInputAttributeDescription
            {
                .location = 1,
                .binding = 0,
                .format = VK_FORMAT_R32G32_SFLOAT,
                .offset = offsetof(Vertex2D, UV)
            },
        };
    }
};

struct SpriteInstanceStruct
{
    vec2 UVOffset;           // 8 bytes
    vec2 SpriteSize;         // 8 bytes
    ivec2 FlipSprite;        // 8 bytes
    vec4 Color;              // 16 bytes
    mat4 InstanceTransform;   // 64 bytes
    int MaterialID;     // 8 bytes
    int buffer;

    SpriteInstanceStruct()
        : UVOffset(vec2(0.0f)), SpriteSize(vec2(0.0f)), FlipSprite(ivec2(0)), Color(vec4(0.0f)), MaterialID(0), InstanceTransform(mat4(1.0f))
    {
    }

    SpriteInstanceStruct(vec2 spriteSize, vec2 uv, ivec2 flipSprite, vec4 color, uint materialID, mat4 instanceTransform)
        : UVOffset(uv), SpriteSize(spriteSize), FlipSprite(flipSprite), Color(color), MaterialID(materialID), InstanceTransform(instanceTransform)
    {
    }
};

struct SpriteInstanceVertex2D
{
    vec2 UVOffset;           // 8 bytes
    vec2 SpriteSize;         // 8 bytes
    ivec2 FlipSprite;        // 8 bytes
    vec4 Color;              // 16 bytes
    mat4 InstanceTransform;   // 64 bytes
    int MaterialID;     // 8 bytes
    int buffer = INT32_MAX;

    SpriteInstanceVertex2D()
        : UVOffset(vec2(0.0f)), SpriteSize(vec2(0.0f)), FlipSprite(ivec2(0)), Color(vec4(0.0f)), MaterialID(0), InstanceTransform(mat4(1.0f))
    {
    }

    SpriteInstanceVertex2D(vec2 uvOffset, vec2 spriteSize, ivec2 flipSprite, vec4 color, uint64_t materialID, mat4 instanceTransform)
        : UVOffset(uvOffset), SpriteSize(spriteSize), FlipSprite(flipSprite), Color(color), MaterialID(materialID), InstanceTransform(instanceTransform)
    {
    }

    static std::vector<VkVertexInputBindingDescription> GetBindingDescriptions()
    {
        return std::vector<VkVertexInputBindingDescription>
        {
            VkVertexInputBindingDescription
            {
                .binding = 1,
                .stride = sizeof(SpriteInstanceVertex2D),
                .inputRate = VK_VERTEX_INPUT_RATE_INSTANCE
            }
        };
    }

    static std::vector<VkVertexInputAttributeDescription> GetAttributeDescriptions()
    {
        return std::vector<VkVertexInputAttributeDescription>
        {
                VkVertexInputAttributeDescription
            {
                .location = 2,
                .binding = 1,
                .format = VK_FORMAT_R32G32_SFLOAT,
                .offset = offsetof(SpriteInstanceVertex2D, UVOffset)
            },
                VkVertexInputAttributeDescription
            {
                .location = 3,
                .binding = 1,
                .format = VK_FORMAT_R32G32_SFLOAT,
                .offset = offsetof(SpriteInstanceVertex2D, SpriteSize)
            },
                VkVertexInputAttributeDescription
            {
                .location = 4,
                .binding = 1,
                .format = VK_FORMAT_R32G32_SINT,
                .offset = offsetof(SpriteInstanceVertex2D, FlipSprite)
            },
                VkVertexInputAttributeDescription
            {
                .location = 5,
                .binding = 1,
                .format = VK_FORMAT_R32G32B32A32_SFLOAT,
                .offset = offsetof(SpriteInstanceVertex2D, Color)
            },
                VkVertexInputAttributeDescription
            {
                .location = 6,
                .binding = 1,
                .format = VK_FORMAT_R32G32B32A32_SFLOAT,
                .offset = offsetof(SpriteInstanceVertex2D, InstanceTransform)
            },
                VkVertexInputAttributeDescription
            {
                .location = 7,
                .binding = 1,
                .format = VK_FORMAT_R32G32B32A32_SFLOAT,
                .offset = offsetof(SpriteInstanceVertex2D, InstanceTransform) + sizeof(vec4)
            },
                VkVertexInputAttributeDescription
            {
                .location = 8,
                .binding = 1,
                .format = VK_FORMAT_R32G32B32A32_SFLOAT,
                .offset = offsetof(SpriteInstanceVertex2D, InstanceTransform) + sizeof(vec4) * 2
            },
                VkVertexInputAttributeDescription
            {
                .location = 9,
                .binding = 1,
                .format = VK_FORMAT_R32G32B32A32_SFLOAT,
                .offset = offsetof(SpriteInstanceVertex2D, InstanceTransform) + sizeof(vec4) * 3
            },
                VkVertexInputAttributeDescription
            {
                .location = 10,
                .binding = 1,
                .format = VK_FORMAT_R32_SINT,
                .offset = offsetof(SpriteInstanceVertex2D, MaterialID)
            },
                    VkVertexInputAttributeDescription
                {
                    .location = 11,
                    .binding = 1,
                    .format = VK_FORMAT_R32_SINT,
                    .offset = offsetof(SpriteInstanceVertex2D, buffer)
                }
        };
    }
};