#include "JsonPipeline.h"
#include "AssetManager.h"
#include "RenderSystem.h"
#include <VulkanPipeline.h>

JsonPipeline::JsonPipeline()
{
}

JsonPipeline::JsonPipeline(uint renderPipelineId, String jsonPath, VkRenderPass renderPass, uint constBufferSize, ivec2& renderPassResolution)
{
    RenderPipelineId = renderPipelineId;
    nlohmann::json json = Json::ReadJson(jsonPath);
    RenderPipelineModel model = RenderPipelineModel::from_json(json);

    GPUIncludes include =
    {
        .vertexProperties = renderSystem.GetVertexPropertiesBuffer(),
        .indexProperties = renderSystem.GetIndexPropertiesBuffer(),
        //        .transformProperties = renderSystem.GetTransformPropertiesBuffer(gpuImport.MeshList),
        .meshProperties = renderSystem.GetMeshPropertiesBuffer(),
        .texturePropertiesList = renderSystem.GetTexturePropertiesBuffer(renderSystem.InputTextureList[renderPipelineId]),
        .materialProperties = renderSystem.GetMaterialPropertiesBuffer()
    };

    DescriptorPool = Pipeline_CreateDescriptorPool(*renderSystem.Device.get(), model, include);
    DescriptorSetLayoutList = Pipeline_CreateDescriptorSetLayout(*renderSystem.Device.get(), model, include);
    DescriptorSetList = Pipeline_AllocateDescriptorSets(*renderSystem.Device.get(), DescriptorPool, model, DescriptorSetLayoutList);
    Pipeline_UpdateDescriptorSets(*renderSystem.Device.get(), DescriptorSetList, model, include);
    PipelineLayout = Pipeline_CreatePipelineLayout(*renderSystem.Device.get(), DescriptorSetLayoutList, constBufferSize);
    Pipeline = Pipeline_CreatePipeline(*renderSystem.Device.get(), renderPass, PipelineLayout, PipelineCache, model, renderPassResolution);
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
