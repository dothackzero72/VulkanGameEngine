#pragma once
#include "DepthTexture.h"
#include "RenderedTexture.h"
#include "JsonStructs.h"
#include "VulkanPipeline.h"
#include "CoreVulkanRenderer.h"

VkRenderPass RenderPass_BuildRenderPass(VkDevice device, const RenderPassBuildInfoModel& renderPassBuildInfo);
Vector<VkFramebuffer> RenderPass_BuildFrameBuffer(VkDevice device, VkRenderPass renderPass, const RenderPassBuildInfoModel& renderPassBuildInfo, Vector<VkImageView>& imageViewList, VkImageView* depthImageView, Vector<VkImageView>& swapChainImageViews, ivec2 renderPassResolution);
