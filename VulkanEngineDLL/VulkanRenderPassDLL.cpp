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

VkDescriptorSetLayout* DLL_Pipeline_CreateDescriptorSetLayout(VkDevice device, RenderPipelineDLL renderPipelineDLL, GPUIncludesDLL includePtr, uint32 descriptorSetLayoutCount)
{
	RenderPipelineModel model = renderPipelineDLL.Convert();
	GPUIncludes includes = includePtr.Convert();


	Vector<VkDescriptorSetLayout> descriptorSetLayoutList = Pipeline_CreateDescriptorSetLayout(device, model, includes, descriptorSetLayoutCount);

	VkDescriptorSetLayout* descriptorSetLayoutPtr = new VkDescriptorSetLayout[descriptorSetLayoutCount];
	std::memcpy(descriptorSetLayoutPtr, descriptorSetLayoutList.data(), descriptorSetLayoutList.size() * sizeof(VkDescriptorSetLayout));
	return descriptorSetLayoutPtr;
}

VkDescriptorSet* DLL_Pipeline_AllocateDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, VkDescriptorSetLayout* descriptorSetLayouts, uint32 descriptorSetLayoutCount, uint32 descriptorSetCount)
{
	Vector<VkDescriptorSetLayout> descriptorSetLayoutList = Vector<VkDescriptorSetLayout>(descriptorSetLayouts, descriptorSetLayouts + descriptorSetLayoutCount);
	Vector<VkDescriptorSet> descriptorSetList = Pipeline_AllocateDescriptorSets(device, descriptorPool, descriptorSetLayoutList, descriptorSetCount);

	VkDescriptorSet* descriptorSetPtr = new VkDescriptorSet[descriptorSetCount];
	std::memcpy(descriptorSetPtr, descriptorSetList.data(), descriptorSetList.size() * sizeof(VkDescriptorSetLayout));
	return descriptorSetPtr;
}

void DLL_Pipeline_UpdateDescriptorSets(VkDevice device, RenderPipelineDLL renderPipelineDLL, GPUIncludesDLL includePtr, VkDescriptorSet* descriptorSetList, uint32 descriptorSetCount)
{
	RenderPipelineModel model = renderPipelineDLL.Convert();
	GPUIncludes includes = includePtr.Convert();

	Vector<VkDescriptorSet> descriptorSetLayoutList = Vector<VkDescriptorSet>(descriptorSetList, descriptorSetList + descriptorSetCount);
	Pipeline_UpdateDescriptorSets( device, descriptorSetLayoutList, model, includes);
}

VkPipelineLayout DLL_Pipeline_CreatePipelineLayout(VkDevice device, uint constBufferSize, VkDescriptorSetLayout* descriptorSetLayout, uint32 descriptorSetLayoutCount)
{
	Vector<VkDescriptorSetLayout> descriptorSetLayoutList = Vector<VkDescriptorSetLayout>(descriptorSetLayout, descriptorSetLayout + descriptorSetLayoutCount);
	return Pipeline_CreatePipelineLayout( device, descriptorSetLayoutList, constBufferSize);
}

VkPipeline DLL_Pipeline_CreatePipeline(VkDevice device, VkRenderPass renderpass, VkPipelineLayout pipelineLayout, VkPipelineCache pipelineCache, RenderPipelineDLL& modelDLL, VkVertexInputBindingDescription* vertexBindingList, VkVertexInputAttributeDescription* vertexAttributeList, uint vertexBindingCount, uint vertexAttributeCount)
{
	RenderPipelineModel model = modelDLL.Convert();
	Vector<VkVertexInputBindingDescription> vertexBindings(vertexBindingList, vertexBindingList + vertexBindingCount);
	Vector<VkVertexInputAttributeDescription> vertexAttributes(vertexAttributeList, vertexAttributeList + vertexAttributeCount);
	return Pipeline_CreatePipeline( device, renderpass, pipelineLayout, pipelineCache, model, vertexBindings, vertexAttributes);
}

//void DLL_Pipeline_CreateDescriptorSetLayout(VkDevice device, RenderPipelineDLL renderPipelineDLL, GPUIncludesDLL includePtr, VkDescriptorSetLayout* descriptorSetLayoutPtr, uint descriptorSetLayoutCount)
//{
//	RenderPipelineModel model = renderPipelineDLL.Convert();
//	GPUIncludes includes = includePtr.Convert();
//
//	Vector<VkDescriptorSetLayout> descriptorSetLayouts(descriptorSetLayoutCount);
//	Pipeline_CreateDescriptorSetLayout(device, model, includes, descriptorSetLayouts);
//	std::memcpy(descriptorSetLayoutPtr, descriptorSetLayouts.data(), descriptorSetLayoutCount * sizeof(VkDescriptorSetLayout));
//}
//
//void DLL_Pipeline_AllocateDescriptorSets(
//	VkDevice device,
//	VkDescriptorPool descriptorPool,
//	VkDescriptorSetLayout* descriptorSetLayoutList,
//	VkDescriptorSet* descriptorSetListPtr,
//	uint descriptorSetLayoutCount)
//{
//	Vector<VkDescriptorSetLayout> descriptorLayoutSets(descriptorSetLayoutList, descriptorSetLayoutList + descriptorSetLayoutCount);
//	Vector<VkDescriptorSet> descriptorSets = Pipeline_AllocateDescriptorSets(device, descriptorPool, descriptorLayoutSets);
//	std::memcpy(descriptorSetListPtr, descriptorSets.data(), descriptorSetLayoutCount * sizeof(VkDescriptorSet));
//}
//
//void DLL_Pipeline_UpdateDescriptorSets(VkDevice device, VkDescriptorSet* descriptorSetList, RenderPipelineDLL renderPipelineDLL, GPUIncludesDLL includePtr, uint descriptorSetListCount)
//{
//	RenderPipelineModel model = renderPipelineDLL.Convert();
//	GPUIncludes includes = includePtr.Convert();
//
//	Vector<VkDescriptorSet> descriptorLayoutSets(descriptorSetList, descriptorSetList + descriptorSetListCount);
//	Pipeline_UpdateDescriptorSets(device, descriptorLayoutSets, model, includes);
//}
//
//void DLL_Pipeline_CreatePipelineLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayoutList, uint constBufferSize, VkPipelineLayout& pipelineLayout, uint descriptorSetLayoutCount)
//{
//	Vector<VkDescriptorSetLayout> descriptorLayoutSets(descriptorSetLayoutList, descriptorSetLayoutList + descriptorSetLayoutCount);
//	Pipeline_CreatePipelineLayout(device, descriptorLayoutSets, constBufferSize, pipelineLayout);
//}
//
//void DLL_Pipeline_CreatePipeline(VkDevice device, VkRenderPass renderpass, VkPipelineLayout pipelineLayout, VkPipelineCache pipelineCache, RenderPipelineDLL& modelDLL, VkVertexInputBindingDescription* vertexBindingList, VkVertexInputAttributeDescription* vertexAttributeList, VkPipeline& pipeline, uint vertexBindingCount, uint vertexAttributeCount)
//{
//	RenderPipelineModel model = modelDLL.Convert();
//	Vector<VkVertexInputBindingDescription> vertexBindings(vertexBindingList, vertexBindingList + vertexBindingCount);
//	Vector<VkVertexInputAttributeDescription> vertexAttributes(vertexAttributeList, vertexAttributeList + vertexAttributeCount);
//	Pipeline_CreatePipeline(device, renderpass, pipelineLayout, pipelineCache, model, vertexBindings, vertexAttributes, pipeline);
//}