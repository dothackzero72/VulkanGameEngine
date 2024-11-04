#pragma once
#include <vulkan/vulkan_core.h>
#include "Typedef.h" // Ensure these files define necessary types
#include "JsonRenderPass.h" // Forward declaration may be used here if needed

class JsonPipeline {
    friend class JsonRenderPass; // Ensure both classes know about each other

private:
    VkDescriptorPool DescriptorPool = VK_NULL_HANDLE;
    VkDescriptorSetLayout DescriptorSetLayout = VK_NULL_HANDLE;
    VkDescriptorSet DescriptorSet = VK_NULL_HANDLE;
    VkPipeline Pipeline = VK_NULL_HANDLE;
    VkPipelineLayout PipelineLayout = VK_NULL_HANDLE;
    VkPipelineCache PipelineCache = VK_NULL_HANDLE;

public:
    JsonPipeline();
    ~JsonPipeline();

    void CreateJsonPipeline(String JsonPath);
    void Destroy();
};