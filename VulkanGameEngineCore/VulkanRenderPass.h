#pragma once
#include "DepthTexture.h"
#include "RenderedTexture.h"
#include "JsonStructs.h"
#include "VulkanPipeline.h"
#include "CoreVulkanRenderer.h"

void RenderPass_BuildRenderPass(VkDevice device, VkRenderPass& renderPass, const RenderPassBuildInfoModel& renderPassBuildInfo, Vector<SharedPtr<RenderedTexture>>& renderedColorTextureList, SharedPtr<DepthTexture>& depthTexture);
void RenderPass_BuildFrameBuffer(VkDevice device, VkRenderPass renderPass, const RenderPassBuildInfoModel& renderPassBuildInfo, Vector<SharedPtr<RenderedTexture>>& renderedColorTextureList, Vector<VkFramebuffer>& frameBufferList, SharedPtr<DepthTexture> depthTexture, ivec2 renderPassResolution);
