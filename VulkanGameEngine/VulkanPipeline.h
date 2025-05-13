#pragma once
#include "JsonStructs.h"
#include "CoreVulkanRenderer.h"
#include "TypeDef.h"
#include "JsonStructs.h"
#include "Texture.h"

struct GPUIncludes
{
	Vector<VkDescriptorBufferInfo> vertexProperties;
	Vector<VkDescriptorBufferInfo> indexProperties;
	Vector<VkDescriptorBufferInfo> transformProperties;
	Vector<VkDescriptorBufferInfo> meshProperties;
	Vector<VkDescriptorBufferInfo> LevelLayermeshProperties;
	Vector<VkDescriptorImageInfo>  texturePropertiesList;
	Vector<VkDescriptorBufferInfo> materialProperties;
};

VkDescriptorPool Pipeline_CreateDescriptorPool(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes);
Vector<VkDescriptorSetLayout> Pipeline_CreateDescriptorSetLayout(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes);
Vector<VkDescriptorSet> Pipeline_AllocateDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, const RenderPipelineModel& model, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList);
void Pipeline_UpdateDescriptorSets(VkDevice device, const Vector<VkDescriptorSet>& descriptorSetList, const RenderPipelineModel& model, const GPUIncludes& includes);
VkPipelineLayout Pipeline_CreatePipelineLayout(VkDevice device, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList, uint constBufferSize);
VkPipeline Pipeline_CreatePipeline(VkDevice device, VkRenderPass renderpass, VkPipelineLayout pipelineLayout, VkPipelineCache pipelineCache, const RenderPipelineModel& model, ivec2& extent);