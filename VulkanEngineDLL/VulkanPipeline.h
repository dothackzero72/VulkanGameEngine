#pragma once
#include "CoreVulkanRenderer.h"

struct VulkanPipeline
{
    uint RenderPipelineId;
    size_t DescriptorSetLayoutCount;
    size_t DescriptorSetCount;
    VkDescriptorPool DescriptorPool = VK_NULL_HANDLE;
    VkDescriptorSetLayout* DescriptorSetLayoutList = nullptr;
    VkDescriptorSet* DescriptorSetList = nullptr;
    VkPipeline Pipeline = VK_NULL_HANDLE;
    VkPipelineLayout PipelineLayout = VK_NULL_HANDLE;
    VkPipelineCache PipelineCache = VK_NULL_HANDLE;
};

DLL_EXPORT VulkanPipeline VulkanPipeline_CreateRenderPipeline(VkDevice device, VkGuid& renderPassId, uint renderPipelineId, RenderPipelineModel& model, VkRenderPass renderPass, size_t constBufferSize, ivec2& renderPassResolution, const GPUIncludes& includes, VkPipelineShaderStageCreateInfo& pipelineShaderList, size_t pipelineShaderCount);
DLL_EXPORT void VulkanPipeline_RecreateSwapchain(VkRenderPass renderPass, uint constBufferSize, int newWidth, int newHeight);
DLL_EXPORT void VulkanPipeline_Destroy(VkDevice device, VulkanPipeline& vulkanPipelineDLL);

VkDescriptorPool Pipeline_CreatePipelineDescriptorPool(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes);
Vector<VkDescriptorSetLayout> Pipeline_CreatePipelineDescriptorSetLayout(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes);
Vector<VkDescriptorSet> Pipeline_AllocatePipelineDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, const RenderPipelineModel& model, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList);
void Pipeline_UpdatePipelineDescriptorSets(VkDevice device, const Vector<VkDescriptorSet>& descriptorSetList, const RenderPipelineModel& model, const GPUIncludes& includes);
VkPipelineLayout Pipeline_CreatePipelineLayout(VkDevice device, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList, uint constBufferSize);
VkPipeline Pipeline_CreatePipeline(VkDevice device, VkRenderPass renderpass, VkPipelineLayout pipelineLayout, VkPipelineCache pipelineCache, const RenderPipelineModel& model, ivec2& extent, Vector<VkPipelineShaderStageCreateInfo>& pipelineShaders);