#pragma once
#include <vulkan/vulkan_core.h>
#include "json.h"
#include "Typedef.h"
#include "JsonStructs.h"
#include "RenderedTexture.h"
#include "DepthTexture.h"
#include "JsonPipeline.h"
#include "GameObject.h"
#include "json.h"

class JsonRenderPass
{
	friend class JsonPipeline;
private:
	ivec2 RenderPassResolution;
	VkSampleCountFlagBits SampleCount;

	VkRenderPass RenderPass = VK_NULL_HANDLE;
	std::vector<VkCommandBuffer> CommandBufferList;
	std::vector<VkFramebuffer> FrameBufferList;

	void BuildRenderPass(RenderPassBuildInfoModel renderPassBuildInfo);
	void BuildFrameBuffer();

	List<SharedPtr<JsonPipeline>> JsonPipelineList;

	JsonRenderPass(String jsonPath, ivec2 renderPassResolution);

public:
	String Name;
	List<SharedPtr<RenderedTexture>> RenderedColorTextureList = List<SharedPtr<RenderedTexture>>();
	SharedPtr<DepthTexture> depthTexture;

	JsonRenderPass();
	JsonRenderPass(const JsonRenderPass& df);
	virtual ~JsonRenderPass();
	static SharedPtr<JsonRenderPass> JsonCreateRenderPass(String JsonPath, ivec2 RenderPassResolution);
	VkCommandBuffer Draw(List<SharedPtr<GameObject>> meshList, SceneDataBuffer& sceneProperties);

	void Destroy();
};

