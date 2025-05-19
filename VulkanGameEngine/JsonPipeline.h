#pragma once
#include <vulkan/vulkan_core.h>
#include "json.h"
#include "Typedef.h"
#include "ECSid.h"
#include "VkGuid.h"
#include <JsonStructs.h>
#include <CoreVulkanRenderer.h>

class JsonPipeline
{
private:
public:
    uint RenderPipelineId;
    VkDescriptorPool DescriptorPool = VK_NULL_HANDLE;
    Vector<VkDescriptorSetLayout> DescriptorSetLayoutList;
    Vector<VkDescriptorSet> DescriptorSetList;
    VkPipeline Pipeline = VK_NULL_HANDLE;
    VkPipelineLayout PipelineLayout = VK_NULL_HANDLE;
    VkPipelineCache PipelineCache = VK_NULL_HANDLE;

    JsonPipeline();
    JsonPipeline(VkGuid& renderPassId, VkGuid& levelLayerId, uint renderPipelineId, String jsonPath, VkRenderPass renderPass, uint constBufferSize, ivec2& renderPassResolution);
    ~JsonPipeline();

    void RecreateSwapchain(VkRenderPass renderPass, uint constBufferSize, int newWidth, int newHeight);
    void Destroy();
};

VkDescriptorPool Pipeline_CreateDescriptorPool(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes);
Vector<VkDescriptorSetLayout> Pipeline_CreateDescriptorSetLayout(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes);
Vector<VkDescriptorSet> Pipeline_AllocateDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, const RenderPipelineModel& model, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList);
void Pipeline_UpdateDescriptorSets(VkDevice device, const Vector<VkDescriptorSet>& descriptorSetList, const RenderPipelineModel& model, const GPUIncludes& includes);
VkPipelineLayout Pipeline_CreatePipelineLayout(VkDevice device, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList, uint constBufferSize);
VkPipeline Pipeline_CreatePipeline(VkDevice device, VkRenderPass renderpass, VkPipelineLayout pipelineLayout, VkPipelineCache pipelineCache, const RenderPipelineModel& model, ivec2& extent);