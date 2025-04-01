#pragma once
#include "TextureDLL.h"
#include "DLLStructs.h"
#include <VulkanPipeline.h>
#include <DepthTexture.h>
#include <RenderedTexture.h>
#include <VulkanRenderPass.h>


extern "C"
{
	DLL_EXPORT VkRenderPass DLL_RenderPass_BuildRenderPass(VkDevice device, RenderPassBuildInfoDLL& renderPassBuildInfo);
	DLL_EXPORT VkFramebuffer* DLL_RenderPass_BuildFrameBuffer(VkDevice device, VkRenderPass renderPass, RenderPassBuildInfoDLL renderPassBuildInfo, VkImageView* renderedColorTextureList, VkImageView* depthTextureView, VkImageView* swapChainImageViewList, uint32_t swapChainImageCount, uint32_t renderedTextureCount, ivec2 renderPassResolution);
	DLL_EXPORT VkDescriptorPool DLL_Pipeline_CreateDescriptorPool(VkDevice device, RenderPipelineDLL& renderPipelineModel, GPUIncludesDLL& includePtr);
	DLL_EXPORT VkDescriptorSetLayout* DLL_Pipeline_CreateDescriptorSetLayout(VkDevice device, RenderPipelineDLL& renderPipelineDLL, GPUIncludesDLL& includePtr);
	DLL_EXPORT VkDescriptorSet* DLL_Pipeline_AllocateDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, RenderPipelineDLL& renderPipelineDLL, VkDescriptorSetLayout* descriptorSetLayouts);
	DLL_EXPORT void DLL_Pipeline_UpdateDescriptorSets(VkDevice device, RenderPipelineDLL& renderPipelineDLL, GPUIncludesDLL& includePtr, VkDescriptorSet* descriptorSetList);
	DLL_EXPORT VkPipelineLayout DLL_Pipeline_CreatePipelineLayout(VkDevice device, RenderPipelineDLL& renderPipelineDLL, uint constBufferSize, VkDescriptorSetLayout* descriptorSetLayout);
	DLL_EXPORT VkPipeline DLL_Pipeline_CreatePipeline(VkDevice device, VkRenderPass renderpass, VkPipelineLayout pipelineLayout, VkPipelineCache pipelineCache, RenderPipelineDLL& modelDLL, VkVertexInputBindingDescription* vertexBindingList, VkVertexInputAttributeDescription* vertexAttributeList, uint vertexBindingCount, uint vertexAttributeCount);
}

