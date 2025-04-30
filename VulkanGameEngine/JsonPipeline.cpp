#include "JsonPipeline.h"
#include "AssetManager.h"
#include "RenderSystem.h"

JsonPipeline::JsonPipeline()
{
}

JsonPipeline::JsonPipeline(uint renderPipelineId, String jsonPath, VkRenderPass renderPass, const Vector<VkVertexInputBindingDescription>& vertexBindings, const Vector<VkVertexInputAttributeDescription>& vertexAttributes, uint constBufferSize, ivec2& renderPassResolution)
{
    //  ParentRenderPass = parentRenderPass;
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

    Vector<VkVertexInputBindingDescription> vertexBindingList = vertexBindings;
    Vector<VkVertexInputAttributeDescription> vertexAttributesList = vertexAttributes;

    DescriptorPool = Pipeline_CreateDescriptorPool(cRenderer.Device, model, include);
    DescriptorSetLayoutList = Pipeline_CreateDescriptorSetLayout(cRenderer.Device, model, include);
    DescriptorSetList = Pipeline_AllocateDescriptorSets(cRenderer.Device, DescriptorPool, model, DescriptorSetLayoutList);
    Pipeline_UpdateDescriptorSets(cRenderer.Device, DescriptorSetList, model, include);
    PipelineLayout = Pipeline_CreatePipelineLayout(cRenderer.Device, DescriptorSetLayoutList, constBufferSize);
    Pipeline = Pipeline_CreatePipeline(cRenderer.Device, renderPass, PipelineLayout, PipelineCache, model, renderPassResolution);
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
