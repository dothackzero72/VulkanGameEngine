#pragma once
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <vector>
#include <vulkan/vulkan_core.h>
#include <TypeDef.h>

struct Vertex
{
    Vertex() { }
    virtual ~Vertex() { }
    virtual  std::vector<VkVertexInputBindingDescription> GetBindingDescriptions() = 0;
    virtual std::vector<VkVertexInputAttributeDescription> GetAttributeDescriptions() = 0;
};