#pragma once
#include "JsonStructs.h"
#include "CoreVulkanRenderer.h"
#include "TypeDef.h"
#include "JsonStructs.h"
#include "ShaderCompiler.h"
#include "Texture.h"

struct GPUIncludes
{
	Vector<VkDescriptorBufferInfo> vertexProperties;
	Vector<VkDescriptorBufferInfo> indexProperties;
	Vector<VkDescriptorBufferInfo> transformProperties;
	Vector<VkDescriptorBufferInfo> meshProperties;
	Vector<VkDescriptorImageInfo>  texturePropertiesList;
	Vector<VkDescriptorBufferInfo> materialProperties;
};

VkDescriptorPool Pipeline_CreateDescriptorPool(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes);
void Pipeline_CreateDescriptorSetLayout(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes, Vector<VkDescriptorSetLayout>& descriptorSetLayoutList);
Vector<VkDescriptorSet> Pipeline_AllocateDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList);
void Pipeline_UpdateDescriptorSets(VkDevice device, const Vector<VkDescriptorSet>& descriptorSetList, const RenderPipelineModel& model, const GPUIncludes& includes);
void Pipeline_CreatePipelineLayout(VkDevice device, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList, uint constBufferSize, VkPipelineLayout& pipelineLayout);
void Pipeline_CreatePipeline(VkDevice device,
	VkRenderPass renderpass,
	VkPipelineLayout pipelineLayout,
	VkPipelineCache pipelineCache,
	RenderPipelineModel& model,
	Vector<VkVertexInputBindingDescription>& vertexBindingList,
	Vector<VkVertexInputAttributeDescription>& vertexAttributeList,
	VkPipeline& pipeline);
