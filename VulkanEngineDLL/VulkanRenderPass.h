#pragma once
#include "Typedef.h"
#include "CoreVulkanRenderer.h"
#include "VulkanPipeline.h"
#include "Texture.h"

struct VulkanRenderPass
{
	RenderPassGuid RenderPassId;
	VkSampleCountFlagBits SampleCount;
	VkRect2D RenderArea;
	VkRenderPass RenderPass = VK_NULL_HANDLE;
	Vector<VkFramebuffer> FrameBufferList;
	Vector<VkClearValue>  ClearValueList;
	VkCommandBuffer CommandBuffer = VK_NULL_HANDLE;
	bool UseFrameBufferResolution = true;
};

struct VulkanRenderPassCS
{
	RenderPassGuid RenderPassId;
	VkSampleCountFlagBits SampleCount;
	VkRect2D RenderArea;
	VkRenderPass RenderPass = VK_NULL_HANDLE;
	VkFramebuffer FrameBufferList;
	VkClearValue  ClearValueList;
	int FrameBufferCount;
	int ClearValueCount;
	VkCommandBuffer CommandBuffer = VK_NULL_HANDLE;
	bool UseFrameBufferResolution = true;
};

DLL_EXPORT VulkanRenderPass RenderPass_CreateVulkanRenderPass(const RendererState& renderState, RenderPassBuildInfoModel& model, ivec2& renderPassResolution, int ConstBuffer, Vector<Texture>& renderedTextureList, Texture& depthTexture);
DLL_EXPORT void RenderPass_DestroyRenderPass(RendererState& renderState, VulkanRenderPass& renderPass, Vector<Texture>& renderedTextureList);

DLL_EXPORT VulkanRenderPassCS RenderPass_ConvertToCS(void* renderState);

VkResult RenderPass_CreateCommandBuffers(const RendererState& renderState, VkCommandBuffer* commandBufferList, uint32 commandBufferCount);
VkResult RenderPass_CreateFrameBuffer(const RendererState& renderState, VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo);
VkRenderPass RenderPass_BuildRenderPass(const RendererState& renderState, VulkanRenderPass& vulkanRenderPass, const RenderPassBuildInfoModel& renderPassBuildInfo, Vector<Texture>& renderedTextureList, Texture& depthTexture);
Vector<VkFramebuffer> RenderPass_BuildFrameBuffer(const RendererState& renderState, const VulkanRenderPass& vulkanRenderPass, const RenderPassBuildInfoModel& renderPassBuildInfo, Vector<Texture>& renderedTextureList, Texture& depthTexture, ivec2& renderPassResolution);
