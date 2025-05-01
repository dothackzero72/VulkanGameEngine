#pragma once
#include <vulkan/vulkan_core.h>
#include <Texture.h>
#include <ShaderCompiler.h>
#include "vertex.h"
#include "JsonPipeline.h"
#include "GameObject.h"
#include "JsonStructs.h"
#include "ECGid.h"
#include "SceneDataBuffer.h"

class JsonRenderPass
{
protected:
	void BuildRenderPipelines(const RenderPassBuildInfoModel& renderPassBuildInfo, SceneDataBuffer& sceneDataBuffer);
	void BuildRenderPass(const RenderPassBuildInfoModel& renderPassBuildInfo);
	void BuildFrameBuffer(const RenderPassBuildInfoModel& renderPassBuildInfo);

public:
	RenderPassID RenderPassId;

	ivec2 RenderPassResolution;
	VkSampleCountFlagBits SampleCount;
	VkRect2D renderArea;
	VkRenderPass RenderPass = VK_NULL_HANDLE;
	std::vector<VkFramebuffer> FrameBufferList;
	VkRenderPassBeginInfo RenderPassInfo;
	VkCommandBuffer CommandBuffer = VK_NULL_HANDLE;

	void CreateCommandBuffer();
	JsonRenderPass();
	JsonRenderPass(RenderPassID renderPassIndex, const String& jsonPath, ivec2& renderPassResolution);
	JsonRenderPass(RenderPassID renderPassIndex, const String& jsonPath, Texture& inputTexture, ivec2& renderPassResolution);
	~JsonRenderPass();

	void Update(const float& deltaTime);
	VkCommandBuffer Draw(Vector<SharedPtr<GameObject>> meshList, SceneDataBuffer& sceneDataBuffer);
	void Destroy();
};
