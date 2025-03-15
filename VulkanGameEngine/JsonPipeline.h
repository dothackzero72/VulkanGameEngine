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
    Vector<SharedPtr<Mesh<Vertex2D>>> MeshList;
    Vector<SharedPtr<Texture>> TextureList;
    Vector<SharedPtr<Material>> MaterialList;
};

class JsonPipeline
{
    friend class JsonRenderPass;

private:
    SharedPtr<JsonRenderPass> ParentRenderPass;

    void LoadDescriptorSets(RenderPipelineModel& model, GPUImport& gpuImport);
    void LoadPipeline(RenderPipelineModel& model, VkRenderPass renderPass, const Vector<VkVertexInputBindingDescription>& vertexBindings, const Vector<VkVertexInputAttributeDescription>& vertexAttributes, uint ConstBufferSize);

protected:

    template<class T>
    const Vector<VkDescriptorBufferInfo> GetVertexPropertiesBuffer(Vector<SharedPtr<Mesh<T>>>& meshList)
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

    template<class T>
    const Vector<VkDescriptorBufferInfo> GetIndexPropertiesBuffer(Vector<SharedPtr<Mesh<T>>>& meshList)
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

    template<class T>
    const Vector<VkDescriptorBufferInfo> GetGameObjectTransformBuffer(Vector<SharedPtr<Mesh<T>>>& meshList)
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
                mesh->GetTransformBuffer(transformPropertiesBuffer);
            }
        }

        return transformPropertiesBuffer;
    }

    template<class T>
    const Vector<VkDescriptorBufferInfo> GetMeshPropertiesBuffer(Vector<SharedPtr<Mesh<T>>>& meshList)
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
                meshPropertiesBuffer.emplace_back(mesh->GetMeshPropertiesBuffer());
            }
        }

        return meshPropertiesBuffer;
    }

    const Vector<VkDescriptorImageInfo>  GetTexturePropertiesBuffer(Vector<SharedPtr<Texture>>& textureList);
    const Vector<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer(Vector<SharedPtr<Material>>& materialList);

public:
    String Name;
    VkDescriptorPool DescriptorPool = VK_NULL_HANDLE;
    Vector<VkDescriptorSetLayout> DescriptorSetLayoutList;
    Vector<VkDescriptorSet> DescriptorSetList;
    VkPipeline Pipeline = VK_NULL_HANDLE;
    VkPipelineLayout PipelineLayout = VK_NULL_HANDLE;
    VkPipelineCache PipelineCache = VK_NULL_HANDLE;

    JsonPipeline();
    JsonPipeline(String jsonPath, VkRenderPass renderPass, GPUImport gpuImport, const Vector<VkVertexInputBindingDescription>& vertexBindings, const Vector<VkVertexInputAttributeDescription>& vertexAttributes, uint constBufferSize);
    ~JsonPipeline();

    void Destroy();
};