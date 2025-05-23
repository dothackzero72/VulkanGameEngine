#pragma once
#include "CoreVulkanRenderer.h"
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
