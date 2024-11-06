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
private:
	ivec2 RenderPassResolution;
	VkSampleCountFlagBits SampleCount;

	VkRenderPass RenderPass = VK_NULL_HANDLE;
	std::vector<VkCommandBuffer> CommandBufferList;
	std::vector<VkFramebuffer> FrameBufferList;

	void BuildRenderPass(RenderPassBuildInfoModel renderPassBuildInfo);
	void BuildFrameBuffer();

	VkAttachmentDescription JsonToVulkanAttachmentDescription(nlohmann::json& json);
	VkImageCreateInfo JsonToVulkanImageCreateInfo(nlohmann::json& json);
	VkSamplerCreateInfo JsonToVulkanSamplerCreateInfo(nlohmann::json& json);
	VkSubpassDependency JsonToVulkanSubpassDependency(nlohmann::json& json);

	RenderedTextureInfoModel JsonToRenderedTextureInfoModel(nlohmann::json& json);
	RenderPassBuildInfoModel JsonToRenderPassBuildInfoModel(nlohmann::json& json);

	List<std::shared_ptr<JsonPipeline>> JsonPipelineList;

	JsonRenderPass(String jsonPath, ivec2 renderPassResolution);

public:
	String RenderPassName;
	List<std::shared_ptr<RenderedTexture>> RenderedColorTextureList = List<std::shared_ptr<RenderedTexture>>();
	std::shared_ptr<DepthTexture> depthTexture;

	JsonRenderPass();
	JsonRenderPass(const JsonRenderPass& df);
	virtual ~JsonRenderPass();
	static std::shared_ptr<JsonRenderPass> JsonCreateRenderPass(String JsonPath, ivec2 RenderPassResolution);
	VkCommandBuffer Draw(List<std::shared_ptr<GameObject>> meshList, SceneDataBuffer& sceneProperties);

	void Destroy();
};

