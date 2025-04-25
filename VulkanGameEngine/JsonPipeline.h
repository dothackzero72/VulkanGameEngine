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
    Vector<SharedPtr<Texture>> TextureList;
};

class SpriteBatchLayer;
class JsonPipeline
{
    friend class JsonRenderPass;

private:
    SharedPtr<JsonRenderPass> ParentRenderPass;

protected:

    const Vector<VkDescriptorBufferInfo> GetVertexPropertiesBuffer(Vector<SpriteMesh>& meshList)
    {
        Vector<VkDescriptorBufferInfo> vertexPropertiesBuffer;
        if (meshList.size() == 0)
        {
            vertexPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
                {
                    .buffer = VK_NULL_HANDLE,
                    .offset = 0,
                    .range = VK_WHOLE_SIZE
                });
        }
        else
        {
            for (auto& mesh : meshList)
            {
               // mesh->GetVertexBuffer(vertexPropertiesBuffer);
            }
        }

        return vertexPropertiesBuffer;
    }

    const Vector<VkDescriptorBufferInfo> GetIndexPropertiesBuffer(Vector<SpriteMesh>& meshList)
    {
        std::vector<VkDescriptorBufferInfo>	indexPropertiesBuffer;
        if (meshList.size() == 0)
        {
            indexPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
                {
                    .buffer = VK_NULL_HANDLE,
                    .offset = 0,
                    .range = VK_WHOLE_SIZE
                });
        }
        else
        {
            for (auto& mesh : meshList)
            {
             //   mesh->GetIndexBuffer(indexPropertiesBuffer);
            }
        }
        return indexPropertiesBuffer;
    }

    const Vector<VkDescriptorBufferInfo> GetGameObjectTransformBuffer(Vector<SpriteMesh>& meshList)
    {
        std::vector<VkDescriptorBufferInfo>	transformPropertiesBuffer;
        if (meshList.size() == 0)
        {
            transformPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
                {
                    .buffer = VK_NULL_HANDLE,
                    .offset = 0,
                    .range = VK_WHOLE_SIZE
                });
        }
        else
        {
            for (auto& mesh : meshList)
            {
                mesh.GetTransformBuffer();
            }
        }

        return transformPropertiesBuffer;
    }

    const Vector<VkDescriptorBufferInfo> GetMeshPropertiesBuffer(Vector<SpriteMesh>& meshList)
    {
        Vector<VkDescriptorBufferInfo> meshPropertiesBuffer;
        if (meshList.size() == 0)
        {
            meshPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
                {
                    .buffer = VK_NULL_HANDLE,
                    .offset = 0,
                    .range = VK_WHOLE_SIZE
                });
        }
        else
        {
            for (auto& mesh : meshList)
            {
                meshPropertiesBuffer.emplace_back(mesh.GetMeshPropertiesBuffer());
            }
        }

        return meshPropertiesBuffer;
    }

    const Vector<VkDescriptorImageInfo>  GetTexturePropertiesBuffer(Vector<SharedPtr<Texture>>& textureList);
    const Vector<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer();

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