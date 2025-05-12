#include "JsonPipeline.h"
#include "AssetManager.h"
#include "RenderSystem.h"
#include "VulkanPipeline.h"

JsonPipeline::JsonPipeline()
{
}

JsonPipeline::JsonPipeline(VkGuid& renderPassId, uint renderPipelineId, String jsonPath, VkRenderPass renderPass, uint constBufferSize, ivec2& renderPassResolution)
{
    RenderPipelineId = renderPipelineId;
    nlohmann::json json = Json::ReadJson(jsonPath);
    renderSystem.renderPipelineModelList[RenderPipelineId] = RenderPipelineModel::from_json(json);

    GPUIncludes include =
    {
        .vertexProperties = renderSystem.GetVertexPropertiesBuffer(),
        .indexProperties = renderSystem.GetIndexPropertiesBuffer(),
        //        .transformProperties = renderSystem.GetTransformPropertiesBuffer(gpuImport.MeshList),
        .meshProperties = renderSystem.GetMeshPropertiesBuffer(),
        .texturePropertiesList = renderSystem.GetTexturePropertiesBuffer(renderPassId, renderSystem.InputTextureList[RenderPipelineId]),
        .materialProperties = renderSystem.GetMaterialPropertiesBuffer(renderPassId)
    };

    DescriptorPool = Pipeline_CreateDescriptorPool(*renderSystem.Device.get(), renderSystem.renderPipelineModelList[RenderPipelineId], include);
    DescriptorSetLayoutList = Pipeline_CreateDescriptorSetLayout(*renderSystem.Device.get(), renderSystem.renderPipelineModelList[RenderPipelineId], include);
    DescriptorSetList = Pipeline_AllocateDescriptorSets(*renderSystem.Device.get(), DescriptorPool, renderSystem.renderPipelineModelList[RenderPipelineId], DescriptorSetLayoutList);
    Pipeline_UpdateDescriptorSets(*renderSystem.Device.get(), DescriptorSetList, renderSystem.renderPipelineModelList[RenderPipelineId], include);
    PipelineLayout = Pipeline_CreatePipelineLayout(*renderSystem.Device.get(), DescriptorSetLayoutList, constBufferSize);
    Pipeline = Pipeline_CreatePipeline(*renderSystem.Device.get(), renderPass, PipelineLayout, PipelineCache, renderSystem.renderPipelineModelList[RenderPipelineId], renderPassResolution);
}

JsonPipeline::~JsonPipeline()
{
}

void JsonPipeline::RecreateSwapchain(VkRenderPass renderPass, uint constBufferSize, int newWidth, int newHeight)
{
    //GPUIncludes include =
    //{
    //    .vertexProperties = renderSystem.GetVertexPropertiesBuffer(),
    //    .indexProperties = renderSystem.GetIndexPropertiesBuffer(),
    //    //        .transformProperties = renderSystem.GetTransformPropertiesBuffer(gpuImport.MeshList),
    //    .meshProperties = renderSystem.GetMeshPropertiesBuffer(),
    //    .texturePropertiesList = renderSystem.GetTexturePropertiesBuffer(renderSystem.InputTextureList[RenderPipelineId]),
    //    .materialProperties = renderSystem.GetMaterialPropertiesBuffer()
    //};

    //Destroy();

    //ivec2 renderPassResolution = ivec2(newWidth, newHeight);
    //DescriptorPool = Pipeline_CreateDescriptorPool(*renderSystem.Device.get(), renderSystem.renderPipelineModelList[RenderPipelineId], include);
    //DescriptorSetLayoutList = Pipeline_CreateDescriptorSetLayout(*renderSystem.Device.get(), renderSystem.renderPipelineModelList[RenderPipelineId], include);
    //DescriptorSetList = Pipeline_AllocateDescriptorSets(*renderSystem.Device.get(), DescriptorPool, renderSystem.renderPipelineModelList[RenderPipelineId], DescriptorSetLayoutList);
    //Pipeline_UpdateDescriptorSets(*renderSystem.Device.get(), DescriptorSetList, renderSystem.renderPipelineModelList[RenderPipelineId], include);
    //PipelineLayout = Pipeline_CreatePipelineLayout(*renderSystem.Device.get(), DescriptorSetLayoutList, constBufferSize);
    //Pipeline = Pipeline_CreatePipeline(*renderSystem.Device.get(), renderPass, PipelineLayout, PipelineCache, renderSystem.renderPipelineModelList[RenderPipelineId], renderPassResolution);
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
