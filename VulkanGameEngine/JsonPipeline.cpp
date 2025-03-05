#include "JsonPipeline.h"
#include "MemoryManager.h"

JsonPipeline::JsonPipeline()
{
}

JsonPipeline::JsonPipeline(String jsonPath, VkRenderPass renderPass, GPUImport& gpuImport, uint constBufferSize)
{
  //  ParentRenderPass = parentRenderPass;
    nlohmann::json json = Json::ReadJson("../Pipelines/Default2DPipeline.json");

    RenderPipelineModel renderPipelineModel = RenderPipelineModel::from_json(json);

    GPUIncludes include =
    {
        .vertexProperties = GetVertexPropertiesBuffer(gpuImport.MeshList),
        .indexProperties = GetIndexPropertiesBuffer(gpuImport.MeshList),
        //        .transformProperties = GetTransformPropertiesBuffer(gpuImport.MeshList),
                .meshProperties = GetMeshPropertiesBuffer(gpuImport.MeshList),
                .texturePropertiesList = GetTexturePropertiesBuffer(gpuImport.TextureList),
                .materialProperties = GetMaterialPropertiesBuffer(gpuImport.MaterialList)
    };

    Vector<VkVertexInputBindingDescription> vertexBinding = Vertex2D::GetBindingDescriptions();
    for (auto& instanceVar : SpriteInstanceVertex2D::GetBindingDescriptions())
    {
        vertexBinding.emplace_back(instanceVar);
    }

    Vector<VkVertexInputAttributeDescription> vertexAttribute = Vertex2D::GetAttributeDescriptions();
    for (auto& instanceVar : SpriteInstanceVertex2D::GetAttributeDescriptions())
    {
        vertexAttribute.emplace_back(instanceVar);
    }


    DescriptorPool = VkPipeline_CreateDescriptorPool(cRenderer.Device, renderPipelineModel, include);
    DescriptorSetLayoutList = VkPipeline_CreateDescriptorSetLayout(cRenderer.Device, renderPipelineModel, include);
    DescriptorSetList = VkPipeline_AllocateDescriptorSets(cRenderer.Device, DescriptorPool, DescriptorSetLayoutList);
    VkPipeline_UpdateDescriptorSets(cRenderer.Device, DescriptorSetList, renderPipelineModel, include);
    VkPipeline_CreatePipelineLayout(cRenderer.Device, DescriptorSetLayoutList, constBufferSize, PipelineLayout);
    VkPipeline_CreatePipeline(cRenderer.Device, renderPass, PipelineLayout, PipelineCache, renderPipelineModel, vertexBinding, vertexAttribute, Pipeline);
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
