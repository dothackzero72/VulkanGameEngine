#pragma once
#include <vulkan/vulkan_core.h>
#include "Typedef.h"
#include "JsonRenderPass.h"
#include "Mesh.h"

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
    void LoadPipeline(RenderPipelineModel& model, VkRenderPass renderPass, uint ConstBufferSize);

    template<class T>
    const Vector<VkDescriptorBufferInfo> GetVertexPropertiesBuffer()
    {

    }

    const Vector<VkDescriptorBufferInfo> GetIndexPropertiesBuffer();
    const Vector<VkDescriptorBufferInfo> GetGameObjectTransformBuffer();
    const Vector<VkDescriptorBufferInfo> GetMeshPropertiesBuffer(Vector<SharedPtr<Mesh<Vertex2D>>> meshList);
    const Vector<VkDescriptorImageInfo>  GetTexturePropertiesBuffer(Vector<SharedPtr<Texture>> textureList);
    const Vector<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer(Vector<SharedPtr<Material>> materialList);

public:
    String Name;
    VkDescriptorPool DescriptorPool = VK_NULL_HANDLE;
    Vector<VkDescriptorSetLayout> DescriptorSetLayoutList;
    Vector<VkDescriptorSet> DescriptorSetList;
    VkPipeline Pipeline = VK_NULL_HANDLE;
    VkPipelineLayout PipelineLayout = VK_NULL_HANDLE;
    VkPipelineCache PipelineCache = VK_NULL_HANDLE;

    JsonPipeline();
    JsonPipeline(String jsonPath, VkRenderPass renderPass, GPUImport& gpuImport, uint constBufferSize);
    ~JsonPipeline();

    void Destroy();
};