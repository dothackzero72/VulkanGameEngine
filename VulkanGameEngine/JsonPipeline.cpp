#include "JsonPipeline.h"

JsonPipeline::JsonPipeline()
{
}

JsonPipeline::~JsonPipeline()
{
}

void JsonPipeline::CreateJsonPipeline(String JsonPath)
{
}

void JsonPipeline::Destroy()
{
    renderer.DestroyPipeline(Pipeline);
    renderer.DestroyPipelineLayout(PipelineLayout);
    renderer.DestroyPipelineCache(PipelineCache);
    renderer.DestroyDescriptorSetLayout(DescriptorSetLayout);
    renderer.DestroyDescriptorPool(DescriptorPool);
}
