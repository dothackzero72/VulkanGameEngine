#pragma once
#include "CoreVulkanRenderer.h"
#include "VulkanPipeline.h"
#include "Texture.h"

struct VulkanRenderPass
{
	VkGuid RenderPassId;
	VkSampleCountFlagBits SampleCount;
	VkRect2D renderArea;
	VkRenderPass RenderPass = VK_NULL_HANDLE;
	Vector<VkFramebuffer> FrameBufferList;
	VkCommandBuffer CommandBuffer = VK_NULL_HANDLE;
	bool UseFrameBufferResolution = true;
};

//DLL_EXPORT VulkanRenderPass RenderPass_CreateVulkanRenderPass(VkDevice device, VkGuid& levelId, RenderPassBuildInfoModel& model, ivec2& renderPassResolution);
DLL_EXPORT void RenderPass_DestroyRenderPass(RendererState& rendererState, VulkanRenderPass& renderPass, Vector<Texture>& renderedTextureList);

DLL_EXPORT VkResult RenderPass_CreateCommandBuffers(VkDevice device, VkCommandPool commandPool, VkCommandBuffer* commandBufferList, uint32 commandBufferCount);
DLL_EXPORT VkResult RenderPass_CreateFrameBuffer(VkDevice device, VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo);
DLL_EXPORT VkRenderPass RenderPass_BuildRenderPass(VkDevice device, const RenderPassBuildInfoModel& renderPassBuildInfo);
DLL_EXPORT Vector<VkFramebuffer> RenderPass_BuildFrameBuffer(VkDevice device, VkRenderPass renderPass, const RenderPassBuildInfoModel& renderPassBuildInfo, Vector<VkImageView>& imageViewList, VkImageView* depthImageView, Vector<VkImageView>& swapChainImageViews, ivec2 renderPassResolution);

//#pragma once
//#include "CoreVulkanRenderer.h"
//#include "VulkanPipeline.h"
//#include "Texture.h"
//
//typedef VkGuid RenderPassGuid;
//
//struct VulkanRenderPass
//{
//	RenderPassGuid RenderPassId;
//	VkSampleCountFlagBits SampleCount;
//	VkRect2D renderArea;
//	VkRenderPass RenderPass = VK_NULL_HANDLE;
//	Vector<VkFramebuffer> FrameBufferList;
//	VkCommandBuffer CommandBuffer = VK_NULL_HANDLE;
//	UnorderedMap<RenderPassGuid, Vector<Texture>> RenderedTextureList;
//	UnorderedMap<RenderPassGuid, Vector<VulkanPipeline>> RenderPipelineList;
//	Texture DepthTexture;
//	bool UseFrameBufferResolution = true;
//};
//
//DLL_EXPORT VulkanRenderPass RenderPass_CreateVulkanRenderPass(RendererState& renderState, VkGuid& levelId, RenderPassBuildInfoModel& model, ivec2& renderPassResolution);
//DLL_EXPORT void RenderPass_DestroyRenderPass(RendererState& rendererState, VulkanRenderPass& renderPass, Vector<Texture>& renderedTextureList);
//
//DLL_EXPORT VkResult RenderPass_CreateCommandBuffers(VkDevice device, VkCommandPool commandPool, VkCommandBuffer* commandBufferList, uint32 commandBufferCount);
//DLL_EXPORT VkResult RenderPass_CreateFrameBuffer(VkDevice device, VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo);
//DLL_EXPORT VkRenderPass RenderPass_BuildRenderPass(RendererState& rendererState, VulkanRenderPass& vulkanRenderPass, const RenderPassBuildInfoModel& renderPassBuildInfo);
//DLL_EXPORT Vector<VkFramebuffer> RenderPass_BuildFrameBuffer(VkDevice device, VkRenderPass renderPass, const RenderPassBuildInfoModel& renderPassBuildInfo, const Vector<Texture>& swapChainTextures, const Vector<Texture>& textureList, const Texture* depthTexture, ivec2 renderPassResolution);
