#include "VulkanRenderPassDLL.h"

void DLL_RenderPass_BuildRenderPass(VkDevice device, VkRenderPass& renderPass, RenderPassBuildInfoModel renderPassBuildInfo, Vector<SharedPtr<RenderedTexture>>& renderedColorTextureList, SharedPtr<DepthTexture>& depthTexture)
{
	RenderPass_BuildRenderPass( device, renderPass, renderPassBuildInfo, renderedColorTextureList, depthTexture);
}

void DLL_RenderPass_BuildFrameBuffer(VkDevice device, VkRenderPass renderPass, RenderPassBuildInfoModel renderPassBuildInfo, VkFramebuffer* frameBufferList, RenderedTexture* renderedColorTextureList, DepthTexture depthTexture, VkImageView* swapChainImageViewList, uint frameBufferCount, uint renderedTextureCount, ivec2 renderPassResolution)
{
	Vector<VkFramebuffer> frameBuffers(frameBufferList, frameBufferList + renderedTextureCount);
	Vector<VkImageView> swapChainImageViews(swapChainImageViewList, swapChainImageViewList + renderedTextureCount);

	Vector<SharedPtr<RenderedTexture>> renderedTextures;
	for (int x = 0; x < renderedTextureCount; x++)
	{
		renderedTextures.emplace_back(std::make_shared<RenderedTexture>(renderedColorTextureList[x]));
	}

	SharedPtr<DepthTexture> depthTexturePtr = std::make_shared<DepthTexture>(depthTexture);

	RenderPass_BuildFrameBuffer( device,  renderPass, renderPassBuildInfo, frameBuffers, renderedTextures, depthTexturePtr, swapChainImageViews, renderPassResolution);
}

VkDescriptorPool DLL_Pipeline_CreateDescriptorPool(VkDevice device, RenderPipelineDLL renderPipelineModel, GPUIncludesDLL includePtr)
{
	RenderPipelineModel model = renderPipelineModel.Convert();
	GPUIncludes includes = includePtr.Convert();
	return Pipeline_CreateDescriptorPool(device, model, includes);
}

void DLL_Pipeline_CreateDescriptorSetLayout(VkDevice device, RenderPipelineDLL renderPipelineDLL, GPUIncludesDLL includePtr, VkDescriptorSetLayout* descriptorSetLayoutPtr, uint descriptorSetLayoutCount)
{
	RenderPipelineModel model = renderPipelineDLL.Convert();
	GPUIncludes includes = includePtr.Convert();

	Vector<VkDescriptorSetLayout> descriptorSetLayouts(descriptorSetLayoutCount);
	Pipeline_CreateDescriptorSetLayout(device, model, includes, descriptorSetLayouts);
	std::memcpy(descriptorSetLayoutPtr, descriptorSetLayouts.data(), descriptorSetLayoutCount * sizeof(VkDescriptorSetLayout));
}

void DLL_Pipeline_AllocateDescriptorSets(
	VkDevice device,
	VkDescriptorPool descriptorPool,
	VkDescriptorSetLayout* descriptorSetLayoutList,
	VkDescriptorSet* descriptorSetListPtr,
	uint descriptorSetLayoutCount)
{
	Vector<VkDescriptorSetLayout> descriptorLayoutSets(descriptorSetLayoutList, descriptorSetLayoutList + descriptorSetLayoutCount);
	Vector<VkDescriptorSet> descriptorSets = Pipeline_AllocateDescriptorSets(device, descriptorPool, descriptorLayoutSets);
	std::memcpy(descriptorSetListPtr, descriptorSets.data(), descriptorSetLayoutCount * sizeof(VkDescriptorSet));
}

void DLL_Pipeline_UpdateDescriptorSets(VkDevice device, VkDescriptorSet* descriptorSetList, RenderPipelineDLL renderPipelineDLL, GPUIncludesDLL includePtr, uint descriptorSetListCount)
{
	RenderPipelineModel model = renderPipelineDLL.Convert();
	GPUIncludes includes = includePtr.Convert();

	Vector<VkDescriptorSet> descriptorLayoutSets(descriptorSetList, descriptorSetList + descriptorSetListCount);
	Pipeline_UpdateDescriptorSets( device, descriptorLayoutSets, model, includes);
}

void DLL_Pipeline_CreatePipelineLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayoutList, uint constBufferSize, VkPipelineLayout& pipelineLayout, uint descriptorSetLayoutCount)
{
	Vector<VkDescriptorSetLayout> descriptorLayoutSets(descriptorSetLayoutList, descriptorSetLayoutList + descriptorSetLayoutCount);
	Pipeline_CreatePipelineLayout( device, descriptorLayoutSets, constBufferSize, pipelineLayout);
}

void DLL_Pipeline_CreatePipeline(VkDevice device, VkRenderPass renderpass, VkPipelineLayout pipelineLayout, VkPipelineCache pipelineCache, RenderPipelineDLL& modelDLL, VkVertexInputBindingDescription* vertexBindingList, VkVertexInputAttributeDescription* vertexAttributeList, VkPipeline& pipeline, uint vertexBindingCount, uint vertexAttributeCount)
{
	RenderPipelineModel model = modelDLL.Convert();
	Vector<VkVertexInputBindingDescription> vertexBindings(vertexBindingList, vertexBindingList + vertexBindingCount);
	Vector<VkVertexInputAttributeDescription> vertexAttributes(vertexAttributeList, vertexAttributeList + vertexAttributeCount);
	Pipeline_CreatePipeline( device, renderpass, pipelineLayout, pipelineCache, model, vertexBindings, vertexAttributes, pipeline);
}