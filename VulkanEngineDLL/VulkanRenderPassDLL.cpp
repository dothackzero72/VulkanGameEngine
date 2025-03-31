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
	//RenderPassBuildInfoModel model = renderPassBuildInfo.Convert();
	//Vector<VkImageView> renderedColorTextureViews(renderedColorTextureList, renderedColorTextureList + renderedTextureCount);
	//Vector<VkImageView> swapChainImageViews(swapChainImageViewList, swapChainImageViewList + swapChainImageCount); 
	//SharedPtr<VkImageView> depthTexturePtr(depthTextureView);

	//Vector<VkFramebuffer> frameBufferList2(frameBufferCount, VK_NULL_HANDLE);
	//RenderPass_BuildFrameBuffer(device, renderPass, model, frameBufferList2, renderedColorTextureViews, depthTexturePtr, swapChainImageViews, renderPassResolution);
	//std::memcpy(frameBufferList, frameBufferList2.data(), frameBufferCount * sizeof(VkFramebuffer));
}

VkDescriptorPool DLL_Pipeline_CreateDescriptorPool(VkDevice device, RenderPipelineDLL renderPipelineModel, GPUIncludesDLL includePtr)
{
	RenderPipelineModel model = renderPipelineModel.Convert();
	GPUIncludes includes = includePtr.Convert();
	return Pipeline_CreateDescriptorPool(device, model, includes);
}

VkDescriptorSetLayout DLL_Pipeline_CreateDescriptorSetLayout(VkDevice device, RenderPipelineDLL renderPipelineDLL, GPUIncludesDLL includePtr)
{
	RenderPipelineModel model = renderPipelineDLL.Convert();
	GPUIncludes includes = includePtr.Convert();
	VkDescriptorSetLayout descriptorSetLayout = Pipeline_CreateDescriptorSetLayout(device, model, includes);
	
	return descriptorSetLayout;
}

VkDescriptorSet DLL_Pipeline_AllocateDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, VkDescriptorSetLayout descriptorSetLayout)
{
	VkDescriptorSet descriptorSet = Pipeline_AllocateDescriptorSets(device, descriptorPool, descriptorSetLayout);
	return descriptorSet;
}

void DLL_Pipeline_UpdateDescriptorSets(VkDevice device, VkDescriptorSet descriptorSet, RenderPipelineDLL renderPipelineDLL, GPUIncludesDLL includePtr)
{
	RenderPipelineModel model = renderPipelineDLL.Convert();
	GPUIncludes includes = includePtr.Convert();

	Pipeline_UpdateDescriptorSets( device, descriptorSet, model, includes);
}

VkPipelineLayout DLL_Pipeline_CreatePipelineLayout(VkDevice device, VkDescriptorSetLayout descriptorSetLayout, uint constBufferSize)
{
	return Pipeline_CreatePipelineLayout( device, descriptorSetLayout, constBufferSize);
}

VkPipeline DLL_Pipeline_CreatePipeline(VkDevice device, VkRenderPass renderpass, VkPipelineLayout pipelineLayout, VkPipelineCache pipelineCache, RenderPipelineDLL& modelDLL, VkVertexInputBindingDescription* vertexBindingList, VkVertexInputAttributeDescription* vertexAttributeList, uint vertexBindingCount, uint vertexAttributeCount)
{
	RenderPipelineModel model = modelDLL.Convert();
	Vector<VkVertexInputBindingDescription> vertexBindings(vertexBindingList, vertexBindingList + vertexBindingCount);
	Vector<VkVertexInputAttributeDescription> vertexAttributes(vertexAttributeList, vertexAttributeList + vertexAttributeCount);
	return Pipeline_CreatePipeline( device, renderpass, pipelineLayout, pipelineCache, model, vertexBindings, vertexAttributes);
}