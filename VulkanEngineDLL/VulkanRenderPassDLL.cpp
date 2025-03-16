#include "VulkanRenderPassDLL.h"

void DLL_RenderPass_BuildRenderPass(VkDevice device, VkRenderPass& renderPass, RenderPassBuildInfoModel renderPassBuildInfo, Vector<SharedPtr<RenderedTexture>>& renderedColorTextureList, SharedPtr<DepthTexture>& depthTexture)
{
	RenderPass_BuildRenderPass( device, renderPass, renderPassBuildInfo, renderedColorTextureList, depthTexture);
}

void DLL_RenderPass_BuildFrameBuffer(VkDevice device, VkRenderPass renderPass, const RenderPassBuildInfoModel& renderPassBuildInfo, Vector<SharedPtr<RenderedTexture>>& renderedColorTextureList, Vector<VkFramebuffer>& frameBufferList, SharedPtr<DepthTexture> depthTexture, ivec2 renderPassResolution)
{
	RenderPass_BuildFrameBuffer( device,  renderPass, renderPassBuildInfo, renderedColorTextureList, frameBufferList, depthTexture, renderPassResolution);
}

VkDescriptorPool DLL_Pipeline_CreateDescriptorPool(VkDevice device, RenderPipelineDLL renderPipelineModel, GPUIncludesDLL* includePtr)
{
	RenderPipelineModel model = renderPipelineModel.Convert();
	GPUIncludes includes = includePtr->Convert();
	return Pipeline_CreateDescriptorPool(device, model, includes);
}

void DLL_Pipeline_CreateDescriptorSetLayout(
	VkDevice device,
	RenderPipelineDLL renderPipelineDLL,
	GPUIncludesDLL* includePtr,
	VkDescriptorSetLayout* descriptorSetLayoutList,
	uint descriptorSetLayoutCount)
{
	RenderPipelineModel model = renderPipelineDLL.Convert();
	GPUIncludes includes = includePtr->Convert();

	Vector<VkDescriptorSetLayout> descriptorSetLayouts(descriptorSetLayoutCount);
	Pipeline_CreateDescriptorSetLayout(device, model, includes, descriptorSetLayouts);

	for (uint x = 0; x < descriptorSetLayoutCount; x++)
	{
		descriptorSetLayoutList[x] = descriptorSetLayouts[x];
	}
}

VkDescriptorSet* DLL_Pipeline_AllocateDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, VkDescriptorSetLayout* descriptorSetLayoutList, uint descriptorSetLayoutCount)
{
	Vector<VkDescriptorSetLayout> descriptorLayoutSets;
	for (int x = 0; x < descriptorSetLayoutCount; x++)
	{
		descriptorLayoutSets.emplace_back(descriptorSetLayoutList[x]);
	}

	Vector<VkDescriptorSet> descriptorSetList = Pipeline_AllocateDescriptorSets(device, descriptorPool, descriptorLayoutSets);

	VkDescriptorSet* descriptorSetPtr = new VkDescriptorSet[descriptorSetLayoutCount];
	for (uint x = 0; x < descriptorSetLayoutCount; x++)
	{
		descriptorSetPtr[x] = descriptorSetList[x];
	}
	return descriptorSetPtr;
}

void DLL_Pipeline_UpdateDescriptorSets(VkDevice device, VkDescriptorSet* descriptorSetList, RenderPipelineDLL renderPipelineDLL, GPUIncludesDLL* includePtr, uint descriptorSetListCount)
{
	RenderPipelineModel model = renderPipelineDLL.Convert();
	GPUIncludes includes = includePtr->Convert();
	Vector<VkDescriptorSet> descriptorLayoutSets;
	for (int x = 0; x < descriptorSetListCount; x++)
	{
		descriptorLayoutSets.emplace_back(descriptorSetList[x]);
	}

	Pipeline_UpdateDescriptorSets( device, descriptorLayoutSets, model, includes);
}

void DLL_Pipeline_CreatePipelineLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayoutList, uint constBufferSize, VkPipelineLayout& pipelineLayout, uint descriptorSetLayoutCount)
{
	Vector<VkDescriptorSetLayout> descriptorLayoutSets;
	for (int x = 0; x < descriptorSetLayoutCount; x++)
	{
		descriptorLayoutSets.emplace_back(descriptorSetLayoutList[x]);
	}

	Pipeline_CreatePipelineLayout( device, descriptorLayoutSets, constBufferSize, pipelineLayout);
}

void DLL_Pipeline_CreatePipeline(VkDevice device,
	VkRenderPass renderpass,
	VkPipelineLayout pipelineLayout,
	VkPipelineCache pipelineCache,
	RenderPipelineDLL& modelDLL,
	VkVertexInputBindingDescription* vertexBindingList,
	VkVertexInputAttributeDescription* vertexAttributeList,
	VkPipeline& pipeline,
	uint vertexBindingCount,
	uint vertexAttributeCount)
{
	RenderPipelineModel model = modelDLL.Convert();
	Vector<VkVertexInputBindingDescription> vertexBindings;
	for (int x = 0; x < vertexBindingCount; x++)
	{
		vertexBindings.emplace_back(vertexBindingList[x]);
	}

	Vector<VkVertexInputAttributeDescription> vertexAttributes;
	for (int x = 0; x < vertexAttributeCount; x++)
	{
		vertexAttributes.emplace_back(vertexAttributeList[x]);
	}
	Pipeline_CreatePipeline( device, renderpass, pipelineLayout, pipelineCache, model, vertexBindings, vertexAttributes, pipeline);
}