#include "JsonPipeline.h"
#include "MemoryManager.h"

JsonPipeline::JsonPipeline()
{
}

JsonPipeline::JsonPipeline(String jsonPath, VkRenderPass renderPass, GPUImport gpuImport, const Vector<VkVertexInputBindingDescription>& vertexBindings, const Vector<VkVertexInputAttributeDescription>& vertexAttributes, uint constBufferSize)
{
  //  ParentRenderPass = parentRenderPass;

    nlohmann::json json = Json::ReadJson(jsonPath);
    RenderPipelineModel model = RenderPipelineModel::from_json(json);

    GPUIncludes include =
    {
        .vertexProperties = GetVertexPropertiesBuffer(gpuImport.MeshList),
        .indexProperties = GetIndexPropertiesBuffer(gpuImport.MeshList),
        //        .transformProperties = GetTransformPropertiesBuffer(gpuImport.MeshList),
        .meshProperties = GetMeshPropertiesBuffer(gpuImport.MeshList),
        .texturePropertiesList = GetTexturePropertiesBuffer(gpuImport.TextureList),
        .materialProperties = GetMaterialPropertiesBuffer(gpuImport.MaterialList)
    };

    Vector<VkVertexInputBindingDescription> vertexBindingList = vertexBindings; 
    Vector<VkVertexInputAttributeDescription> vertexAttributesList = vertexAttributes;

    DescriptorSetLayoutList.resize(1);
    DescriptorPool = Pipeline_CreateDescriptorPool(cRenderer.Device, model, include);
     Pipeline_CreateDescriptorSetLayout(cRenderer.Device, model, include, DescriptorSetLayoutList);
     DescriptorSetList = Pipeline_AllocateDescriptorSets(cRenderer.Device, DescriptorPool, DescriptorSetLayoutList);
     Pipeline_UpdateDescriptorSets(cRenderer.Device, DescriptorSetList, model, include);
     Pipeline_CreatePipelineLayout(cRenderer.Device, DescriptorSetLayoutList, constBufferSize, PipelineLayout);
     Pipeline_CreatePipeline(cRenderer.Device,
         renderPass,
         PipelineLayout,
         PipelineCache,
        model,
         vertexBindingList,
         vertexAttributesList,
        Pipeline);
}

JsonPipeline::~JsonPipeline()
{
}

void JsonPipeline::Destroy()
{
    renderer.DestroyPipeline(Pipeline);
    renderer.DestroyPipelineLayout(PipelineLayout);
    renderer.DestroyPipelineCache(PipelineCache);
    renderer.DestroyDescriptorPool(DescriptorPool);
    for (auto& descriptorSet : DescriptorSetLayoutList)
    {
        renderer.DestroyDescriptorSetLayout(descriptorSet);
    }
}

const Vector<VkDescriptorImageInfo> JsonPipeline::GetTexturePropertiesBuffer(Vector<SharedPtr<Texture>>& textureList)
{
    Vector<VkDescriptorImageInfo>	texturePropertiesBuffer;
    if (textureList.size() == 0)
    {
        VkSamplerCreateInfo NullSamplerInfo =
        {
            .sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
            .magFilter = VK_FILTER_NEAREST,
            .minFilter = VK_FILTER_NEAREST,
            .mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR,
            .addressModeU = VK_SAMPLER_ADDRESS_MODE_REPEAT,
            .addressModeV = VK_SAMPLER_ADDRESS_MODE_REPEAT,
            .addressModeW = VK_SAMPLER_ADDRESS_MODE_REPEAT,
            .mipLodBias = 0,
            .anisotropyEnable = VK_TRUE,
            .maxAnisotropy = 16.0f,
            .compareEnable = VK_FALSE,
            .compareOp = VK_COMPARE_OP_ALWAYS,
            .minLod = 0,
            .maxLod = 0,
            .borderColor = VK_BORDER_COLOR_INT_OPAQUE_BLACK,
            .unnormalizedCoordinates = VK_FALSE,
        };

        VkSampler nullSampler = VK_NULL_HANDLE;
        if (vkCreateSampler(cRenderer.Device, &NullSamplerInfo, nullptr, &nullSampler))
        {
            throw std::runtime_error("Failed to create Sampler.");
        }

        VkDescriptorImageInfo nullBuffer =
        {
            .sampler = nullSampler,
            .imageView = VK_NULL_HANDLE,
            .imageLayout = VK_IMAGE_LAYOUT_UNDEFINED,
        };
        texturePropertiesBuffer.emplace_back(nullBuffer);
    }
    else
    {
        for (auto& texture : textureList)
        {
            texture->GetTexturePropertiesBuffer(texturePropertiesBuffer);
        }
    }

    return texturePropertiesBuffer;
}

const Vector<VkDescriptorBufferInfo> JsonPipeline::GetMaterialPropertiesBuffer(Vector<SharedPtr<Material>>& materialList)
{
    std::vector<VkDescriptorBufferInfo>	materialPropertiesBuffer;
    for (auto& material : materialList)
    {
        material->GetMaterialPropertiesBuffer(materialPropertiesBuffer);
    }
    return materialPropertiesBuffer;
}
