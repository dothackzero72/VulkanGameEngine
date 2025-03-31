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
VkDescriptorSetLayout Pipeline_CreateDescriptorSetLayout(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes);
VkDescriptorSet Pipeline_AllocateDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, VkDescriptorSetLayout& descriptorSetLayoutList);
void Pipeline_UpdateDescriptorSets(VkDevice device, VkDescriptorSet& descriptorSet, const RenderPipelineModel& model, const GPUIncludes& includes);
VkPipelineLayout Pipeline_CreatePipelineLayout(VkDevice device, VkDescriptorSetLayout& descriptorSetLayout, uint constBufferSize);
VkPipeline Pipeline_CreatePipeline(VkDevice device, VkRenderPass renderpass, VkPipelineLayout pipelineLayout, VkPipelineCache pipelineCache, RenderPipelineModel& model, Vector<VkVertexInputBindingDescription>& vertexBindingList, Vector<VkVertexInputAttributeDescription>& vertexAttributeList);
