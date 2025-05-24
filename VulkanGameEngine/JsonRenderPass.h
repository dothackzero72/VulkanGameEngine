//#pragma once
//#include <vulkan/vulkan_core.h>
//#include "Texture.h"
//#include "vertex.h"
//#include <VulkanRenderPass.h>
//#include <VulkanPipeline.h>
//#include "GameObject.h"
//#include "JsonStructs.h"
//#include "ECSid.h"
//#include "SceneDataBuffer.h"
//#include <Texture.h>
//
//class JsonRenderPass
//{
//protected:
//	void BuildRenderPass(const RenderPassBuildInfoModel& renderPassBuildInfo);
//	void BuildFrameBuffer(const RenderPassBuildInfoModel& renderPassBuildInfo);
//	void BuildCommandBuffer();
//
//	void RebuildFrameBuffer(const RenderPassBuildInfoModel& renderPassBuildInfo);
//public:
//	VkGuid RenderPassId;
//
//	VkSampleCountFlagBits SampleCount;
//	VkRect2D renderArea;
//	bool UseFrameBufferResolution = true;
//
//	VkRenderPass RenderPass = VK_NULL_HANDLE;
//	Vector<VkFramebuffer> FrameBufferList;
//	VkRenderPassBeginInfo RenderPassInfo;
//	VkCommandBuffer CommandBuffer = VK_NULL_HANDLE;
//
//	JsonRenderPass();
//	JsonRenderPass(VkGuid& levelId, RenderPassBuildInfoModel& model, ivec2& renderPassResolution);
//	JsonRenderPass(VkGuid& levelId, RenderPassBuildInfoModel& model, Texture& inputTexture, ivec2& renderPassResolution);
//	~JsonRenderPass();
//
//	void RecreateSwapchain(int newWidth, int newHeight);
//	void Destroy();
//};
