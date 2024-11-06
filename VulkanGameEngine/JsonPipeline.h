#pragma once
#include <vulkan/vulkan_core.h>
#include "Typedef.h"
#include "JsonRenderPass.h"

class JsonPipeline 
{
    friend class JsonRenderPass;
    friend class Mesh;

private:
    VkDescriptorPool DescriptorPool = VK_NULL_HANDLE;
    VkDescriptorSetLayout DescriptorSetLayout = VK_NULL_HANDLE;
    VkDescriptorSet DescriptorSet = VK_NULL_HANDLE;
    VkPipeline Pipeline = VK_NULL_HANDLE;
    VkPipelineLayout PipelineLayout = VK_NULL_HANDLE;
    VkPipelineCache PipelineCache = VK_NULL_HANDLE;

    //std::shared_ptr<JsonRenderPass> ParentRenderPass;

    JsonPipeline(String jsonPath);
public:
    String PipelineName;

    JsonPipeline();
    ~JsonPipeline();
    static std::shared_ptr<JsonPipeline> CreateJsonRenderPass(String jsonPath);
    void Destroy();
};