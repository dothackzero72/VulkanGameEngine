#include "VulkanRenderPassDLL.h"

RenderPassBuildInfoModel RenderPass_RenderPassBuildInfoDLLConverter(const RenderPassBuildInfoDLL& renderPassBuildInfo)
{
	RenderPassBuildInfoModel model;
	//for (int x = 0; x < renderPassBuildInfo.RenderPipelineListCount; x++)
	//{
	//	model.RenderPipelineList.emplace_back(renderPassBuildInfo.RenderPipelineList[x]);
	//}
	//for (int x = 0; x < renderPassBuildInfo.RenderedTextureInfoDLLListCount; x++)
	//{
	//	model.RenderedTextureInfoModelList.emplace_back(renderPassBuildInfo.RenderedTextureInfoDLLList[x]);
	//}
	//for (int x = 0; x < renderPassBuildInfo.SubpassDependencyListCount; x++)
	//{
	//	model.SubpassDependencyModelList.emplace_back(renderPassBuildInfo.SubpassDependencyList[x]);
	//}
	return model;
}

RenderPipelineModel Pipeline_RenderPipelineDLLConverter(const RenderPipelineModel& renderPipeline)
{
	RenderPipelineModel model;
	return model;
}

void DLL_RenderPass_BuildRenderPass(VkDevice device, VkRenderPass& renderPass, RenderPassBuildInfoModel renderPassBuildInfo, Vector<SharedPtr<RenderedTexture>>& renderedColorTextureList, SharedPtr<DepthTexture>& depthTexture)
{
	RenderPass_BuildRenderPass( device, renderPass, renderPassBuildInfo, renderedColorTextureList, depthTexture);
}

void DLL_RenderPass_BuildFrameBuffer(VkDevice device, VkRenderPass renderPass, const RenderPassBuildInfoModel& renderPassBuildInfo, Vector<SharedPtr<RenderedTexture>>& renderedColorTextureList, Vector<VkFramebuffer>& frameBufferList, SharedPtr<DepthTexture> depthTexture, ivec2 renderPassResolution)
{
	RenderPass_BuildFrameBuffer( device,  renderPass, renderPassBuildInfo, renderedColorTextureList, frameBufferList, depthTexture, renderPassResolution);
}

VkDescriptorPool DLL_Pipeline_CreateDescriptorPool(VkDevice device, RenderPipelineDLL* renderPipelineModel, GPUIncludesDLL* includePtr)
{
	RenderPipelineModel model = renderPipelineModel->Convert();
	GPUIncludes includes = includePtr->Convert();
	return Pipeline_CreateDescriptorPool(device, model, includes);
}

void DLL_Pipeline_CreateDescriptorSetLayout(VkDevice device, RenderPipelineModel renderPipelineDLL, GPUIncludesDLL* includePtr, VkDescriptorSetLayout* descriptorSetLayoutList, uint descriptorSetLayoutCount)
{
	GPUIncludes includes = includePtr->Convert();
	Vector<VkDescriptorSetLayout> descriptorSetLayouts;
	for (int x = 0; x < descriptorSetLayoutCount; x++)
	{
		descriptorSetLayouts.emplace_back(descriptorSetLayoutList[x]);
	}

	RenderPipelineModel renderPassInfo = Pipeline_RenderPipelineDLLConverter(renderPipelineDLL);
	Pipeline_CreateDescriptorSetLayout( device, renderPassInfo, includes, descriptorSetLayouts);
}

VkDescriptorSet* DLL_Pipeline_AllocateDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList)
{
	Pipeline_AllocateDescriptorSets(device, descriptorPool, descriptorSetLayoutList);

	return nullptr;
}

void DLL_Pipeline_UpdateDescriptorSets(VkDevice device, const Vector<VkDescriptorSet>& descriptorSetList, const RenderPipelineModel& renderPipelineDLL, const GPUIncludes& includes)
{
	RenderPipelineModel renderPassInfo = Pipeline_RenderPipelineDLLConverter(renderPipelineDLL);
	Pipeline_UpdateDescriptorSets( device, descriptorSetList, renderPassInfo, includes);
}

void DLL_Pipeline_CreatePipelineLayout(VkDevice device, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList, uint constBufferSize, VkPipelineLayout& pipelineLayout)
{
	Pipeline_CreatePipelineLayout( device, descriptorSetLayoutList, constBufferSize, pipelineLayout);
}

void DLL_Pipeline_CreatePipeline(VkDevice device,
	VkRenderPass renderpass,
	VkPipelineLayout pipelineLayout,
	VkPipelineCache pipelineCache,
	RenderPipelineModel& model,
	Vector<VkVertexInputBindingDescription>& vertexBindingList,
	Vector<VkVertexInputAttributeDescription>& vertexAttributeList,
	VkPipeline& pipeline)
{
	Pipeline_CreatePipeline( device, renderpass, pipelineLayout, pipelineCache, model, vertexBindingList, vertexAttributeList, pipeline);
}