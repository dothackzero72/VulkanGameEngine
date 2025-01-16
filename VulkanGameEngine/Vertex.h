#pragma once
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <vector>
#include <vulkan/vulkan_core.h>

struct Vertex2D
{
    vec2 Position;
    vec2 UV;
    vec4 Color;
   // uint MaterialID;

    Vertex2D()
    {
        Position = vec2(0.0f);
        UV = vec2(0.0f);
        Color = vec4(0.0f);
       // MaterialID = 0;
    }

    Vertex2D(vec2 position, vec2 uv, vec4 color)
    {
        Position = position;
        UV = uv;
        Color = color;
     //   MaterialID = materialID;
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
            VkVertexInputAttributeDescription
            {
                .location = 2,
                .binding = 0,
                .format = VK_FORMAT_R32G32B32A32_SFLOAT,
                .offset = offsetof(Vertex2D, Color)
            },
         /*   VkVertexInputAttributeDescription
            {
                .location = 3,
                .binding = 0,
                .format = VK_FORMAT_R32_UINT,
                .offset = offsetof(Vertex2D, Color)
            },*/
        };
    }
};