#pragma once
#include <vulkan/vulkan_core.h>
#include "json.h"
#include "Typedef.h"
#include "JsonStructs.h"
#include "RenderedTexture.h"
#include "DepthTexture.h"
#include "JsonPipeline.h"
#include "GameObject.h"

class JsonRenderPass
{
	friend class JsonPipeline;

protected:
	ivec2 RenderPassResolution;
	VkSampleCountFlagBits SampleCount;

	VkRenderPass RenderPass = VK_NULL_HANDLE;
	VkCommandBuffer CommandBuffer;
	std::vector<VkFramebuffer> FrameBufferList;

	virtual void BuildRenderPass(RenderPassBuildInfoModel renderPassBuildInfo);
	virtual void BuildFrameBuffer();

	Vector<SharedPtr<JsonPipeline>> JsonPipelineList;

	JsonRenderPass(String jsonPath, ivec2 renderPassResolution);

public:
	String Name;
	Vector<SharedPtr<RenderedTexture>> RenderedColorTextureList = Vector<SharedPtr<RenderedTexture>>();
	SharedPtr<DepthTexture> depthTexture;

	JsonRenderPass();
	static SharedPtr<JsonRenderPass> JsonCreateRenderPass(String JsonPath, ivec2 RenderPassResolution);
	virtual ~JsonRenderPass();

	virtual void Update(const float& deltaTime);
	virtual VkCommandBuffer Draw(Vector<SharedPtr<GameObject>> meshList, SceneDataBuffer& sceneProperties);
	virtual void Destroy();
};

