#include "../VulkanGameEngine/JsonStructs.h"
#include "CoreVulkanRenderer.h"
#include "TypeDef.h"
#include "JsonStructs.h"
#include "ShaderCompiler.h"

struct GPUIncludes
{
	Vector<VkDescriptorBufferInfo> vertexProperties;
	Vector<VkDescriptorBufferInfo> indexProperties;
	Vector<VkDescriptorBufferInfo> transformProperties;
	Vector<VkDescriptorBufferInfo> meshProperties;
	Vector<VkDescriptorImageInfo>  texturePropertiesList;
	Vector<VkDescriptorBufferInfo> materialProperties;
};

VkDescriptorPool VkPipeline_CreateDescriptorPool(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes);
Vector<VkDescriptorSetLayout> VkPipeline_CreateDescriptorSetLayout(VkDevice device, const RenderPipelineModel& model, const GPUIncludes& includes);
Vector<VkDescriptorSet> VkPipeline_AllocateDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList);
void VkPipeline_UpdateDescriptorSets(VkDevice device, const Vector<VkDescriptorSet>& descriptorSetList, const RenderPipelineModel& model, const GPUIncludes& includes);
void VkPipeline_CreatePipelineLayout(VkDevice device, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList, uint constBufferSize, VkPipelineLayout& pipelineLayout);
void VkPipeline_CreatePipeline(VkDevice device,
	VkRenderPass renderpass,
	VkPipelineLayout pipelineLayout,
	VkPipelineCache pipelineCache,
	RenderPipelineModel& model,
	Vector<VkVertexInputBindingDescription>& vertexBindingList,
	Vector<VkVertexInputAttributeDescription>& vertexAttributeList,
	VkPipeline& pipeline);
