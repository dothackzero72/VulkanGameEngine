#pragma once
#include "CoreVulkanRenderer.h"
#include "VulkanPipeline.h"
#include "Texture.h"

<<<<<<< Updated upstream
typedef VkGuid RenderPassGuid;

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
DLL_EXPORT VulkanRenderPassCS RenderPass_ConvertToCS(void* renderState);
DLL_EXPORT void RenderPass_DestroyRenderPass(RendererState& renderState, VulkanRenderPass& renderPass, Vector<Texture>& renderedTextureList);

VkResult RenderPass_CreateCommandBuffers(const RendererState& renderState, VkCommandBuffer* commandBufferList, uint32 commandBufferCount);
VkResult RenderPass_CreateFrameBuffer(const RendererState& renderState, VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo);
VkRenderPass RenderPass_BuildRenderPass(const RendererState& renderState, VulkanRenderPass& vulkanRenderPass, const RenderPassBuildInfoModel& renderPassBuildInfo, Vector<Texture>& renderedTextureList, Texture& depthTexture);
Vector<VkFramebuffer> RenderPass_BuildFrameBuffer(const RendererState& renderState, const VulkanRenderPass& vulkanRenderPass, const RenderPassBuildInfoModel& renderPassBuildInfo, Vector<Texture>& renderedTextureList, Texture& depthTexture, ivec2& renderPassResolution);
=======
struct VulkanRenderPass
{
	RenderPassGuid RenderPassId;
	VkSampleCountFlagBits SampleCount;
	VkRect2D RenderArea;
	VkRenderPass RenderPass = VK_NULL_HANDLE;
	VkFramebuffer* FrameBufferList;
	VkClearValue* ClearValueList;
	size_t FrameBufferCount;
	size_t ClearValueCount;
	VkCommandBuffer CommandBuffer = VK_NULL_HANDLE;
	bool UseFrameBufferResolution = true;
};

#ifdef __cplusplus
extern "C" {
#endif
//DLL_EXPORT VulkanRenderPassDLL* VulkanRenderPass_ConvertToVulkanRenderPassDLL(VulkanRenderPass& renderPass);
//DLL_EXPORT VulkanRenderPass VulkanRenderPass_ConvertToVulkanRenderPass(VulkanRenderPassDLL* renderPassDLL);
DLL_EXPORT VulkanRenderPass VulkanRenderPass_CreateVulkanRenderPass(RendererStateDLL& renderState, const char* renderPassLoader, ivec2& renderPassResolution, int ConstBuffer, Texture& renderedTextureListPtr, size_t& renderedTextureCount, Texture& depthTexture);
DLL_EXPORT void VulkanRenderPass_DestroyRenderPass(RendererStateDLL& renderStateDLL, VulkanRenderPass& renderPass);
//DLL_EXPORT void VulkanRenderPass_DeleteVulkanRenderPassDLLPtrs(VulkanRenderPassDLL* vulkanRenderPassDLL);

VkResult RenderPass_CreateCommandBuffers(const RendererState& renderState, VkCommandBuffer* commandBufferList, size_t commandBufferCount);
VkRenderPass RenderPass_BuildRenderPass(const RendererState& renderState, const RenderPassLoader& renderPassBuildInfo, Vector<Texture>& renderedTextureList, Texture& depthTexture);
#ifdef __cplusplus
}
#endif
Vector<VkFramebuffer> RenderPass_BuildFrameBuffer(const RendererState& renderState, const VkRenderPass& renderPass, const RenderPassLoader& renderPassJsonLoader, Vector<Texture>& renderedTextureList, Texture& depthTexture, ivec2& renderPassResolution);
>>>>>>> Stashed changes
