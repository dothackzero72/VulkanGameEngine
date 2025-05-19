#include "JsonPipeline.h"
#include "AssetManager.h"
#include "RenderSystem.h"
#include "ShaderSystem.h"

JsonPipeline::JsonPipeline()
{
}

JsonPipeline::JsonPipeline(VkGuid& renderPassId, VkGuid& levelLayerId, uint renderPipelineId, String jsonPath, VkRenderPass renderPass, uint constBufferSize, ivec2& renderPassResolution)
{
    vulkanPipeline.RenderPipelineId = renderPipelineId;
    nlohmann::json json = Json::ReadJson(jsonPath);
    renderSystem.renderPipelineModelList[vulkanPipeline.RenderPipelineId] = RenderPipelineModel::from_json(json);

    GPUIncludes include =
    {
        .vertexProperties = renderSystem.GetVertexPropertiesBuffer(),
        .indexProperties = renderSystem.GetIndexPropertiesBuffer(),
        //        .transformProperties = renderSystem.GetTransformPropertiesBuffer(gpuImport.MeshList),
        .meshProperties = renderSystem.GetMeshPropertiesBuffer(levelLayerId),
        .texturePropertiesList = renderSystem.GetTexturePropertiesBuffer(renderPassId, renderSystem.InputTextureList[vulkanPipeline.RenderPipelineId]),
        .materialProperties = renderSystem.GetMaterialPropertiesBuffer()
    };
     
    Vector<VkPipelineShaderStageCreateInfo> pipelineShaderStageCreateInfoList = Vector<VkPipelineShaderStageCreateInfo>
    {
        shaderSystem.CreateShader(cRenderer.Device, renderSystem.renderPipelineModelList[vulkanPipeline.RenderPipelineId].VertexShaderPath, VK_SHADER_STAGE_VERTEX_BIT),
        shaderSystem.CreateShader(cRenderer.Device, renderSystem.renderPipelineModelList[vulkanPipeline.RenderPipelineId].FragmentShaderPath, VK_SHADER_STAGE_FRAGMENT_BIT)
    };

    vulkanPipeline.DescriptorPool = Pipeline_CreatePipelineDescriptorPool(*renderSystem.Device.get(), renderSystem.renderPipelineModelList[vulkanPipeline.RenderPipelineId], include);
    vulkanPipeline.DescriptorSetLayoutList = Pipeline_CreatePipelineDescriptorSetLayout(*renderSystem.Device.get(), renderSystem.renderPipelineModelList[vulkanPipeline.RenderPipelineId], include);
    vulkanPipeline.DescriptorSetList = Pipeline_AllocatePipelineDescriptorSets(*renderSystem.Device.get(), vulkanPipeline.DescriptorPool, renderSystem.renderPipelineModelList[vulkanPipeline.RenderPipelineId], vulkanPipeline.DescriptorSetLayoutList);
    Pipeline_UpdatePipelineDescriptorSets(*renderSystem.Device.get(), vulkanPipeline.DescriptorSetList, renderSystem.renderPipelineModelList[vulkanPipeline.RenderPipelineId], include);
    vulkanPipeline.PipelineLayout = Pipeline_CreatePipelineLayout(*renderSystem.Device.get(), vulkanPipeline.DescriptorSetLayoutList, constBufferSize);
    vulkanPipeline.Pipeline = Pipeline_CreatePipeline(*renderSystem.Device.get(), renderPass, vulkanPipeline.PipelineLayout, vulkanPipeline.PipelineCache, renderSystem.renderPipelineModelList[vulkanPipeline.RenderPipelineId], renderPassResolution, pipelineShaderStageCreateInfoList);
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
    renderer.DestroyPipeline(vulkanPipeline.Pipeline);
    renderer.DestroyPipelineLayout(vulkanPipeline.PipelineLayout);
    renderer.DestroyPipelineCache(vulkanPipeline.PipelineCache);
    renderer.DestroyDescriptorPool(vulkanPipeline.DescriptorPool);
    for (auto& descriptorSet : vulkanPipeline.DescriptorSetLayoutList)
    {
        renderer.DestroyDescriptorSetLayout(descriptorSet);
    }
}
