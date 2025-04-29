#pragma once
#include <Texture.h>
#include <ShaderCompiler.h>
#include "vertex.h"
#include "JsonPipeline.h"

class JsonRenderPass
{
private:
	 void BuildRenderPipelines(const RenderPassBuildInfoModel& renderPassBuildInfo, GPUImport& renderGraphics, SceneDataBuffer& sceneDataBuffer);
	 void BuildRenderPass(const RenderPassBuildInfoModel& renderPassBuildInfo);
	 void BuildFrameBuffer(const RenderPassBuildInfoModel& renderPassBuildInfo);

public:
	uint32 RenderPassId = 0;

	ivec2 RenderPassResolution;
	VkSampleCountFlagBits SampleCount;

	VkRenderPass RenderPass = VK_NULL_HANDLE;
	std::vector<VkFramebuffer> FrameBufferList;
	Vector<VkClearValue> ClearValueList;
	VkRenderPassBeginInfo RenderPassInfo;

	JsonRenderPass();
	JsonRenderPass(uint renderPassIndex, const String& jsonPath, Texture& inputTexture, ivec2 renderPassResolution);
	 ~JsonRenderPass();

	 void Update(const float& deltaTime);
	VkCommandBuffer DrawFrameBuffer();
	 VkCommandBuffer Draw(Vector<SharedPtr<GameObject>> meshList, SceneDataBuffer& sceneDataBuffer);
	 void Destroy();
};