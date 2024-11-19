#pragma once
#include <vulkan/vulkan_core.h>
#include "Typedef.h"
#include "JsonRenderPass.h"

class JsonPipeline
{
    friend class JsonRenderPass;
    friend class Mesh;

private:


    std::shared_ptr<JsonRenderPass> ParentRenderPass;

    JsonPipeline(String jsonPath, VkRenderPass renderPass, uint constBufferSize);

    void LoadDescriptorSets(RenderPipelineModel model);
    void LoadPipeline(RenderPipelineModel model, VkRenderPass renderPass, uint ConstBufferSize);

public:
    String PipelineName;
    VkDescriptorPool DescriptorPool = VK_NULL_HANDLE;
    List<VkDescriptorSetLayout> DescriptorSetLayoutList;
    List<VkDescriptorSet> DescriptorSetList;
    VkPipeline Pipeline = VK_NULL_HANDLE;
    VkPipelineLayout PipelineLayout = VK_NULL_HANDLE;
    VkPipelineCache PipelineCache = VK_NULL_HANDLE;

    JsonPipeline();
    ~JsonPipeline();
    static std::shared_ptr<JsonPipeline> CreateJsonRenderPass(String jsonPath, VkRenderPass renderPass, uint constBufferSize);
    void Destroy();
};