#pragma once

extern "C"
{
	#include "CShaderCompiler.h"
}

#include "JsonStructs.h"
#include "TypeDef.h"
#include "VulkanError.h"

struct GPUIncludes
{
	Vector<VkDescriptorBufferInfo> vertexProperties;
	Vector<VkDescriptorBufferInfo> indexProperties;
	Vector<VkDescriptorBufferInfo> transformProperties;
	Vector<VkDescriptorBufferInfo> meshProperties;
	Vector<VkDescriptorImageInfo>  texturePropertiesList;
	Vector<VkDescriptorBufferInfo> materialProperties;
};

DLL_EXPORT VkPipelineShaderStageCreateInfo Pipeline_CreateShader(VkDevice device, const String& filename, VkShaderStageFlagBits shaderStages);
DLL_EXPORT VkDescriptorPool Pipeline_CreateDescriptorPool(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes);
DLL_EXPORT Vector<VkDescriptorSetLayout> Pipeline_CreateDescriptorSetLayout(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes);
DLL_EXPORT Vector<VkDescriptorSet> Pipeline_AllocateDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, const RenderPipelineModel& model, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList);
DLL_EXPORT void Pipeline_UpdateDescriptorSets(VkDevice device, const Vector<VkDescriptorSet>& descriptorSetList, const RenderPipelineModel& model, const GPUIncludes& includes);
DLL_EXPORT VkPipelineLayout Pipeline_CreatePipelineLayout(VkDevice device, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList, uint constBufferSize);
DLL_EXPORT VkPipeline Pipeline_CreatePipeline(VkDevice device, VkRenderPass renderpass, VkPipelineLayout pipelineLayout, VkPipelineCache pipelineCache, const RenderPipelineModel& model, ivec2& extent);