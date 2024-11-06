#include "JsonPipeline.h"
#include "MemoryManager.h"

JsonPipeline::JsonPipeline()
{
}

JsonPipeline::JsonPipeline(String jsonPath)
{
    //ParentRenderPass = parentRenderPass;

}

JsonPipeline::~JsonPipeline()
{
}

std::shared_ptr<JsonPipeline> JsonPipeline::CreateJsonRenderPass(String jsonPath)
{
    std::shared_ptr<JsonPipeline> pipeline = MemoryManager::AllocateJsonPipeline();
    new (pipeline.get()) JsonPipeline(jsonPath);
    return pipeline;
}

void JsonPipeline::Destroy()
{
    renderer.DestroyPipeline(Pipeline);
    renderer.DestroyPipelineLayout(PipelineLayout);
    renderer.DestroyPipelineCache(PipelineCache);
    renderer.DestroyDescriptorSetLayout(DescriptorSetLayout);
    renderer.DestroyDescriptorPool(DescriptorPool);
}
