#include "JsonPipeline.h"
#include "MemoryManager.h"

JsonPipeline::JsonPipeline()
{
}

JsonPipeline::JsonPipeline(String jsonPath, VkRenderPass renderPass, uint constBufferSize)
{
  //  ParentRenderPass = parentRenderPass;
    nlohmann::json json = Json::ReadJson("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\\RenderPass\\DefaultRenderPass.json");
   // RenderPipelineModel renderPipelineModel = RenderPipelineModel::from_json(json);
  //  LoadDescriptorSets(renderPipelineModel);
  //  LoadPipeline(renderPipelineModel, renderPass, constBufferSize);
}

JsonPipeline::~JsonPipeline()
{
}

std::shared_ptr<JsonPipeline> JsonPipeline::CreateJsonRenderPass(String jsonPath, VkRenderPass renderPass, uint constBufferSize)
{
    std::shared_ptr<JsonPipeline> pipeline = MemoryManager::AllocateJsonPipeline();
    new (pipeline.get()) JsonPipeline(jsonPath, renderPass, constBufferSize);
    return pipeline;
}

//void JsonPipeline::LoadDescriptorSets(RenderPipelineModel model)
//{
//}
//
//void JsonPipeline::LoadPipeline(RenderPipelineModel model, VkRenderPass renderPass, uint constBufferSize)
//{
//}

void JsonPipeline::Destroy()
{
    renderer.DestroyPipeline(Pipeline);
    renderer.DestroyPipelineLayout(PipelineLayout);
    renderer.DestroyPipelineCache(PipelineCache);
    renderer.DestroyDescriptorSetLayout(DescriptorSetLayout);
    renderer.DestroyDescriptorPool(DescriptorPool);
}
