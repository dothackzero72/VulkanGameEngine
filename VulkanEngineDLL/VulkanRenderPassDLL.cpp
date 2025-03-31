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

VkDescriptorPool DLL_Pipeline_CreateDescriptorPool(VkDevice device, RenderPipelineDLL& renderPipelineModel, GPUIncludesDLL& includePtr)
{
	RenderPipelineModel model = renderPipelineModel.Convert();
	GPUIncludes includes = includePtr.Convert();
	return Pipeline_CreateDescriptorPool(device, model, includes);
}

VkDescriptorSetLayout* DLL_Pipeline_CreateDescriptorSetLayout(VkDevice device, RenderPipelineDLL& renderPipelineDLL, GPUIncludesDLL& includePtr)
{
	RenderPipelineModel model = renderPipelineDLL.Convert();
	GPUIncludes includes = includePtr.Convert();

	Vector<VkDescriptorSetLayout> descriptorSetLayoutList = Pipeline_CreateDescriptorSetLayout(device, model, includes);

	VkDescriptorSetLayout* descriptorSetLayoutPtr = new VkDescriptorSetLayout[model.DescriptorSetCount];
	std::memcpy(descriptorSetLayoutPtr, descriptorSetLayoutList.data(), descriptorSetLayoutList.size() * sizeof(VkDescriptorSetLayout));
	return descriptorSetLayoutPtr;
}

VkDescriptorSet* DLL_Pipeline_AllocateDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, RenderPipelineDLL& renderPipelineDLL, VkDescriptorSetLayout* descriptorSetLayouts)
{
	RenderPipelineModel model = renderPipelineDLL.Convert();
	Vector<VkDescriptorSetLayout> descriptorSetLayoutList = Vector<VkDescriptorSetLayout>(descriptorSetLayouts, descriptorSetLayouts + model.DescriptorSetLayoutCount);
	Vector<VkDescriptorSet> descriptorSetList = Pipeline_AllocateDescriptorSets(device, descriptorPool, model, descriptorSetLayoutList);

	VkDescriptorSet* descriptorSetPtr = new VkDescriptorSet[model.DescriptorSetCount];
	std::memcpy(descriptorSetPtr, descriptorSetList.data(), descriptorSetList.size() * sizeof(VkDescriptorSetLayout));
	return descriptorSetPtr;
}

void DLL_Pipeline_UpdateDescriptorSets(VkDevice device, RenderPipelineDLL& renderPipelineDLL, GPUIncludesDLL& includePtr, VkDescriptorSet* descriptorSetList)
{
	RenderPipelineModel model = renderPipelineDLL.Convert();
	GPUIncludes includes = includePtr.Convert();

	Vector<VkDescriptorSet> descriptorSetLayoutList = Vector<VkDescriptorSet>(descriptorSetList, descriptorSetList + model.DescriptorSetCount);
	Pipeline_UpdateDescriptorSets( device, descriptorSetLayoutList, model, includes);
}

VkPipelineLayout DLL_Pipeline_CreatePipelineLayout(VkDevice device, RenderPipelineDLL& renderPipelineDLL, uint constBufferSize, VkDescriptorSetLayout* descriptorSetLayout)
{
	RenderPipelineModel model = renderPipelineDLL.Convert();
	Vector<VkDescriptorSetLayout> descriptorSetLayoutList = Vector<VkDescriptorSetLayout>(descriptorSetLayout, descriptorSetLayout + model.DescriptorSetLayoutCount);
	return Pipeline_CreatePipelineLayout( device, descriptorSetLayoutList, constBufferSize);
}

VkPipeline DLL_Pipeline_CreatePipeline(VkDevice device, VkRenderPass renderpass, VkPipelineLayout pipelineLayout, VkPipelineCache pipelineCache, RenderPipelineDLL& modelDLL, VkVertexInputBindingDescription* vertexBindingList, VkVertexInputAttributeDescription* vertexAttributeList, uint vertexBindingCount, uint vertexAttributeCount)
{
	RenderPipelineModel model = modelDLL.Convert();
	Vector<VkVertexInputBindingDescription> vertexBindings(vertexBindingList, vertexBindingList + vertexBindingCount);
	Vector<VkVertexInputAttributeDescription> vertexAttributes(vertexAttributeList, vertexAttributeList + vertexAttributeCount);
	return Pipeline_CreatePipeline( device, renderpass, pipelineLayout, pipelineCache, model, vertexBindings, vertexAttributes);
}