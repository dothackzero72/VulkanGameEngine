#pragma once
#include <vulkan/vulkan_core.h>
#include "Typedef.h"
#include "JsonRenderPass.h"
#include "Mesh.h"
#include <VulkanPipeline.h>
#include "json.h"
#include "vertex.h"

class Material;
class Texture;


struct GPUImport
{
    SharedPtr<Vector<SharedPtr<Mesh<Vertex2D>>>> MeshList;
    SharedPtr<Vector<SharedPtr<Texture>>> TextureList;
    SharedPtr<Vector<SharedPtr<Material>>> MaterialList;
};

class JsonPipeline
{
    friend class JsonRenderPass;

private:
    SharedPtr<JsonRenderPass> ParentRenderPass;

    void LoadDescriptorSets(RenderPipelineModel& model, GPUImport& gpuImport);
    void LoadPipeline(RenderPipelineModel& model, VkRenderPass renderPass, uint ConstBufferSize);

protected:

    template<class T>
    const Vector<VkDescriptorBufferInfo> GetVertexPropertiesBuffer(SharedPtr<Vector<SharedPtr<Mesh<T>>>> meshList)
    {
        if (!meshList.get())
        {
            return Vector<VkDescriptorBufferInfo>();
        }

        Vector<VkDescriptorBufferInfo> vertexPropertiesBuffer;
        if ((*meshList.get()).size() == 0)
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
            for (auto& mesh : *meshList.get())
            {
                mesh->GetVertexPropertiesBuffer();
            }
        }

        return vertexPropertiesBuffer;
    }

    template<class T>
    const Vector<VkDescriptorBufferInfo> GetIndexPropertiesBuffer(SharedPtr<Vector<SharedPtr<Mesh<T>>>> meshList)
    {
        if (!meshList.get())
        {
            return Vector<VkDescriptorBufferInfo>();
        }

        std::vector<VkDescriptorBufferInfo>	indexPropertiesBuffer;
        if ((*meshList.get()).size() == 0)
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
            for (auto& mesh : *meshList.get())
            {
                mesh->GetIndexPropertiesBuffer();
            }
        }
        return indexPropertiesBuffer;
    }

    template<class T>
    const Vector<VkDescriptorBufferInfo> GetGameObjectTransformBuffer(SharedPtr<Vector<SharedPtr<Mesh<T>>>> meshList)
    {
        if (!meshList.get())
        {
            return Vector<VkDescriptorBufferInfo>();
        }

        std::vector<VkDescriptorBufferInfo>	transformPropertiesBuffer;
        if ((*meshList.get()).size() == 0)
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
            for (auto& mesh : *meshList.get())
            {
                mesh->GetTransformBuffer(transformPropertiesBuffer);
            }
        }

        return transformPropertiesBuffer;
    }

    template<class T>
    const Vector<VkDescriptorBufferInfo> GetMeshPropertiesBuffer(SharedPtr<Vector<SharedPtr<Mesh<T>>>> meshList)
    {
        if (!meshList.get())
        {
            return Vector<VkDescriptorBufferInfo>();
        }

        Vector<VkDescriptorBufferInfo> meshPropertiesBuffer;
        if ((*meshList.get()).size() == 0)
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
            for (auto& mesh : *meshList.get())
            {
                meshPropertiesBuffer.emplace_back(mesh->GetMeshPropertiesBuffer());
            }
        }

        return meshPropertiesBuffer;
    }

    const Vector<VkDescriptorImageInfo>  GetTexturePropertiesBuffer(SharedPtr<Vector<SharedPtr<Texture>>> textureList);
    const Vector<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer(SharedPtr<Vector<SharedPtr<Material>>> materialList);

public:
    String Name;
    VkDescriptorPool DescriptorPool = VK_NULL_HANDLE;
    Vector<VkDescriptorSetLayout> DescriptorSetLayoutList;
    Vector<VkDescriptorSet> DescriptorSetList;
    VkPipeline Pipeline = VK_NULL_HANDLE;
    VkPipelineLayout PipelineLayout = VK_NULL_HANDLE;
    VkPipelineCache PipelineCache = VK_NULL_HANDLE;

    JsonPipeline();
    JsonPipeline(String jsonPath, VkRenderPass renderPass, GPUImport gpuImport, uint constBufferSize);
    ~JsonPipeline();

    void Destroy();
};