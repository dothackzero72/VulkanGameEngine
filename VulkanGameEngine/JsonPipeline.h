#pragma once
#include <vulkan/vulkan_core.h>
#include "Typedef.h"
#include "JsonRenderPass.h"
#include "Mesh.h"
#include <VulkanPipeline.h>
#include <json.h>
#include "vertex.h"

class Material;
class Texture;


struct GPUImport
{
    Vector<SpriteMesh> MeshList;
    Vector<Texture> TextureList;
};

class JsonPipeline
{
    friend class JsonRenderPass;

private:
    SharedPtr<JsonRenderPass> ParentRenderPass;
public:
    String Name;
    VkDescriptorPool DescriptorPool = VK_NULL_HANDLE;
    Vector<VkDescriptorSetLayout> DescriptorSetLayoutList;
    Vector<VkDescriptorSet> DescriptorSetList;
    VkPipeline Pipeline = VK_NULL_HANDLE;
    VkPipelineLayout PipelineLayout = VK_NULL_HANDLE;
    VkPipelineCache PipelineCache = VK_NULL_HANDLE;

    JsonPipeline();
    JsonPipeline(String jsonPath, VkRenderPass renderPass, GPUImport gpuImport, const Vector<VkVertexInputBindingDescription>& vertexBindings, const Vector<VkVertexInputAttributeDescription>& vertexAttributes, uint constBufferSize, ivec2& renderPassResolution);
    ~JsonPipeline();

    void Destroy();
};