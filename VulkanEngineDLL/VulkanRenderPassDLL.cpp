#include "VulkanRenderPassDLL.h"

void DLL_RenderPass_BuildRenderPass(VkDevice device, VkRenderPass& renderPass, RenderPassBuildInfoModel renderPassBuildInfo, Vector<SharedPtr<RenderedTexture>>& renderedColorTextureList, SharedPtr<DepthTexture>& depthTexture)
{
	RenderPass_BuildRenderPass( device, renderPass, renderPassBuildInfo, renderedColorTextureList, depthTexture);
}

void __stdcall DLL_RenderPass_BuildFrameBuffer(
	VkDevice device,
	VkRenderPass renderPass,
	RenderPassBuildInfoDLL renderPassBuildInfo,
	VkFramebuffer* frameBufferList,
	VkImageView* renderedColorTextureList,
	VkImageView* depthTextureView,
	VkImageView* swapChainImageViewList,
	uint32_t frameBufferCount,
	uint32_t swapChainImageCount,
	uint32_t renderedTextureCount,
	ivec2 renderPassResolution)
{
	RenderPassBuildInfoModel model = renderPassBuildInfo.Convert();
	Vector<VkImageView> renderedColorTextureViews(renderedColorTextureList, renderedColorTextureList + renderedTextureCount);
	Vector<VkImageView> swapChainImageViews(swapChainImageViewList, swapChainImageViewList + swapChainImageCount); 
	SharedPtr<VkImageView> depthTexturePtr(depthTextureView);

	Vector<VkFramebuffer> frameBufferList2(frameBufferCount, VK_NULL_HANDLE);
	RenderPass_BuildFrameBuffer(device, renderPass, model, frameBufferList2, renderedColorTextureViews, depthTexturePtr, swapChainImageViews, renderPassResolution);
	std::memcpy(frameBufferList, frameBufferList2.data(), frameBufferCount * sizeof(VkFramebuffer));
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