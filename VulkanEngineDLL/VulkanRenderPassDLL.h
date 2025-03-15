#pragma once
#include "TextureDLL.h"
#include "DLLStructs.h"
#include <VulkanPipeline.h>
#include <DepthTexture.h>
#include <RenderedTexture.h>
#include <VulkanRenderPass.h>


extern "C"
{
	RenderPassBuildInfoModel RenderPass_RenderPassBuildInfoDLLConverter(const RenderPassBuildInfoDLL& renderPassBuildInfo);
	RenderPipelineModel Pipeline_RenderPipelineDLLConverter(const RenderPipelineDLL& renderPipeline);

	DLL_EXPORT void DLL_RenderPass_BuildRenderPass(VkDevice device, VkRenderPass& renderPass, RenderPassBuildInfoModel renderPassBuildInfo, Vector<SharedPtr<RenderedTexture>>& renderedColorTextureList, SharedPtr<DepthTexture>& depthTexture);
	DLL_EXPORT void DLL_RenderPass_BuildFrameBuffer(VkDevice device, VkRenderPass renderPass, const RenderPassBuildInfoModel& renderPassBuildInfo, Vector<SharedPtr<RenderedTexture>>& renderedColorTextureList, Vector<VkFramebuffer>& frameBufferList, SharedPtr<DepthTexture> depthTexture, ivec2 renderPassResolution);

	DLL_EXPORT VkDescriptorPool DLL_Pipeline_CreateDescriptorPool(VkDevice device, RenderPipelineDLL* renderPipelineModel, GPUIncludesDLL* includePtr);
	DLL_EXPORT void DLL_Pipeline_CreateDescriptorSetLayout(VkDevice device, RenderPipelineModel renderPipelineDLL, GPUIncludesDLL* includePtr, VkDescriptorSetLayout* descriptorSetLayoutList, uint descriptorSetLayoutCount);
	DLL_EXPORT VkDescriptorSet* DLL_Pipeline_AllocateDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList);
	DLL_EXPORT void DLL_Pipeline_UpdateDescriptorSets(VkDevice device, const Vector<VkDescriptorSet>& descriptorSetList, const RenderPipelineModel& renderPipelineDLL, const GPUIncludes& includes);
	DLL_EXPORT void DLL_Pipeline_CreatePipelineLayout(VkDevice device, const Vector<VkDescriptorSetLayout>& descriptorSetLayoutList, uint constBufferSize, VkPipelineLayout& pipelineLayout);
	DLL_EXPORT void DLL_Pipeline_CreatePipeline(VkDevice device,
		VkRenderPass renderpass,
		VkPipelineLayout pipelineLayout,
		VkPipelineCache pipelineCache,
		RenderPipelineModel& model,
		Vector<VkVertexInputBindingDescription>& vertexBindingList,
		Vector<VkVertexInputAttributeDescription>& vertexAttributeList,
		VkPipeline& pipeline);
}