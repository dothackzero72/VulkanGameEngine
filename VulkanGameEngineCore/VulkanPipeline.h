#include "../VulkanGameEngine/JsonStructs.h"
#include "CoreVulkanRenderer.h"
#include "TypeDef.h"
#include "JsonStructs.h"
#include "ShaderCompiler.h"

//void VkPipeline_CreateDescriptorPool(RenderPipelineModel& model, Vector<VkDescriptorBufferInfo> meshProperties, Vector<VkDescriptorImageInfo> textureList, Vector<VkDescriptorBufferInfo> materialProperties);
//void VkPipeline_CreateDescriptorSetLayout();
//void VkPipeline_AllocateDescriptorSets();
//void VkPipeline_UpdateDescriptorSets();
void VkPipeline_CreatePipelineLayout(VkDevice device, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList, uint constBufferSize, VkPipelineLayout& pipelineLayout);
void VkPipeline_CreatePipeline(VkDevice device,
	VkRenderPass renderpass,
	VkPipelineLayout pipelineLayout,
	VkPipelineCache pipelineCache,
	RenderPipelineModel& model,
	Vector<VkVertexInputBindingDescription>& vertexBindingList,
	Vector<VkVertexInputAttributeDescription>& vertexAttributeList,
	VkPipeline& pipeline);
