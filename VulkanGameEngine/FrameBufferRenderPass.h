#pragma once
#include <Texture.h>
#include <ShaderCompiler.h>
#include "vertex.h"
#include "JsonPipeline.h"
#include "JsonRenderPass.h"

class FrameBufferRenderPass
{
private:
	 void BuildRenderPipelines(const RenderPassBuildInfoModel& renderPassBuildInfo, GPUImport& renderGraphics, SceneDataBuffer& sceneDataBuffer);
	 void BuildRenderPass(const RenderPassBuildInfoModel& renderPassBuildInfo);
	 void BuildFrameBuffer(const RenderPassBuildInfoModel& renderPassBuildInfo);

public:
	uint32 RenderPassId = 0;
	Vector<uint32> JsonPipelineList;

	ivec2 RenderPassResolution;
	VkSampleCountFlagBits SampleCount;

	VkRenderPass RenderPass = VK_NULL_HANDLE;
	std::vector<VkFramebuffer> FrameBufferList;
	Vector<VkClearValue> ClearValueList;
	VkRenderPassBeginInfo RenderPassInfo;

	FrameBufferRenderPass();
	FrameBufferRenderPass(uint renderPassIndex, const String& jsonPath, Texture& inputTexture, ivec2 renderPassResolution);
	 ~FrameBufferRenderPass();

	 void Update(const float& deltaTime);
	VkCommandBuffer DrawFrameBuffer();
	 VkCommandBuffer Draw(Vector<SharedPtr<GameObject>> meshList, SceneDataBuffer& sceneDataBuffer);
	 void Destroy();
};